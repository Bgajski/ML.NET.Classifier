using System.Data;
using System.Windows.Forms.DataVisualization.Charting;

namespace ML.Graph
{
    public class TextualChartDisplay : IChartDisplay
    {
        private static readonly List<Color> ContrastColors = new List<Color>
        {
            Color.FromArgb(52, 152, 219),
            Color.FromArgb(231, 76, 60),
            Color.FromArgb(46, 204, 113),
            Color.FromArgb(155, 89, 182),
            Color.FromArgb(241, 196, 15),
            Color.FromArgb(52, 73, 94),
            Color.FromArgb(230, 126, 34),
            Color.FromArgb(26, 188, 156),
            Color.FromArgb(149, 165, 166),
            Color.FromArgb(243, 156, 18)
        };

        private static readonly List<Color> TopContrastColors = new List<Color>
        {
            Color.FromArgb(52, 152, 219),
            Color.FromArgb(231, 76, 60),
            Color.FromArgb(46, 204, 113),
            Color.FromArgb(155, 89, 182),
            Color.FromArgb(241, 196, 15)
        };

        public void DisplayDataOnChart(Chart chart, DataTable dataTable, SeriesChartType chartType, string classificationColumn)
        {
            chart.Series.Clear();
            chart.ChartAreas.Clear();

            var chartArea = new ChartArea
            {
                BackColor = Color.White,
                BorderWidth = 0,
                Position = new ElementPosition(5, 5, 90, 90),
                InnerPlotPosition = new ElementPosition(15, 15, 70, 70) 
            };
            chart.ChartAreas.Add(chartArea);

            var series = new Series
            {
                Name = classificationColumn,
                ChartType = chartType,
                XValueType = ChartValueType.String,
                YValueType = ChartValueType.Int32,
                Font = new Font("Tahoma", 10, FontStyle.Regular),
                IsValueShownAsLabel = false
            };

            var categoryCounts = dataTable.AsEnumerable()
                .GroupBy(row => row[classificationColumn]?.ToString() ?? "Unknown")
                .Select(g => new { Category = g.Key, Count = g.Count() })
                .ToList();

            int maxTopCategories = TopContrastColors.Count;
            var topCategories = categoryCounts.Take(maxTopCategories).ToList();
            var otherCategories = categoryCounts.Skip(maxTopCategories).ToList();

            var colorAssignments = new Dictionary<string, Color>();
            for (int i = 0; i < topCategories.Count; i++)
            {
                colorAssignments[topCategories[i].Category] = TopContrastColors[i % TopContrastColors.Count];
            }

            int otherColorIndex = 0;
            foreach (var category in otherCategories)
            {
                colorAssignments[category.Category] = ContrastColors[otherColorIndex % ContrastColors.Count];
                otherColorIndex++;
            }

            foreach (var categoryCount in categoryCounts)
            {
                var point = new DataPoint
                {
                    YValues = new[] { (double)categoryCount.Count },
                    Color = colorAssignments[categoryCount.Category]
                };
                series.Points.Add(point);
            }

            chart.Series.Add(series);

            chart.Legends.Clear();
            var legend = new Legend
            {
                Docking = Docking.Bottom,
                Alignment = StringAlignment.Center,
                Font = new Font("Tahoma", 10, FontStyle.Regular),
                BackColor = Color.Transparent
            };
            chart.Legends.Add(legend);

            foreach (var categoryCount in categoryCounts)
            {
                var legendItem = new LegendItem
                {
                    Name = $"{categoryCount.Category} ({categoryCount.Count})",
                    Color = colorAssignments[categoryCount.Category],
                    ImageStyle = LegendImageStyle.Rectangle
                };
                legend.CustomItems.Add(legendItem);
            }

            chartArea.AxisX.LabelStyle.Font = new Font("Tahoma", 10, FontStyle.Regular);
            chartArea.AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;
            chartArea.AxisX.LabelStyle.Angle = -45;
            chartArea.AxisY.LabelStyle.Font = new Font("Tahoma", 10, FontStyle.Regular);
            chartArea.AxisY.Interval = 1;

            chart.CustomizeLegend += (sender, e) =>
            {
                foreach (var legendItem in e.LegendItems.ToList())
                {
                    if (legendItem.Name.Contains("Point"))
                    {
                        e.LegendItems.Remove(legendItem);
                    }
                }
            };

            chartArea.RecalculateAxesScale();
            chart.Invalidate();
        }
    }
}