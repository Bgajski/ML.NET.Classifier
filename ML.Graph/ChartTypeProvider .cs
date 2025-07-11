using System.Windows.Forms.DataVisualization.Charting;

namespace ML.Graph
{
    /// <summary>
    /// provides default and available chart types depending on classification result
    /// </summary>
    public class ChartTypeProvider : IChartTypeProvider
    {
        public SeriesChartType GetDefaultChartType(string classificationResult)
        {
            if (classificationResult.Contains("binary classification"))
                return SeriesChartType.Column;
            else if (classificationResult.Contains("textual classification"))
                return SeriesChartType.Pie;
            else
                return SeriesChartType.Column;
        }

        public string[] GetAvailableChartTypes(string classificationResult)
        {
            if (classificationResult.Contains("binary classification"))
                return new[] { "Column Chart (vertical)", "Bar Chart (horizontal)" };
            else if (classificationResult.Contains("textual classification"))
                return new[] { "Pie Chart", "Doughnut Chart" };
            else
                return new string[0];
        }

        public SeriesChartType GetChartTypeFromName(string chartName)
        {
            return chartName switch
            {
                "Column Chart (vertical)" => SeriesChartType.Column,
                "Bar Chart (horizontal)" => SeriesChartType.Bar,
                "Pie Chart" => SeriesChartType.Pie,
                "Doughnut Chart" => SeriesChartType.Doughnut,
            };
        }
    }
}
