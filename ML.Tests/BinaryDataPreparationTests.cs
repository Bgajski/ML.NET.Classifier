using Microsoft.ML;
using NUnit.Framework;
using ML.DataPreparation;

namespace ML.Tests
{
    /// <summary>
    /// tests for verifying feature column name generation in binary data preparation
    /// </summary>
    [TestFixture]
    public class BinaryDataPreparationTests
    {
        private MLContext _mlContext;
        private string _dataFilePath;

        [SetUp]
        public void SetUp()
        {
            _mlContext = new MLContext();
            _dataFilePath = "path/to/your/binary_data.csv"; // optional
        }

        [Test]
        public void GenerateFeatureColumnNames_CreatesCorrectFeatureColumnNames()
        {
            // arrange test input
            int featureCount = 4;

            // act - generate feature names
            var featureColumnNames = BinaryDataPreparation.GenerateFeatureColumnNames(featureCount);

            // assert - check expected count and naming pattern
            Assert.AreEqual(featureCount, featureColumnNames.Length, "Should return exactly 'featureCount' names.");
            for (int i = 0; i < featureCount; i++)
            {
                Assert.AreEqual($"F{i}", featureColumnNames[i], $"Feature name at index {i} should be 'F{i}'");
            }
        }
    }
}
