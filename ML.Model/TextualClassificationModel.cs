using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers.FastTree;
using Microsoft.ML.Transforms.Text;

namespace ML.Model
{
    public class TextualClassificationModel : ITextualClassificationModel
    {
        private readonly MLContext _mlContext;
        private ITransformer _trainedModel;

        public TextualClassificationModel(MLContext mlContext)
        {
            _mlContext = mlContext;
        }

        public ITransformer TrainFastForest(IDataView trainingData)
        {
            var pipeline = _mlContext.Transforms.Conversion.MapValueToKey("Label")
                .Append(_mlContext.Transforms.Text.FeaturizeText("Features", "Text"))
                .AppendCacheCheckpoint(_mlContext)
                .Append(_mlContext.MulticlassClassification.Trainers.OneVersusAll(
                    binaryEstimator: _mlContext.BinaryClassification.Trainers.FastForest(new FastForestBinaryTrainer.Options
                    {
                        NumberOfLeaves = 50,
                        NumberOfTrees = 100,
                        MinimumExampleCountPerLeaf = 1
                    }), labelColumnName: "Label"))
                .Append(_mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));

            _trainedModel = pipeline.Fit(trainingData);
            return _trainedModel;
        }

        public ITransformer TrainNaiveBayes(IDataView trainingData)
        {
            var ngramLengths = new int[] { 1, 2, 3 };
            ITransformer bestModel = null;
            double bestMicroAccuracy = 0;

            foreach (var ngramLength in ngramLengths)
            {
                var pipeline = _mlContext.Transforms.Conversion.MapValueToKey("Label")
                    .Append(_mlContext.Transforms.Text.FeaturizeText("Features", new TextFeaturizingEstimator.Options
                    {
                        WordFeatureExtractor = new WordBagEstimator.Options
                        {
                            NgramLength = ngramLength,
                            UseAllLengths = true,
                            Weighting = NgramExtractingEstimator.WeightingCriteria.TfIdf
                        },
                        CharFeatureExtractor = new WordBagEstimator.Options
                        {
                            NgramLength = 3,
                            UseAllLengths = false,
                            Weighting = NgramExtractingEstimator.WeightingCriteria.TfIdf
                        },
                        Norm = TextFeaturizingEstimator.NormFunction.L2,
                        KeepPunctuations = false,
                        StopWordsRemoverOptions = new StopWordsRemovingEstimator.Options
                        {
                            Language = TextFeaturizingEstimator.Language.English
                        }
                    }, "Text"))
                    .Append(_mlContext.Transforms.NormalizeMinMax("Features"))
                    .Append(_mlContext.MulticlassClassification.Trainers.NaiveBayes(labelColumnName: "Label", featureColumnName: "Features"))
                    .Append(_mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));

                var crossValidationResults = _mlContext.MulticlassClassification.CrossValidate(trainingData, pipeline, numberOfFolds: 5);
              
                var averageMicroAccuracy = crossValidationResults.Average(cvResult => cvResult.Metrics.MicroAccuracy);

                if (averageMicroAccuracy > bestMicroAccuracy)
                {
                    bestMicroAccuracy = averageMicroAccuracy;
                    bestModel = pipeline.Fit(trainingData);
                }
            }

            _trainedModel = bestModel;
            return _trainedModel;
        }

        public MulticlassClassificationMetrics EvaluateModel(IDataView testData)
        {
            if (_trainedModel == null)
                throw new InvalidOperationException("Model has not been trained yet.");

            var predictions = _trainedModel.Transform(testData);
            var metrics = _mlContext.MulticlassClassification.Evaluate(predictions);
            return metrics;
        }

        public IDataView Transform(IDataView data)
        {
            if (_trainedModel == null)
                throw new InvalidOperationException("Model has not been trained yet.");

            return _trainedModel.Transform(data);
        }

        public (float[] probabilities, string[] actuals) GetPredictionsAndLabels(IDataView testData)
        {
            var transformedData = Transform(testData);
            var originalLabels = _mlContext.Data.CreateEnumerable<OriginalLabelData>(testData, reuseRowObject: false).ToList();
            var predictions = _mlContext.Data.CreateEnumerable<PredictionWithLabel>(transformedData, reuseRowObject: false).ToList();

            var probabilities = predictions.Select(x => x.Score.Max()).ToArray();
            var actuals = originalLabels.Select(x => x.Label).ToArray();

            return (probabilities, actuals);
        }

        public class PredictionWithLabel
        {
            [ColumnName("Score")]
            public float[] Score { get; set; }
            [ColumnName("Label")]
            public uint Label { get; set; }
        }

        public class OriginalLabelData
        {
            [ColumnName("Label")]
            public string Label { get; set; }
        }
    }
}