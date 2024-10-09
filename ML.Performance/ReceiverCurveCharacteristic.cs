using System.Windows.Forms.DataVisualization.Charting;

namespace ML.Performance   
{
    public class ReceiverCurveCharacteristic
    {
        private static readonly List<Color> ColorPalette = new List<Color>
        {
            Color.FromArgb(52, 152, 219),    
            Color.FromArgb(231, 76, 60),     
            Color.FromArgb(46, 204, 113),    
            Color.FromArgb(149, 165, 166),   
        };

        public static void ShowRecieverCurve(
            IEnumerable<float> probabilities, IEnumerable<bool> actuals, Chart chart, string modelName)
        {
            chart.Series.Clear();

            var chartArea = new ChartArea
            {
                AxisX = { Title = "False Positive Rate", TitleFont = new Font("Tahoma", 10) },
                AxisY = { Title = "True Positive Rate", TitleFont = new Font("Tahoma", 10) }
            };
            chart.ChartAreas.Clear();
            chart.ChartAreas.Add(chartArea);

            var rocPoints = CalculateReceiverCurvePoints(probabilities, actuals);

            var rocSeries = new Series
            {
                Name = "ROC Curve",
                ChartType = SeriesChartType.Line,
                Color = ColorPalette[0], 
                BorderWidth = 2,
                IsVisibleInLegend = true,
                IsValueShownAsLabel = false
            };

            foreach (var point in rocPoints)
            {
                rocSeries.Points.AddXY(point.Fpr, point.Tpr);
            }

            var rocPointsSeries = new Series
            {
                Name = "ROC Points",
                ChartType = SeriesChartType.Point,
                Color = ColorPalette[1],  
                MarkerStyle = MarkerStyle.Circle,
                MarkerSize = 5,
                IsVisibleInLegend = true,
                IsValueShownAsLabel = false
            };

            int pointStep = Math.Max(1, rocPoints.Count / 25);
            for (int i = 0; i < rocPoints.Count; i += pointStep)
            {
                rocPointsSeries.Points.AddXY(rocPoints[i].Fpr, rocPoints[i].Tpr);
            }

            var randomGuessSeries = new Series
            {
                Name = "Random Selection",
                ChartType = SeriesChartType.Line,
                Color = ColorPalette[2], 
                BorderDashStyle = ChartDashStyle.Dash,
                BorderWidth = 2,
                IsVisibleInLegend = true,
                IsValueShownAsLabel = false
            };
            randomGuessSeries.Points.AddXY(0, 0);
            randomGuessSeries.Points.AddXY(1, 1);

            chart.Series.Add(rocSeries);
            chart.Series.Add(rocPointsSeries);
            chart.Series.Add(randomGuessSeries);

            chartArea.AxisX.LabelStyle.Format = "0.#";
            chartArea.AxisY.LabelStyle.Format = "0.#";
            chartArea.AxisX.LabelStyle.Font = new Font("Tahoma", 10);
            chartArea.AxisY.LabelStyle.Font = new Font("Tahoma", 10);

            chartArea.AxisX.Minimum = 0.0;
            chartArea.AxisX.Maximum = 1.0;
            chartArea.AxisY.Minimum = 0.0;
            chartArea.AxisY.Maximum = 1.0;
            chartArea.AxisX.Interval = 0.2;
            chartArea.AxisY.Interval = 0.2;

            chartArea.AxisX.MajorGrid.LineColor = Color.LightGray;
            chartArea.AxisY.MajorGrid.LineColor = Color.LightGray;

            chart.Legends.Clear();
            var legend = new Legend
            {
                Docking = Docking.Bottom,
                Alignment = StringAlignment.Center,
                Font = new Font("Tahoma", 10),
                BackColor = Color.Transparent
            };
            chart.Legends.Add(legend);

            chart.Invalidate();
        }

        private static List<(double Fpr, double Tpr)> CalculateReceiverCurvePoints(
            IEnumerable<float> probabilities, IEnumerable<bool> actuals)
        {
            var data = probabilities.Zip(actuals, (prob, actual) => new { Probability = prob, Actual = actual })
                                    .OrderByDescending(x => x.Probability)
                                    .ToList();

            int positiveCount = actuals.Count(a => a);
            int negativeCount = actuals.Count(a => !a);

            int tp = 0, fp = 0;
            var rocPoints = new List<(double Fpr, double Tpr)>();

            foreach (var item in data)
            {
                if (item.Actual)
                {
                    tp++;
                }
                else
                {
                    fp++;
                }

                double tpr = positiveCount > 0 ? (double)tp / positiveCount : 0;
                double fpr = negativeCount > 0 ? (double)fp / negativeCount : 0;

                rocPoints.Add((fpr, tpr));
            }

            return rocPoints;
        }
    }
}