using System.Data;
using System.Windows.Forms.DataVisualization.Charting;

namespace ML.Graph
{
    public interface IChartDisplay
    {
        void DisplayDataOnChart(Chart chart, DataTable dataTable, SeriesChartType chartType, string column);
    }
}