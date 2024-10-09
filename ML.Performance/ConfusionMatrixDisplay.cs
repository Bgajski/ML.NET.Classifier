using Microsoft.ML.Data;
using System.Windows.Forms.DataVisualization.Charting;

namespace ML.Performance
{
    public class ConfusionMatrixDisplay
    {
        private static readonly List<(Color LightColor, Color DarkColor)> ColorPalette = new List<(Color, Color)>
        {
            (Color.FromArgb(52, 152, 219), Color.FromArgb(41, 128, 185)),    
            (Color.FromArgb(231, 76, 60), Color.FromArgb(192, 57, 43)),      
            (Color.FromArgb(46, 204, 113), Color.FromArgb(39, 174, 96)),     
            (Color.FromArgb(155, 89, 182), Color.FromArgb(142, 68, 173)),    
            (Color.FromArgb(241, 196, 15), Color.FromArgb(243, 156, 18))     
        };

        public static void ShowMetricsChart(
            MulticlassClassificationMetrics metrics, Chart chart, string modelName)
        {
            var counts = metrics.ConfusionMatrix.Counts;

            int tpHam = (int)counts[0][0];
            int tnHam = (int)counts[1][1];
            int fpHam = (int)counts[1][0];
            int fnHam = (int)counts[0][1];

            int tpSpam = (int)counts[1][1];
            int tnSpam = (int)counts[0][0];
            int fpSpam = (int)counts[0][1];
            int fnSpam = (int)counts[1][0];

            int maxValue = new[] { tpHam, tnHam, fpHam, fnHam, tpSpam, tnSpam, fpSpam, fnSpam }.Max();
            int axisYMaximum = (int)(Math.Ceiling(maxValue / 100.0) * 100);

            chart.Series.Clear();

            var chartArea = new ChartArea
            {
                BackColor = Color.White,
                AxisX = { Title = "Performance Metric", TitleFont = new Font("Tahoma", 10, FontStyle.Regular) },
                AxisY = { Title = "Count", TitleFont = new Font("Tahoma", 10, FontStyle.Regular) }
            };
            chart.ChartAreas.Clear();
            chart.ChartAreas.Add(chartArea);

            AddMetricSeries(chart, "True Positives (spam)", tpSpam, ColorPalette[0].DarkColor);
            AddMetricSeries(chart, "True Positives (ham)", tpHam, ColorPalette[0].LightColor);
            AddMetricSeries(chart, "True Negatives (spam)", tnSpam, ColorPalette[1].DarkColor);
            AddMetricSeries(chart, "True Negatives (ham)", tnHam, ColorPalette[1].LightColor);
            AddMetricSeries(chart, "False Positives (spam)", fpSpam, ColorPalette[2].DarkColor);
            AddMetricSeries(chart, "False Positives (ham)", fpHam, ColorPalette[2].LightColor);
            AddMetricSeries(chart, "False Negatives (spam)", fnSpam, ColorPalette[3].DarkColor);
            AddMetricSeries(chart, "False Negatives (ham)", fnHam, ColorPalette[3].LightColor);

            chartArea.AxisX.LabelStyle.Enabled = false;
            chartArea.AxisX.MajorGrid.Enabled = false;
            chartArea.AxisX.MinorGrid.Enabled = false;
            chartArea.AxisY.Maximum = axisYMaximum;
            chartArea.AxisY.Interval = axisYMaximum / 10;
            chartArea.AxisY.LabelStyle.Format = "0";
            chartArea.AxisY.LabelStyle.Font = new Font("Tahoma", 10);

            foreach (var series in chart.Series)
            {
                series["PointWidth"] = "1.0";
                series.IsValueShownAsLabel = false;
                series.Font = new Font("Tahoma", 10); 
            }

            chartArea.AxisY.MajorGrid.LineColor = Color.LightGray;

            chart.Legends.Clear();
            chart.Legends.Add(new Legend
            {
                Docking = Docking.Right,
                Alignment = StringAlignment.Near,
                Font = new Font("Tahoma", 10, FontStyle.Regular),
                BackColor = Color.Transparent
            });

            chart.BackColor = Color.White;
            chart.BorderlineColor = Color.LightGray;
            chart.BorderlineDashStyle = ChartDashStyle.Solid;
            chart.BorderlineWidth = 1;

            chart.Invalidate();
        }

        private static void AddMetricSeries(Chart chart, string name, int value, Color color)
        {
            var series = new Series
            {
                Name = name,
                ChartType = SeriesChartType.Column,
                Color = color,
                IsValueShownAsLabel = false
            };
            series.Points.Add(new DataPoint(0, value));
            chart.Series.Add(series);
        }
    }
}