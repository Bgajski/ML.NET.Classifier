using Microsoft.ML.Data;
using ML.Performance;
using System.Windows.Forms.DataVisualization.Charting;

public class PerformanceVisualizer : IPerformanceVisualizer
{
    public void VisualizeReceiverCurve(IEnumerable<float> probabilities, IEnumerable<bool> actuals, Chart chart, string modelName)
    {
        ReceiverCurveCharacteristic.ShowRecieverCurve(probabilities, actuals, chart, modelName);
    }

    public void VisualizeConfusionMatrix(MulticlassClassificationMetrics metrics, Chart chart, string modelName)
    {
        ConfusionMatrixDisplay.ShowMetricsChart(metrics, chart, modelName);
    }

    public void VisualizePerformanceMetrics(BinaryClassificationMetrics metrics, Chart chart, string modelName)
    {
        PredictionPerformanceEvaluation.ShowPerformanceMetrics(metrics, chart, modelName);
    }

    public void VisualizeCumulativeGainsChart(float[] probabilities, string[] actuals, Chart chart, string modelName)
    {
        CumulativeGainsChart.ShowCumulativeGainsChart(probabilities, actuals, chart, modelName);
    }
}