using Microsoft.ML.Data;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms.DataVisualization.Charting;

namespace ML.Performance
{
    /// <summary>
    /// displays the counts from a binary confusion matrix as a bar chart
    /// </summary>
    public class ConfusionMatrixDisplay
    {
        // palette for alternating bar colors (true vs false cases)
        private static readonly (Color Light, Color Dark)[] Palette =
        {
            (Color.FromArgb(52,152,219), Color.FromArgb(41,128,185)),
            (Color.FromArgb(231,76,60),  Color.FromArgb(192,57,43))
        };

        public static void ShowMetricsChart(MulticlassClassificationMetrics m, Chart chart, string modelName)
        {
            // extract raw counts: matrix[row][col] → row = actual, col = predicted
            var c = m.ConfusionMatrix.Counts;

            // extract binary confusion matrix entries
            int tn = (int)c[0][0]; // true negatives: predicted 0, actual 0
            int fp = (int)c[0][1]; // false positives: predicted 1, actual 0
            int fn = (int)c[1][0]; // false negatives: predicted 0, actual 1
            int tp = (int)c[1][1]; // true positives: predicted 1, actual 1

            // find maximum value to normalize y-axis (rounded to nearest 100)
            int axisMax = new[] { tp, tn, fp, fn }.Max();
            axisMax = (int)(Math.Ceiling(axisMax / 100.0) * 100);

            // reset chart visuals
            chart.Series.Clear();
            chart.ChartAreas.Clear();

            var area = new ChartArea
            {
                BackColor = Color.White,
                AxisX = { Title = "Metric", TitleFont = new Font("Tahoma", 10) },
                AxisY = { Title = "Count", TitleFont = new Font("Tahoma", 10) }
            };
            chart.ChartAreas.Add(area);

            // add bars to chart
            AddBar(chart, "TP", tp, Palette[0].Dark);   // true positive
            AddBar(chart, "TN", tn, Palette[0].Light);  // true negative
            AddBar(chart, "FP", fp, Palette[1].Dark);   // false positive
            AddBar(chart, "FN", fn, Palette[1].Light);  // false negative

            // remove x-labels, scale y-axis
            area.AxisX.LabelStyle.Enabled = false;
            area.AxisY.Maximum = axisMax;
            area.AxisY.Interval = axisMax / 10.0;
            area.AxisY.MajorGrid.LineColor = Color.LightGray;

            chart.Legends.Clear();
            chart.Legends.Add(new Legend
            {
                Docking = Docking.Right,
                Font = new Font("Tahoma", 9)
            });

            chart.Invalidate();
        }

        // helper to add a single colored bar to the chart
        private static void AddBar(Chart chart, string name, int value, Color color)
        {
            var s = new Series
            {
                Name = name,
                ChartType = SeriesChartType.Column,
                Color = color,
                IsValueShownAsLabel = false
            };
            s.Points.Add(value);
            chart.Series.Add(s);
        }
    }
}
