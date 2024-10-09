using System.Windows.Forms.DataVisualization.Charting;

namespace ML.Performance
{
    public static class CumulativeGainsChart
    {
        private static readonly List<(Color LightColor, Color DarkColor)> ColorPalette = new List<(Color, Color)>
        {
            (Color.FromArgb(52, 152, 219), Color.FromArgb(41, 128, 185)),
            (Color.FromArgb(231, 76, 60), Color.FromArgb(192, 57, 43)),
            (Color.FromArgb(46, 204, 113), Color.FromArgb(39, 174, 96)),
            (Color.FromArgb(155, 89, 182), Color.FromArgb(142, 68, 173)),
            (Color.FromArgb(241, 196, 15), Color.FromArgb(243, 156, 18))
        };

        public static void ShowCumulativeGainsChart(float[] probabilities, string[] actuals, Chart chart, string modelName)
        {
            bool[] isSpam = actuals.Select(a => a.Equals("spam", StringComparison.OrdinalIgnoreCase)).ToArray();
            bool[] isHam = actuals.Select(a => a.Equals("ham", StringComparison.OrdinalIgnoreCase)).ToArray();

            var data = probabilities.Zip(actuals, (prob, label) => new { Probability = prob, Label = label })
                                    .OrderByDescending(x => x.Probability)
                                    .ToList();

            int totalSpam = isSpam.Count(a => a);
            int totalHam = isHam.Count(a => a);

            int cumulativeSpam = 0;
            int cumulativeHam = 0;

            List<double> tprSpamList = new List<double>();
            List<double> fprSpamList = new List<double>();
            List<double> tprHamList = new List<double>();
            List<double> fprHamList = new List<double>();

            foreach (var item in data)
            {
                if (item.Label.Equals("spam", StringComparison.OrdinalIgnoreCase))
                {
                    cumulativeSpam++;
                }
                else if (item.Label.Equals("ham", StringComparison.OrdinalIgnoreCase))
                {
                    cumulativeHam++;
                }

                double tprSpam = totalSpam > 0 ? (double)cumulativeSpam / totalSpam : 0;
                double fprSpam = totalHam > 0 ? (double)cumulativeHam / totalHam : 0;

                tprSpamList.Add(tprSpam);
                fprSpamList.Add(fprSpam);

                double tprHam = totalHam > 0 ? (double)cumulativeHam / totalHam : 0;
                double fprHam = totalSpam > 0 ? (double)cumulativeSpam / totalSpam : 0;

                tprHamList.Add(tprHam);
                fprHamList.Add(fprHam);
            }

            chart.Series.Clear();

            var chartArea = new ChartArea();
            chart.ChartAreas.Clear();
            chart.ChartAreas.Add(chartArea);

            var randomSeries = new Series
            {
                Name = "Random Selection",
                ChartType = SeriesChartType.Line,
                Color = Color.LightGray,
                BorderDashStyle = ChartDashStyle.Dash,
                BorderWidth = 2,
                IsVisibleInLegend = true,
                IsValueShownAsLabel = false
            };
            randomSeries.Points.AddXY(0, 0);
            randomSeries.Points.AddXY(1, 1);
            chart.Series.Add(randomSeries);

            var hamSeries = new Series
            {
                Name = "Cumulative Gains of the Model (Ham)",
                ChartType = SeriesChartType.Line,
                Color = ColorPalette[0].LightColor,
                BorderWidth = 2,
                IsVisibleInLegend = true,
                IsValueShownAsLabel = false
            };

            for (int i = 0; i < tprHamList.Count; i++)
            {
                hamSeries.Points.AddXY(fprHamList[i], tprHamList[i]);
            }
            chart.Series.Add(hamSeries);

            var spamSeries = new Series
            {
                Name = "Cumulative Gains of the Model (Spam)",
                ChartType = SeriesChartType.Line,
                Color = ColorPalette[1].DarkColor,
                BorderWidth = 2,
                IsVisibleInLegend = true,
                IsValueShownAsLabel = false
            };

            for (int i = 0; i < tprSpamList.Count; i++)
            {
                spamSeries.Points.AddXY(fprSpamList[i], tprSpamList[i]);
            }
            chart.Series.Add(spamSeries);

            chartArea.AxisX.Title = "False Positive Rate";
            chartArea.AxisX.TitleFont = new Font("Tahoma", 10);
            chartArea.AxisY.Title = "True Positive Rate";
            chartArea.AxisY.TitleFont = new Font("Tahoma", 10);

            chartArea.AxisX.LabelStyle.Format = "0.#";
            chartArea.AxisY.LabelStyle.Format = "0.#";
            chartArea.AxisX.LabelStyle.Font = new Font("Tahoma", 10);
            chartArea.AxisY.LabelStyle.Font = new Font("Tahoma", 10);

            chartArea.AxisX.Minimum = 0;
            chartArea.AxisX.Maximum = 1;
            chartArea.AxisY.Minimum = 0;
            chartArea.AxisY.Maximum = 1;
            chartArea.AxisX.Interval = 0.2;
            chartArea.AxisY.Interval = 0.2;

            chartArea.AxisX.MajorGrid.LineColor = Color.LightGray;
            chartArea.AxisY.MajorGrid.LineColor = Color.LightGray;

            chart.Legends.Clear();
            chart.Legends.Add(new Legend
            {
                Docking = Docking.Bottom,
                Alignment = StringAlignment.Center,
                Font = new Font("Tahoma", 8),
                BackColor = Color.Transparent
            });

            chart.Invalidate();
        }
    }
}