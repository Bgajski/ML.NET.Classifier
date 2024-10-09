using Microsoft.ML.Data;
using System.Windows.Forms.DataVisualization.Charting;

public interface IPerformanceVisualizer
{
    void VisualizeReceiverCurve(IEnumerable<float> probabilities, IEnumerable<bool> actuals, Chart chart, string modelName);
    void VisualizeConfusionMatrix(MulticlassClassificationMetrics metrics, Chart chart, string modelName);
    void VisualizePerformanceMetrics(BinaryClassificationMetrics metrics, Chart chart, string modelName);
    void VisualizeCumulativeGainsChart(float[] probabilities, string[] actuals, Chart chart, string modelName);
}