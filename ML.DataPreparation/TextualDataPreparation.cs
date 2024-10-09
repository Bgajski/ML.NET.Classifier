using Microsoft.ML;
using Microsoft.ML.Data;

namespace ML.DataPreparation
{
    public class TextualDataPreparation : ITextualDataPreparation
    {
        public (IDataView TrainingData, IDataView TestData) PrepareData(MLContext mlContext, string filePath, int featureCount)
        {
            var labelsToKeep = new string[0];
            return PrepareData(mlContext, filePath, featureCount, labelsToKeep);
        }

        public (IDataView TrainingData, IDataView TestData) PrepareData(MLContext mlContext, string filePath, int featureCount, string[] labelsToKeep)
        {
            var loader = mlContext.Data.CreateTextLoader(new TextLoader.Options
            {
                Separators = new[] { ',' },
                HasHeader = true,
                Columns = new[]
                {
                    new TextLoader.Column("Label", DataKind.String, 0),
                    new TextLoader.Column("Text", DataKind.String, 1)
                }
            });

            var dataView = loader.Load(filePath);

            dataView = RemoveEmptyRows(mlContext, dataView);

            if (labelsToKeep != null && labelsToKeep.Length > 0)
            {
                dataView = FilterLabels(mlContext, dataView, labelsToKeep);
            }

            var split = mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);
            var trainingData = BalanceData(mlContext, split.TrainSet);
            return (trainingData, split.TestSet);
        }

        public IDataView RemoveEmptyRows(MLContext mlContext, IDataView dataView)
        {
            var data = mlContext.Data.CreateEnumerable<OriginalLabelData>(dataView, reuseRowObject: false).ToList();
            var filteredData = data.Where(row => !string.IsNullOrEmpty(row.Label) && !string.IsNullOrEmpty(row.Text)).ToList();
            return mlContext.Data.LoadFromEnumerable(filteredData);
        }

        public IDataView FilterLabels(MLContext mlContext, IDataView dataView, string[] labelsToKeep)
        {
            var data = mlContext.Data.CreateEnumerable<OriginalLabelData>(dataView, reuseRowObject: false).ToList();
            var filteredData = data.Where(row => labelsToKeep.Contains(row.Label)).ToList();
            return mlContext.Data.LoadFromEnumerable(filteredData);
        }

        public IDataView BalanceData(MLContext mlContext, IDataView trainSet)
        {
            var sampledData = mlContext.Data.CreateEnumerable<OriginalLabelData>(trainSet, reuseRowObject: false).ToList();
            var labelCounts = sampledData.GroupBy(x => x.Label).ToDictionary(g => g.Key, g => g.Count());
            var maxCount = labelCounts.Values.Max();
            var balancedData = new List<OriginalLabelData>();

            foreach (var label in labelCounts.Keys)
            {
                var items = sampledData.Where(x => x.Label == label).ToList();
                while (items.Count < maxCount)
                {
                    items.AddRange(items.Take(maxCount - items.Count));
                }
                balancedData.AddRange(items);
            }

            return mlContext.Data.LoadFromEnumerable(balancedData);
        }

        public (IDataView TrainingData, IDataView TestData) PrepareDataForNaiveBayes(MLContext mlContext, string filePath, int featureCount)
        {
            var labelsToKeep = new string[0]; 
            return PrepareData(mlContext, filePath, featureCount, labelsToKeep);
        }

        public (IDataView TrainingData, IDataView TestData) PrepareDataForFastForest(MLContext mlContext, string filePath, int featureCount)
        {
            var labelsToKeep = new string[0]; 
            return PrepareData(mlContext, filePath, featureCount, labelsToKeep);
        }

        public class OriginalLabelData
        {
            public string Label { get; set; }
            public string Text { get; set; }
        }
    }
}