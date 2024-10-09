using Microsoft.ML.Data;

public interface IPerformanceDescription
{
    string GetReceiverCurveDescription();
    string GetPredictionDistributionDescription();
    string GetConfusionMatrixDescription();
    string GetCumulativeGainsChartDescription();
    List<(string text, Color color)> GetMetricsReceiverCurve(BinaryClassificationMetrics metrics);
    List<(string text, Color color)> GetMetricsPredictionDistribution(BinaryClassificationMetrics metrics);
    List<(string text, Color color)> GetMetricsConfusionMatrix(MulticlassClassificationMetrics metrics);
    List<(string text, Color color)> GetMetricsCumulativeGainsChart(MulticlassClassificationMetrics metrics);
}