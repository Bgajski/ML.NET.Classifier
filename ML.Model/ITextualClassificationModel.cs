using Microsoft.ML;
using Microsoft.ML.Data;

namespace ML.Model
{
    /// <summary>
    /// interface for textual (multiclass) classification models.
    /// supports training using fast forest and naive bayes.
    /// </summary>
    public interface ITextualClassificationModel
    {
        // trains fast forest model on textual data
        ITransformer TrainFastForest(IDataView trainingData);

        // trains naive bayes model on textual data
        ITransformer TrainNaiveBayes(IDataView trainingData);

        // evaluates trained model using multiclass metrics
        MulticlassClassificationMetrics EvaluateModel(IDataView testData);

        // transforms input data using trained model
        IDataView Transform(IDataView data);

        // extracts class probabilities and actual string labels
        (float[] probabilities, string[] actuals) GetPredictionsAndLabels(IDataView testData);
    }
}
