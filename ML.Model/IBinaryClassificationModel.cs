using Microsoft.ML;
using Microsoft.ML.Data;

public interface IBinaryClassificationModel
{
    ITransformer TrainLogisticRegression(IDataView trainingData);
    BinaryClassificationMetrics EvaluateModel(IDataView testData);
    ITransformer TrainAveragedPerceptron(IDataView trainingData);
    IDataView Transform(IDataView data);
}