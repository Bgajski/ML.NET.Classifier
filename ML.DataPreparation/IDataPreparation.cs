using Microsoft.ML;

public interface IDataPreparation
{
    // prepares generic data for training/testing
    (IDataView TrainingData, IDataView TestData) PrepareData(MLContext mlContext, string filePath, int featureCount);
}

// extends data preparation interface with binary-specific model options
public interface IBinaryDataPreparation : IDataPreparation
{
    // prepares binary data with weighting suitable for logistic regression
    (IDataView TrainingData, IDataView TestData) PrepareDataForLogisticRegression(MLContext mlContext, string filePath, int featureCount);

    // prepares binary data without weighting for perceptron-based models
    (IDataView TrainingData, IDataView TestData) PrepareDataForAveragedPerceptron(MLContext mlContext, string filePath, int featureCount);
}

// textual classification: label + input text as features
public interface ITextualDataPreparation : IDataPreparation
{
    (IDataView TrainingData, IDataView TestData) PrepareDataForNaiveBayes(MLContext mlContext, string filePath, int featureCount);
    (IDataView TrainingData, IDataView TestData) PrepareDataForFastForest(MLContext mlContext, string filePath, int featureCount);
}
