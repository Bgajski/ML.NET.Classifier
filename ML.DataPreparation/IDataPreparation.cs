using Microsoft.ML;

public interface IDataPreparation
{
    (IDataView TrainingData, IDataView TestData) PrepareData(MLContext mlContext, string filePath, int featureCount);
}

public interface IBinaryDataPreparation : IDataPreparation
{
    (IDataView TrainingData, IDataView TestData) PrepareDataForLogisticRegression(MLContext mlContext, string filePath, int featureCount);
    (IDataView TrainingData, IDataView TestData) PrepareDataForAveragedPerceptron(MLContext mlContext, string filePath, int featureCount);
}

public interface ITextualDataPreparation : IDataPreparation
{
    (IDataView TrainingData, IDataView TestData) PrepareDataForNaiveBayes(MLContext mlContext, string filePath, int featureCount);
    (IDataView TrainingData, IDataView TestData) PrepareDataForFastForest(MLContext mlContext, string filePath, int featureCount);
}