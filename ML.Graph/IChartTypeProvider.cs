using System.Windows.Forms.DataVisualization.Charting;

namespace ML.Graph
{
    public interface IChartTypeProvider
    {
        SeriesChartType GetDefaultChartType(string classificationResult);
        string[] GetAvailableChartTypes(string classificationResult);
        SeriesChartType GetChartTypeFromName(string chartName);
    }
}