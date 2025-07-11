using Microsoft.ML;
using Microsoft.ML.Data;
using System.Collections.Generic;

/// <summary>
/// interface for binary classification model implementations.
/// defines training methods for logistic regression and averaged perceptron,
/// as well as methods for evaluation, transformation and prediction extraction.
/// </summary>
public interface IBinaryClassificationModel
{
    // trains logistic regression model
    ITransformer TrainLogisticRegression(IDataView trainingData);

    // trains averaged perceptron model
    ITransformer TrainAveragedPerceptron(IDataView trainingData);

    // evaluates trained model on given test dataset
    BinaryClassificationMetrics EvaluateModel(IDataView testData);

    // transforms data using trained model
    IDataView Transform(IDataView data);

    // extracts probability scores and actual labels from prediction
    (List<float> probs, List<bool> labels) GetPredictionsAndLabels(IDataView testData);

    // finds threshold that maximizes f1 score using validation data
    double OptimizeThresholdByF1(IDataView validationData);
}
