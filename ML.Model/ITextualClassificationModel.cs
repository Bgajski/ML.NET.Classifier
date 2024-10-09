using Microsoft.ML;
using Microsoft.ML.Data;

namespace ML.Model
{
    public interface ITextualClassificationModel
    {
        ITransformer TrainFastForest(IDataView trainingData);
        ITransformer TrainNaiveBayes(IDataView trainingData);
        MulticlassClassificationMetrics EvaluateModel(IDataView testData);
        IDataView Transform(IDataView data);
        (float[] probabilities, string[] actuals) GetPredictionsAndLabels(IDataView testData);
    }
}