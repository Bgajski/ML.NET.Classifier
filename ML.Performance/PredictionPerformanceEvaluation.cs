using Microsoft.ML.Data;
using System.Windows.Forms.DataVisualization.Charting;

namespace ML.Performance
{
    /// <summary>
    /// visualizes key binary classification metrics (precision and recall for both classes)
    /// </summary>
    public class PredictionPerformanceEvaluation
    {
        // color palette for 4 performance bars: ppv, tpr, npv, tnr
        private static readonly List<Color> MetricColors = new List<Color>
        {
            Color.FromArgb(52, 152, 219),   // ppv
            Color.FromArgb(231, 76, 60),    // tpr
            Color.FromArgb(46, 204, 113),   // npv
            Color.FromArgb(155, 89, 182),   // tnr
        };

        /// <summary>
        /// draws bar chart of binary classification metrics (precision/recall per class)
        /// </summary>
        public static void ShowPerformanceMetrics(BinaryClassificationMetrics metrics, Chart chart, string modelName)
        {
            // select metrics for display with colors
            var metricsToDisplay = new List<(string Name, double Value, Color Color)>
            {
                ("Precision of Positive Predictions (PPV)", metrics.PositivePrecision, MetricColors[0]),
                ("Recall of Positive Predictions (TPR)", metrics.PositiveRecall, MetricColors[1]),
                ("Precision of Negative Predictions (NPV)", metrics.NegativePrecision, MetricColors[2]),
                ("Recall of Negative Predictions (TNR)", metrics.NegativeRecall, MetricColors[3])
            };

            // calculate y-axis max to scale bar height cleanly
            double maxValue = metricsToDisplay.Max(metric => metric.Value);
            double axisYMaximum = Math.Ceiling(maxValue * 100) / 100.0 + 0.1;

            chart.Series.Clear();

            var chartArea = new ChartArea
            {
                AxisX = { Title = "Performance Metric", TitleFont = new Font("Tahoma", 10) },
                AxisY = { Title = "Value", TitleFont = new Font("Tahoma", 10) }
            };
            chart.ChartAreas.Clear();
            chart.ChartAreas.Add(chartArea);

            // draw one bar per metric
            foreach (var metric in metricsToDisplay)
            {
                AddMetricSeries(chart, metric.Name, metric.Value, metric.Color);
            }

            chartArea.AxisX.LabelStyle.Angle = -45; // rotate label if enabled
            chartArea.AxisX.LabelStyle.Enabled = false;
            chartArea.AxisX.Interval = 1;

            chartArea.AxisY.LabelStyle.Format = "0.00";
            chartArea.AxisY.Maximum = axisYMaximum;
            chartArea.AxisY.Interval = axisYMaximum / 10;

            chartArea.AxisX.MajorGrid.LineColor = Color.LightGray;
            chartArea.AxisY.MajorGrid.LineColor = Color.LightGray;

            chart.Legends.Clear();
            chart.Legends.Add(new Legend
            {
                Docking = Docking.Right,
                Alignment = StringAlignment.Near,
                Font = new Font("Tahoma", 8),
                BackColor = Color.Transparent
            });
        }

        // adds one metric as a vertical bar to the chart
        private static void AddMetricSeries(Chart chart, string name, double value, Color color)
        {
            var series = new Series
            {
                Name = name,
                ChartType = SeriesChartType.Column,
                Color = color,
                IsValueShownAsLabel = false
            };

            series.Points.AddXY(name, value);
            chart.Series.Add(series);
        }
    }
}
