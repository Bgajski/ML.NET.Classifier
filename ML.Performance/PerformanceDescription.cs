using Microsoft.ML.Data;

namespace ML.Performance
{
    public class PerformanceDescription : IPerformanceDescription
    {
        public string GetPredictionDistributionDescription()  
        {
            return "The Performance\n" +
                   "evaluation of predictions \n" +
                   "displays the probability \n" +
                   "distribution assigned \n" +
                   "to samples for a specific \n" +
                   "class. \n\n" +
                   "Precision of positive \n" +
                   "predictions (PPV) \n" +
                   "shows the proportion \n" +
                   "of true positive cases \n" +
                   "among all positive \n" +
                   "predictions. High\n" +
                   "precision means that  \n" +
                   "the model rarely \n" +
                   "gives false positive \n" +
                   "results. \n\n" +
                   "Recall of positive \n" +
                   "predictions (TPR) \n" +
                   "shows the proportion \n" +
                   "of true positive cases\n" +
                   "that are correctly\n" +
                   "identified. High recall \n" +
                   "means that the model \n" +
                   "identifies a larger " +
                   "number of true positive \n" +
                   "cases. \n\n" +
                   "Precision of negative \n" +
                   "predictions (NPV) \n" +
                   "shows the proportion \n" +
                   "of true negative cases \n" +
                   "among all negative \n" +
                   "predictions. High \n" +
                   "precision means that \n" +
                   "the model rarely gives \n" +
                   "false negative results. \n\n" +
                   "Recall of negative \n" +
                   "predictions (TNR) \n" +
                   "shows the proportion \n" +
                   "of true negative cases \n" +
                   "that are correctly \n" +
                   "identified. High \n" +
                   "specificity means that\n" +
                   "the model rarely gives \n" +
                   "false positive results.";
        }

        public string GetReceiverCurveDescription()
        {
            return "The Receiver Operating \n" +
                   "Characteristic (ROC) \n" +
                   "curve visually displays\n" +
                   "the relationship \n" +
                   "between the true \n" +
                   "positive rate and the \n" +
                   "false positive rate. \n\n" +
                   "The Area Under the \n" +
                   "Curve (AUC) represents \n" +
                   "a unique measure of \n" +
                   "model performance. \n\n" +
                   "A value between 0 and 1 \n" +
                   "shows the model's \n" +
                   "ability to distinguish \n" +
                   "between classes, with a \n" +
                   "higher value indicating \n" +
                   "better overall model \n" +
                   "effectiveness.";
        }

        public string GetConfusionMatrixDescription()
        {
            return "The confusion matrix \n" +
                   "displays the actual \n" +
                   "versus predicted \n" +
                   "values of the model. \n\n" +
                   "True Positives (TP) \n" +
                   "are correctly predicted \n" +
                   "positive cases. \n\n" +
                   "True Negatives (TN)\n" +
                   "are correctly predicted \n" +
                   "negative cases. \n\n" +
                   "False Positives (FP) \n" +
                   "are incorrectly \n" +
                   "predicted positive\n" +
                   "cases. \n\n" +
                   "False Negatives (FN) \n" +
                   "are incorrectly \n" +
                   "predicted negative\n" +
                   "cases.";
        }
        public string GetCumulativeGainsChartDescription()
        {
            return "The cumulative gains \n" +
                   "chart shows how well \n" +
                   "the model separates \n" +
                   "positive and negative  \n" +
                   "examples in relation to \n" +
                   "the overall population.\n\n" +
                   "The True Positive Rate \n" +
                   "(TPR) shows the " +
                   "proportion of positive \n" +
                   "cases that are correctly \n" +
                   "identified by the model.\n\n" +
                   "The False Positive Rate \n" +
                   "(FPR) shows the \n" +
                   "proportion of negative \n" +
                   "cases that are \n" +
                   "incorrectly classified as\n" +
                   "positive.";
        }

        public List<(string text, Color color)> GetMetricsReceiverCurve(BinaryClassificationMetrics metrics)
        {
            return new List<(string text, Color color)>
            {
                ("Area Under the ROC Curve (AUC): ", Color.Red),
                ($"{metrics.AreaUnderRocCurve:F2}", Color.Green)
            };
        }

        public List<(string text, Color color)> GetMetricsPredictionDistribution(BinaryClassificationMetrics metrics)
        {
            return new List<(string text, Color color)>
            {
                ("Precision of Positive Predictions (PPV): ", Color.Red),
                ($"{metrics.PositivePrecision:F2}", Color.Green),
                ("Recall of Positive Predictions (TPR): ", Color.Red),
                ($"{metrics.PositiveRecall:F2}", Color.Green),
                ("Precision of Negative Predictions (NPV): ", Color.Red),
                ($"{metrics.NegativePrecision:F2}", Color.Green),
                ("Recall of Negative Predictions (TNR): ", Color.Red),
                ($"{metrics.NegativeRecall:F2}", Color.Green)
            };
        }

        public List<(string text, Color color)> GetMetricsConfusionMatrix(MulticlassClassificationMetrics metrics)
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

            return new List<(string text, Color color)>
            {
                ("Class 0 (ham) - True Positives (TP): ", Color.Red), ($"{tpHam} ", Color.Green),
                ("Class 0 (ham) - True Negatives (TN): ", Color.Red), ($"{tnHam} ", Color.Green),
                ("Class 0 (ham) - False Positives (FP): ", Color.Red), ($"{fpHam} ", Color.Green),
                ("Class 0 (ham) - False Negatives (FN): ", Color.Red), ($"{fnHam} ", Color.Green),

                ("Class 1 (spam) - True Positives (TP): ", Color.Red), ($"{tpSpam} ", Color.Green),
                ("Class 1 (spam) - True Negatives (TN): ", Color.Red), ($"{tnSpam} ", Color.Green),
                ("Class 1 (spam) - False Positives (FP): ", Color.Red), ($"{fpSpam} ", Color.Green),
                ("Class 1 (spam) - False Negatives (FN): ", Color.Red), ($"{fnSpam} ", Color.Green),
            };
        }

        public List<(string text, Color color)> GetMetricsCumulativeGainsChart(MulticlassClassificationMetrics metrics)
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

            double tprHam = (double)tpHam / (tpHam + fnHam); 
            double fprHam = (double)fpHam / (fpHam + tnHam); 
            double tprSpam = (double)tpSpam / (tpSpam + fnSpam); 
            double fprSpam = (double)fpSpam / (fpSpam + tnSpam); 

            return new List<(string text, Color color)>
            {
                ("True Positive Rate (TPR ham): ", Color.Red), ($"{tprHam:F2}", Color.Green),
                ("False Positive Rate (FPR ham): ", Color.Red), ($"{fprHam:F2}", Color.Green),
                ("True Positive Rate (TPR spam): ", Color.Red), ($"{tprSpam:F2}", Color.Green),
                ("False Positive Rate (FPR spam): ", Color.Red), ($"{fprSpam:F2}", Color.Green)
            };
        }
    }
}