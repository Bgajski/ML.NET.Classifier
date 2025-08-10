using Microsoft.ML.Data;
using System.Drawing;

/// <summary>
/// defines textual descriptions and color-coded metrics for various performance visualizations
/// </summary>
public interface IPerformanceDescription
{
    // returns textual summary for receiver operating characteristic curve
    string GetReceiverCurveDescription();

    // returns summary for prediction distribution histogram
    string GetPredictionDistributionDescription();

    // returns textual explanation of confusion matrix content
    string GetConfusionMatrixDescription();

    // returns description of cumulative gains curve
    string GetCumulativeGainsChartDescription();

    // returns list of metrics to annotate receiver curve
    List<(string text, Color color)> GetMetricsReceiverCurve(BinaryClassificationMetrics metrics);

    // returns list of metrics to annotate prediction distribution chart
    List<(string text, Color color)> GetMetricsPredictionDistribution(BinaryClassificationMetrics metrics);

    // returns key values extracted from multiclass confusion matrix
    List<(string text, Color color)> GetMetricsConfusionMatrix(MulticlassClassificationMetrics metrics);

    // returns list of color-coded metrics for cumulative gains visualization
    List<(string text, Color color)> GetMetricsCumulativeGainsChart(MulticlassClassificationMetrics metrics);
}
