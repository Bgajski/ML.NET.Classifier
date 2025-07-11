using Microsoft.ML;
using Microsoft.ML.Data;

namespace ML.DataPreparation
{
    /// <summary>
    /// prepares datasets for text classification tasks (label + free text column)
    /// supports filtering, cleaning, balancing, and multiple algorithm types
    /// </summary>
    public class TextualDataPreparation : ITextualDataPreparation
    {
        /// <summary>
        /// default data preparation method (no label filtering)
        /// </summary>
        public (IDataView TrainingData, IDataView TestData) PrepareData(MLContext mlContext, string filePath, int featureCount)
        {
            var labelsToKeep = new string[0];
            return PrepareData(mlContext, filePath, featureCount, labelsToKeep);
        }

        /// <summary>
        /// main preparation logic for textual classification
        /// loads CSV, removes empty rows, optionally filters labels, balances training data
        /// </summary>
        public (IDataView TrainingData, IDataView TestData) PrepareData(MLContext mlContext, string filePath, int featureCount, string[] labelsToKeep)
        {
            // configure loader for first two columns: Label and Text
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

            // load raw text data
            var dataView = loader.Load(filePath);

            // remove rows with missing label or text
            dataView = RemoveEmptyRows(mlContext, dataView);

            // optional label filtering
            if (labelsToKeep != null && labelsToKeep.Length > 0)
            {
                dataView = FilterLabels(mlContext, dataView, labelsToKeep);
            }

            // split into training and test sets
            var split = mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);

            // apply class balancing to training set
            var trainingData = BalanceData(mlContext, split.TrainSet);
            return (trainingData, split.TestSet);
        }

        /// <summary>
        /// removes rows where either label or text is missing
        /// </summary>
        public IDataView RemoveEmptyRows(MLContext mlContext, IDataView dataView)
        {
            var data = mlContext.Data.CreateEnumerable<OriginalLabelData>(dataView, reuseRowObject: false).ToList();
            var filteredData = data.Where(row => !string.IsNullOrEmpty(row.Label) && !string.IsNullOrEmpty(row.Text)).ToList();
            return mlContext.Data.LoadFromEnumerable(filteredData);
        }

        /// <summary>
        /// filters dataset to include only rows with specified label values
        /// </summary>
        public IDataView FilterLabels(MLContext mlContext, IDataView dataView, string[] labelsToKeep)
        {
            var data = mlContext.Data.CreateEnumerable<OriginalLabelData>(dataView, reuseRowObject: false).ToList();
            var filteredData = data.Where(row => labelsToKeep.Contains(row.Label)).ToList();
            return mlContext.Data.LoadFromEnumerable(filteredData);
        }

        /// <summary>
        /// balances training set by oversampling minority classes
        /// ensures all classes appear approximately equally
        /// </summary>
        public IDataView BalanceData(MLContext mlContext, IDataView trainSet)
        {
            var sampledData = mlContext.Data.CreateEnumerable<OriginalLabelData>(trainSet, reuseRowObject: false).ToList();
            var labelCounts = sampledData.GroupBy(x => x.Label).ToDictionary(g => g.Key, g => g.Count());
            var maxCount = labelCounts.Values.Max();
            var balancedData = new List<OriginalLabelData>();

            foreach (var label in labelCounts.Keys)
            {
                var items = sampledData.Where(x => x.Label == label).ToList();
                // duplicate class samples until all labels are equally represented
                while (items.Count < maxCount)
                {
                    items.AddRange(items.Take(maxCount - items.Count));
                }
                balancedData.AddRange(items);
            }

            return mlContext.Data.LoadFromEnumerable(balancedData);
        }

        /// <summary>
        /// preparation pipeline for naive bayes classifier
        /// </summary>
        public (IDataView TrainingData, IDataView TestData) PrepareDataForNaiveBayes(MLContext mlContext, string filePath, int featureCount)
        {
            var labelsToKeep = new string[0];
            return PrepareData(mlContext, filePath, featureCount, labelsToKeep);
        }

        /// <summary>
        /// preparation pipeline for fast forest classifier
        /// </summary>
        public (IDataView TrainingData, IDataView TestData) PrepareDataForFastForest(MLContext mlContext, string filePath, int featureCount)
        {
            var labelsToKeep = new string[0];
            return PrepareData(mlContext, filePath, featureCount, labelsToKeep);
        }

        // internal class used for label/text schema binding
        public class OriginalLabelData
        {
            public string Label { get; set; }
            public string Text { get; set; }
        }
    }
}
