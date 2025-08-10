using System.Data;
using System.Windows.Forms.DataVisualization.Charting;

namespace ML.Graph
{
    /// <summary>
    /// interface for chart renderers (binary or textual classification visualizations)
    /// </summary>
    public interface IChartDisplay
    {
        void DisplayDataOnChart(Chart chart, DataTable dataTable, SeriesChartType chartType, string column);
    }
}
