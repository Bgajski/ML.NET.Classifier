using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms.DataVisualization.Charting;

namespace ML.Performance
{
    /// <summary>
    /// draws a cumulative gains chart (like ROC) for binary classification: shows performance gain over random guessing
    /// </summary>
    public static class CumulativeGainsChart
    {
        // predefined color palette for ham/spam curves
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
            // convert labels to booleans for spam/ham
            bool[] isSpam = actuals.Select(a => a.Equals("spam", StringComparison.OrdinalIgnoreCase)).ToArray();
            bool[] isHam = actuals.Select(a => a.Equals("ham", StringComparison.OrdinalIgnoreCase)).ToArray();

            // zip labels + probabilities and sort by descending probability
            var data = probabilities.Zip(actuals, (prob, label) => new { Probability = prob, Label = label })
                                    .OrderByDescending(x => x.Probability)
                                    .ToList();

            int totalSpam = isSpam.Count(x => x);
            int totalHam = isHam.Count(x => x);

            int cumulativeSpam = 0;
            int cumulativeHam = 0;

            // store tpr and fpr over ranked population
            List<double> tprSpamList = new();
            List<double> fprSpamList = new();
            List<double> tprHamList = new();
            List<double> fprHamList = new();

            // walk through ranked instances and compute cumulative TPR and FPR
            foreach (var item in data)
            {
                if (item.Label.Equals("spam", StringComparison.OrdinalIgnoreCase))
                    cumulativeSpam++;
                else if (item.Label.Equals("ham", StringComparison.OrdinalIgnoreCase))
                    cumulativeHam++;

                // spam: tpr = hits / total spam, fpr = ham mistakes / total ham
                double tprSpam = totalSpam > 0 ? (double)cumulativeSpam / totalSpam : 0;
                double fprSpam = totalHam > 0 ? (double)cumulativeHam / totalHam : 0;

                tprSpamList.Add(tprSpam);
                fprSpamList.Add(fprSpam);

                // ham: tpr = ham hits / total ham, fpr = spam mistakes / total spam
                double tprHam = totalHam > 0 ? (double)cumulativeHam / totalHam : 0;
                double fprHam = totalSpam > 0 ? (double)cumulativeSpam / totalSpam : 0;

                tprHamList.Add(tprHam);
                fprHamList.Add(fprHam);
            }

            // clear previous chart
            chart.Series.Clear();
            chart.ChartAreas.Clear();
            var chartArea = new ChartArea();
            chart.ChartAreas.Add(chartArea);

            // draw diagonal baseline (random model)
            var randomSeries = new Series
            {
                Name = "Random Selection",
                ChartType = SeriesChartType.Line,
                Color = Color.LightGray,
                BorderDashStyle = ChartDashStyle.Dash,
                BorderWidth = 2,
                IsVisibleInLegend = true
            };
            randomSeries.Points.AddXY(0, 0);
            randomSeries.Points.AddXY(1, 1);
            chart.Series.Add(randomSeries);

            // draw gains curve for ham
            var hamSeries = new Series
            {
                Name = "Cumulative Gains of the Model (Ham)",
                ChartType = SeriesChartType.Line,
                Color = ColorPalette[0].LightColor,
                BorderWidth = 2,
                IsVisibleInLegend = true
            };
            for (int i = 0; i < tprHamList.Count; i++)
                hamSeries.Points.AddXY(fprHamList[i], tprHamList[i]);
            chart.Series.Add(hamSeries);

            // draw gains curve for spam
            var spamSeries = new Series
            {
                Name = "Cumulative Gains of the Model (Spam)",
                ChartType = SeriesChartType.Line,
                Color = ColorPalette[1].DarkColor,
                BorderWidth = 2,
                IsVisibleInLegend = true
            };
            for (int i = 0; i < tprSpamList.Count; i++)
                spamSeries.Points.AddXY(fprSpamList[i], tprSpamList[i]);
            chart.Series.Add(spamSeries);

            // axis config and visuals
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
