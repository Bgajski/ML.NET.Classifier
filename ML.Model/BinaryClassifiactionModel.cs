using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;

public class BinaryClassificationModel : IBinaryClassificationModel
{
    private readonly MLContext _mlContext;
    private ITransformer _trainedModel;

    public BinaryClassificationModel(MLContext mlContext)
    {
        _mlContext = mlContext;
    }

    public ITransformer TrainLogisticRegression(IDataView trainingData)
    {
        var pipeline = _mlContext.Transforms.Concatenate("Features", "Features")
            .Append(_mlContext.Transforms.NormalizeMinMax("Features"))
            .Append(_mlContext.BinaryClassification.Trainers.LbfgsLogisticRegression(new LbfgsLogisticRegressionBinaryTrainer.Options
            {
                MaximumNumberOfIterations = 100,
                L2Regularization = 0.1f
            }));

        _trainedModel = pipeline.Fit(trainingData);
        return _trainedModel;
    }

    public ITransformer TrainAveragedPerceptron(IDataView trainingData)
    {
        var pipeline = _mlContext.Transforms.Concatenate("Features", "Features")
            .Append(_mlContext.Transforms.NormalizeMinMax("Features"))
            .Append(_mlContext.BinaryClassification.Trainers.AveragedPerceptron(new AveragedPerceptronTrainer.Options
            {
                LearningRate = 0.1f,
                NumberOfIterations = 10
            }));

        var trainedModel = pipeline.Fit(trainingData);

        var scoredData = trainedModel.Transform(trainingData);
        var calibratorEstimator = _mlContext.BinaryClassification.Calibrators.Platt();
        var calibratorTransformer = calibratorEstimator.Fit(scoredData);

        _trainedModel = new TransformerChain<ITransformer>(trainedModel, calibratorTransformer);

        return _trainedModel;
    }

    public BinaryClassificationMetrics EvaluateModel(IDataView testData)
    {
        if (_trainedModel == null)
            throw new InvalidOperationException("The model has not been trained yet.");

        var predictions = _trainedModel.Transform(testData);
        var metrics = _mlContext.BinaryClassification.Evaluate(predictions);
        return metrics;
    }

    public IDataView Transform(IDataView data)
    {
        if (_trainedModel == null)
            throw new InvalidOperationException("The model has not been trained yet.");

        return _trainedModel.Transform(data);
    }

    public (float[] probabilities, bool[] actuals) GetPredictionsAndLabels(IDataView testData)
    {
        var transformedData = Transform(testData);
        var probabilities = transformedData.GetColumn<float>("Score").ToArray();
        var actuals = transformedData.GetColumn<bool>("Label").ToArray();
        return (probabilities, actuals);
    }
}