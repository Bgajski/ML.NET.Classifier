using Microsoft.ML.Data;
using ML.Performance;
using System.Windows.Forms.DataVisualization.Charting;

/// <summary>
/// implementation that delegates visual performance rendering to specialized chart classes
/// </summary>
public class PerformanceVisualizer : IPerformanceVisualizer
{
    // draw receiver curve (roc) from probabilities and ground truth
    public void VisualizeReceiverCurve(IEnumerable<float> probabilities, IEnumerable<bool> actuals, Chart chart, string modelName)
    {
        ReceiverCurveCharacteristic.ShowRecieverCurve(probabilities, actuals, chart, modelName);
    }

    // draw bar chart from confusion matrix counts
    public void VisualizeConfusionMatrix(MulticlassClassificationMetrics metrics, Chart chart, string modelName)
    {
        ConfusionMatrixDisplay.ShowMetricsChart(metrics, chart, modelName);
    }

    // show core binary metrics (accuracy, precision, recall etc)
    public void VisualizePerformanceMetrics(BinaryClassificationMetrics metrics, Chart chart, string modelName)
    {
        PredictionPerformanceEvaluation.ShowPerformanceMetrics(metrics, chart, modelName);
    }

    // draw curve showing cumulative model gain over random guessing
    public void VisualizeCumulativeGainsChart(float[] probabilities, string[] actuals, Chart chart, string modelName)
    {
        CumulativeGainsChart.ShowCumulativeGainsChart(probabilities, actuals, chart, modelName);
    }
}
