using Microsoft.ML.Data;
using System.Windows.Forms.DataVisualization.Charting;

namespace ML.Performance
{
    public class PredictionPerformanceEvaluation
    {
        private static readonly List<Color> MetricColors = new List<Color>
        {
            Color.FromArgb(52, 152, 219),   
            Color.FromArgb(231, 76, 60),    
            Color.FromArgb(46, 204, 113),    
            Color.FromArgb(155, 89, 182),    
        };

        public static void ShowPerformanceMetrics(BinaryClassificationMetrics metrics, Chart chart, string modelName)
        {
            var metricsToDisplay = new List<(string Name, double Value, Color Color)>
            {
                ("Precision of Positive Predictions (PPV)", metrics.PositivePrecision, MetricColors[0]),
                ("Recall of Positive Predictions (TPR)", metrics.PositiveRecall, MetricColors[1]),
                ("Precision of Negative Predictions (NPV)", metrics.NegativePrecision, MetricColors[2]),
                ("Recall of Negative Predictions (TNR)", metrics.NegativeRecall, MetricColors[3])
            };

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

            foreach (var metric in metricsToDisplay)
            {
                AddMetricSeries(chart, metric.Name, metric.Value, metric.Color);
            }

            chartArea.AxisX.LabelStyle.Angle = -45;
            chartArea.AxisX.LabelStyle.Enabled = false;
            chartArea.AxisX.Interval = 1;
            chartArea.AxisX.LabelStyle.Font = new Font("Tahoma", 10);

            chartArea.AxisY.LabelStyle.Format = "0.00";
            chartArea.AxisY.Maximum = axisYMaximum;
            chartArea.AxisY.Interval = axisYMaximum / 10;
            chartArea.AxisY.LabelStyle.Font = new Font("Tahoma", 10);
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