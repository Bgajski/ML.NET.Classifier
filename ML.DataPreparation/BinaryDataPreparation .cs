using Microsoft.ML;
using Microsoft.ML.Data;

namespace ML.DataPreparation
{
    public class BinaryDataPreparation : IBinaryDataPreparation
    {
        public (IDataView TrainingData, IDataView TestData) PrepareData(MLContext mlContext, string filePath, int featureCount)
        {
            return PrepareDataForLogisticRegression(mlContext, filePath, featureCount);
        }

        public IDataView CleanData(MLContext mlContext, IDataView dataView)
        {
            var missingValuesFilter = mlContext.Data.FilterRowsByMissingValues(dataView);

            var columnNames = dataView.Schema.Select(column => column.Name).ToArray();
            var cleaningPipeline = mlContext.Transforms.ReplaceMissingValues(columnNames.Select(name => new InputOutputColumnPair(name)).ToArray());

            var cleanedData = cleaningPipeline.Fit(missingValuesFilter).Transform(missingValuesFilter);

            return cleanedData;
        }

        public (IDataView TrainingData, IDataView TestData) PrepareDataForLogisticRegression(MLContext mlContext, string filePath, int featureCount)
        {
            if (mlContext == null) throw new ArgumentNullException(nameof(mlContext));
            if (string.IsNullOrEmpty(filePath)) throw new ArgumentNullException(nameof(filePath));

            var loader = mlContext.Data.CreateTextLoader(new TextLoader.Options
            {
                Separators = new[] { ',' },
                HasHeader = true,
                Columns = GenerateColumns(featureCount, DataKind.Single, DataKind.Boolean)
            });

            var dataView = loader.Load(filePath);

            var pipeline = mlContext.Transforms.Concatenate("Features", GenerateFeatureColumnNames(featureCount));
            var transformedData = pipeline.Fit(dataView).Transform(dataView);

            var split = mlContext.Data.TrainTestSplit(transformedData, testFraction: 0.2);
            return (split.TrainSet, split.TestSet);
        }

        public (IDataView TrainingData, IDataView TestData) PrepareDataForAveragedPerceptron(MLContext mlContext, string filePath, int featureCount)
        {
            return PrepareDataForLogisticRegression(mlContext, filePath, featureCount);
        }

        public TextLoader.Column[] GenerateColumns(int featureCount, DataKind featureKind, DataKind labelKind)
        {
            var columns = new TextLoader.Column[featureCount + 1];
            for (int i = 0; i < featureCount; i++)
            {
                columns[i] = new TextLoader.Column($"Feature{i}", featureKind, i);
            }
            columns[featureCount] = new TextLoader.Column("Label", labelKind, featureCount);
            return columns;
        }

        public string[] GenerateFeatureColumnNames(int featureCount)
        {
            var featureColumnNames = new string[featureCount];
            for (int i = 0; i < featureCount; i++)
            {
                featureColumnNames[i] = $"Feature{i}";
            }
            return featureColumnNames;
        }
    }
}