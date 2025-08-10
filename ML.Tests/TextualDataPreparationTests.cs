using Microsoft.ML;
using ML.DataPreparation;

namespace ML.Tests
{
    /// <summary>
    /// tests for preparing textual data including basic filtering and splitting
    /// </summary>
    [TestFixture]
    public class TextualDataPreparationTests
    {
        private MLContext _mlContext;
        private string _dataFilePath;

        [SetUp]
        public void SetUp()
        {
            _mlContext = new MLContext();
            _dataFilePath = "textual_data.csv";
        }

        [Test]
        public void PrepareData_TransformsTextDataCorrectly()
        {
            var textualDataPreparation = new TextualDataPreparation();

            // create dummy data for testing
            var data = new[]
            {
                new { Label = "positive", Text = "Lorem ipsum dolor sit amet" },
                new { Label = "negative", Text = "Sed ut perspiciatis error sit voluptatem" },
                new { Label = "positive", Text = "Ut enim ad minima veniam et nostrum" },
                new { Label = "negative", Text = "Nemo enim ipsam quia voluptas sit" },
                new { Label = "positive", Text = "Quis autem vel eum iure reprehenderit" },
                new { Label = "negative", Text = "Excepteur sint occaecat non proident" }
            };

            // save data to temporary csv file
            File.WriteAllLines(_dataFilePath, new[] {
                "Label,Text",
                "positive,Lorem ipsum dolor sit amet",
                "negative,Sed ut perspiciatis error sit voluptatem",
                "positive,Ut enim ad minima veniam et nostrum",
                "negative,Nemo enim ipsam quia voluptas sit",
                "positive,Quis autem vel eum iure reprehenderit",
                "negative,Excepteur sint occaecat non proident"
            });

            // act - prepare training and test data
            var (trainingData, testData) = textualDataPreparation.PrepareData(_mlContext, _dataFilePath, 1);

            // assert - data views should not be null
            Assert.IsNotNull(trainingData, "Training data should not be null.");
            Assert.IsNotNull(testData, "Test data should not be null.");

            // preview both datasets for basic visibility
            var trainingDataPreview = trainingData.Preview();
            var testDataPreview = testData.Preview();

            Console.WriteLine("Training Data Preview:");
            foreach (var row in trainingDataPreview.RowView)
            {
                Console.WriteLine(string.Join(", ", row.Values.Select(v => v.Value)));
            }

            Console.WriteLine("Test Data Preview:");
            foreach (var row in testDataPreview.RowView)
            {
                Console.WriteLine(string.Join(", ", row.Values.Select(v => v.Value)));
            }

            // assert - data should not be empty
            Assert.IsTrue(trainingDataPreview.RowView.Length > 0, "Training data should contain rows.");
            Assert.IsTrue(testDataPreview.RowView.Length > 0, "Test data should contain rows.");
        }
    }
}
