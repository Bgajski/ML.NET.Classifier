using Microsoft.ML;
using Microsoft.ML.Data;
using ML.DataPreparation;

namespace ML.Tests
{
    [TestFixture]
    public class BinaryDataPreparationTests
    {
        public MLContext _mlContext;
        public string _dataFilePath;

        [SetUp]
        public void SetUp()
        {
            _mlContext = new MLContext();
            _dataFilePath = "path/to/your/binary_data.csv";
        }

        [Test]
        public void GenerateColumns_CreatesCorrectColumns()
        {
            int featureCount = 2;
            var featureKind = DataKind.Single;
            var labelKind = DataKind.Boolean;
            var binaryDataPreparation = new BinaryDataPreparation();

            var columns = binaryDataPreparation.GenerateColumns(featureCount, featureKind, labelKind);

            Assert.AreEqual(featureCount + 1, columns.Length, "Number of columns should be featureCount + 1.");
            for (int i = 0; i < featureCount; i++)
            {
                Assert.AreEqual($"Feature{i}", columns[i].Name, $"Column name should be Feature{i}.");
                Assert.AreEqual(featureKind, columns[i].DataKind, $"Column data kind should be {featureKind}.");
            }
            Assert.AreEqual("Label", columns[featureCount].Name, "Last column name should be Label.");
            Assert.AreEqual(labelKind, columns[featureCount].DataKind, "Last column data kind should be Label kind.");
        }

        [Test]
        public void GenerateFeatureColumnNames_CreatesCorrectFeatureColumnNames()
        {
            int featureCount = 3;
            var binaryDataPreparation = new BinaryDataPreparation();

            var featureColumnNames = binaryDataPreparation.GenerateFeatureColumnNames(featureCount);

            Assert.AreEqual(featureCount, featureColumnNames.Length, "Number of feature column names should match feature count.");
            for (int i = 0; i < featureCount; i++)
            {
                Assert.AreEqual($"Feature{i}", featureColumnNames[i], $"Feature column name should be Feature{i}.");
            }
        }
    }
}
