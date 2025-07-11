using System.Windows.Forms.DataVisualization.Charting;

namespace ML.Performance
{
    /// <summary>
    /// visualizes the Receiver Operating Characteristic (ROC) curve for a binary classifier
    /// </summary>
    public class ReceiverCurveCharacteristic
    {
        // predefined colors for styling the chart elements
        private static readonly List<Color> ColorPalette = new List<Color>
        {
            Color.FromArgb(52, 152, 219),    // main ROC curve (line)
            Color.FromArgb(231, 76, 60),     // individual points on ROC curve
            Color.FromArgb(46, 204, 113),    // random guess diagonal line
            Color.FromArgb(149, 165, 166),   // optional future usage
        };

        /// <summary>
        /// displays the ROC curve inside the given Chart control
        /// </summary>
        /// <param name="probabilities">predicted probabilities for the positive class</param>
        /// <param name="actuals">actual ground truth labels (true = positive, false = negative)</param>
        /// <param name="chart">Chart control from WinForms to draw the ROC curve</param>
        /// <param name="modelName">name of the model to display in the chart legend</param>
        public static void ShowRecieverCurve(
            IEnumerable<float> probabilities, IEnumerable<bool> actuals, Chart chart, string modelName)
        {
            chart.Series.Clear(); // clear previous content if any

            // create chart area and set axis titles
            var chartArea = new ChartArea
            {
                AxisX = { Title = "False Positive Rate", TitleFont = new Font("Tahoma", 10) },
                AxisY = { Title = "True Positive Rate", TitleFont = new Font("Tahoma", 10) }
            };
            chart.ChartAreas.Clear();
            chart.ChartAreas.Add(chartArea);

            // compute TPR/FPR points for various thresholds
            var rocPoints = CalculateReceiverCurvePoints(probabilities, actuals);

            // smooth ROC curve line
            var rocSeries = new Series
            {
                Name = "ROC Curve",
                ChartType = SeriesChartType.Line,
                Color = ColorPalette[0],
                BorderWidth = 2,
                IsVisibleInLegend = true
            };
            foreach (var point in rocPoints)
                rocSeries.Points.AddXY(point.Fpr, point.Tpr);

            // subset of ROC points (for clearer visual feedback)
            var rocPointsSeries = new Series
            {
                Name = "ROC Points",
                ChartType = SeriesChartType.Point,
                Color = ColorPalette[1],
                MarkerStyle = MarkerStyle.Circle,
                MarkerSize = 5,
                IsVisibleInLegend = true
            };
            int pointStep = Math.Max(1, rocPoints.Count / 25);
            for (int i = 0; i < rocPoints.Count; i += pointStep)
                rocPointsSeries.Points.AddXY(rocPoints[i].Fpr, rocPoints[i].Tpr);

            // diagonal random guess line (from (0,0) to (1,1))
            var randomGuessSeries = new Series
            {
                Name = "Random Selection",
                ChartType = SeriesChartType.Line,
                Color = ColorPalette[2],
                BorderDashStyle = ChartDashStyle.Dash,
                BorderWidth = 2,
                IsVisibleInLegend = true
            };
            randomGuessSeries.Points.AddXY(0, 0);
            randomGuessSeries.Points.AddXY(1, 1);

            // add all series to chart
            chart.Series.Add(rocSeries);
            chart.Series.Add(rocPointsSeries);
            chart.Series.Add(randomGuessSeries);

            // configure axis scaling and appearance
            chartArea.AxisX.Minimum = 0.0;
            chartArea.AxisX.Maximum = 1.0;
            chartArea.AxisY.Minimum = 0.0;
            chartArea.AxisY.Maximum = 1.0;
            chartArea.AxisX.Interval = 0.2;
            chartArea.AxisY.Interval = 0.2;

            chartArea.AxisX.LabelStyle.Format = "0.#";
            chartArea.AxisY.LabelStyle.Format = "0.#";
            chartArea.AxisX.LabelStyle.Font = new Font("Tahoma", 10);
            chartArea.AxisY.LabelStyle.Font = new Font("Tahoma", 10);

            chartArea.AxisX.MajorGrid.LineColor = Color.LightGray;
            chartArea.AxisY.MajorGrid.LineColor = Color.LightGray;

            // legend configuration
            chart.Legends.Clear();
            chart.Legends.Add(new Legend
            {
                Docking = Docking.Bottom,
                Alignment = StringAlignment.Center,
                Font = new Font("Tahoma", 10),
                BackColor = Color.Transparent
            });

            // force chart refresh
            chart.Invalidate();
        }

        /// <summary>
        /// calculates points for the ROC curve by simulating thresholds
        /// </summary>
        /// <param name="probabilities">predicted probabilities for the positive class</param>
        /// <param name="actuals">actual ground truth labels</param>
        /// <returns>list of (FPR, TPR) pairs representing the ROC curve</returns>
        private static List<(double Fpr, double Tpr)> CalculateReceiverCurvePoints(
            IEnumerable<float> probabilities, IEnumerable<bool> actuals)
        {
            // pair each probability with its corresponding true label
            // sort descending by probability to simulate threshold moving from high to low
            var data = probabilities.Zip(actuals, (prob, actual) => new { Probability = prob, Actual = actual })
                                    .OrderByDescending(x => x.Probability)
                                    .ToList();

            // count total positives and negatives for normalization
            int positiveCount = actuals.Count(a => a);     // ground truth positives
            int negativeCount = actuals.Count(a => !a);    // ground truth negatives

            int tp = 0; // true positives encountered so far
            int fp = 0; // false positives encountered so far
            var rocPoints = new List<(double Fpr, double Tpr)>(); // list to collect ROC points

            // simulate threshold descending: add one sample at a time and recompute TPR/FPR
            foreach (var item in data)
            {
                if (item.Actual)
                    tp++;   // true label is positive → true positive
                else
                    fp++;   // true label is negative → false positive

                // calculate true positive rate and false positive rate at this threshold
                double tpr = positiveCount > 0 ? (double)tp / positiveCount : 0;
                double fpr = negativeCount > 0 ? (double)fp / negativeCount : 0;

                // add current point to the curve
                rocPoints.Add((fpr, tpr));
            }

            return rocPoints;
        }
    }
}
