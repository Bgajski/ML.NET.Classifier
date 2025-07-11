using System.Windows.Forms.DataVisualization.Charting;

namespace ML.Graph
{
    /// <summary>
    /// defines chart type selection logic based on classification result
    /// </summary>
    public interface IChartTypeProvider
    {
        SeriesChartType GetDefaultChartType(string classificationResult);
        string[] GetAvailableChartTypes(string classificationResult);
        SeriesChartType GetChartTypeFromName(string chartName);
    }
}
