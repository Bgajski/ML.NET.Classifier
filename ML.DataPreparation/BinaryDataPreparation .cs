using Microsoft.ML;
using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ML.DataPreparation
{
    /// <summary>
    /// prepares binary classification datasets and applies optional instance weighting
    /// used for models like logistic regression and averaged perceptron
    /// </summary>
    public class BinaryDataPreparation : IBinaryDataPreparation, IDataPreparation
    {
        public (IDataView TrainingData, IDataView TestData)
            PrepareData(MLContext ml, string path, int featureCount) =>
            Prepare(ml, path);

        public (IDataView TrainingData, IDataView TestData)
            PrepareDataForLogisticRegression(MLContext ml, string path, int featureCount) =>
            Prepare(ml, path, useWeighting: true); // use instance weighting for logistic regression

        public (IDataView TrainingData, IDataView TestData)
            PrepareDataForAveragedPerceptron(MLContext ml, string path, int featureCount) =>
            Prepare(ml, path, useWeighting: false); // no weighting for averaged perceptron

        /// <summary>
        /// internal preparation pipeline for binary data
        /// handles label detection, column parsing, normalization, and optional weighting
        /// </summary>
        private static (IDataView TrainingData, IDataView TestData) Prepare(MLContext ml, string csvPath, bool useWeighting = false)
        {
            // detect label column index based on common names or binary patterns
            string[] header = File.ReadLines(csvPath).First().Split(',');
            int labelIdx = DetectLabelIndex(csvPath, header);
            if (labelIdx < 0) throw new Exception("No binary label column found.");

            // define columns for TextLoader
            var columns = new List<TextLoader.Column>
            {
                new("Label", DataKind.Boolean, labelIdx)
            };

            // create feature columns with names F0, F1, ...
            int featOrdinal = 0;
            for (int i = 0; i < header.Length; i++)
            {
                if (i == labelIdx) continue;
                columns.Add(new($"F{featOrdinal++}", DataKind.Single, i));
            }

            // load raw CSV using configured schema
            var loader = ml.Data.CreateTextLoader(new TextLoader.Options
            {
                Columns = columns.ToArray(),
                HasHeader = true,
                Separators = new[] { ',' }
            });

            var raw = loader.Load(csvPath);

            // stratified train/test split using label column
            var split = ml.Data.TrainTestSplit(raw, testFraction: 0.2, seed: 42, samplingKeyColumnName: "Label");
            var train = split.TrainSet;
            var test = split.TestSet;

            // concatenate features and apply normalization
            string[] featCols = columns.Where(c => c.Name.StartsWith("F")).Select(c => c.Name).ToArray();

            var pipeline = ml.Transforms.Concatenate("Features", featCols)
                                       .Append(ml.Transforms.NormalizeMeanVariance("Features"));

            var transformer = pipeline.Fit(train);
            var transformedTrain = transformer.Transform(train);
            var transformedTest = transformer.Transform(test);

            // apply instance weighting if requested
            if (useWeighting)
            {
                var weightedTrain = WeightingHelper.CreateWeightEstimator(ml, transformedTrain)
                                                   .Fit(transformedTrain)
                                                   .Transform(transformedTrain);
                return (weightedTrain, transformedTest);
            }
            else
            {
                return (transformedTrain, transformedTest);
            }
        }

        /// <summary>
        /// tries to locate a binary label column using header keywords or value patterns
        /// </summary>
        private static int DetectLabelIndex(string path, string[] header)
        {
            var common = new[] { "label", "outcome", "class", "target", "y" };
            for (int i = 0; i < header.Length; i++)
                if (common.Contains(header[i], StringComparer.OrdinalIgnoreCase))
                    return i;

            // fallback: scan first 200 rows for binary values in each column
            var sample = File.ReadLines(path).Skip(1).Take(200).Select(l => l.Split(',')).ToArray();
            for (int col = 0; col < header.Length; col++)
            {
                var distinct = new HashSet<string>();
                foreach (var row in sample)
                {
                    if (col >= row.Length) continue;
                    var v = row[col].Trim().ToLowerInvariant();
                    if (!string.IsNullOrWhiteSpace(v)) distinct.Add(v);
                    if (distinct.Count > 2) break;
                }

                // check if values are binary-compatible
                if (distinct.Count <= 2 && distinct.All(IsBinary)) return col;
            }

            return -1;
        }

        // check if string represents a binary value
        private static bool IsBinary(string v) =>
            v is "0" or "1" or "false" or "true";

        // generate generic feature names: F0, F1, F2...
        public static string[] GenerateFeatureColumnNames(int n) =>
            Enumerable.Range(0, n).Select(i => $"F{i}").ToArray();

        // used for schema compatibility in ML.NET pipelines
        private sealed class LabelOut { public bool Label { get; set; } }
    }
}
