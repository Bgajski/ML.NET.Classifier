using Microsoft.ML.Data;
using System.Collections.Generic;
using System.Windows.Forms.DataVisualization.Charting;

/// <summary>
/// defines visual rendering for multiple performance charts
/// </summary>
public interface IPerformanceVisualizer
{
    // draws receiver operating characteristic (roc) curve
    void VisualizeReceiverCurve(IEnumerable<float> probabilities, IEnumerable<bool> actuals, Chart chart, string modelName);

    // draws bar chart from multiclass confusion matrix
    void VisualizeConfusionMatrix(MulticlassClassificationMetrics metrics, Chart chart, string modelName);

    // displays binary metrics (accuracy, auc, f1) as performance bars
    void VisualizePerformanceMetrics(BinaryClassificationMetrics metrics, Chart chart, string modelName);

    // draws cumulative gains chart for ham/spam separation
    void VisualizeCumulativeGainsChart(float[] probabilities, string[] actuals, Chart chart, string modelName);
}
