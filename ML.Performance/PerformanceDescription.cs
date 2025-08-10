using Microsoft.ML.Data;

namespace ML.Performance
{
    /// <summary>
    /// provides descriptions and metric calculations 
    /// for classification performance visualizations
    /// handles both binary and multiclass classification metrics
    /// </summary>
    public class PerformanceDescription : IPerformanceDescription
    {
        /// <summary>
        /// returns a short explanation for prediction distribution charts
        /// includes explanation of precision and recall for both classes
        /// </summary>
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
                   "identifies a larger \n" +
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

        /// <summary>
        /// explains what the ROC curve represents and the meaning of AUC
        /// </summary>
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

        /// <summary>
        /// describes the layout and meaning of the confusion matrix
        /// </summary>
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

        /// <summary>
        /// describes what is shown on a cumulative gains chart,
        /// including interpretation of TPR and FPR values
        /// </summary>
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

        /// <summary>
        /// returns ROC AUC score formatted for display
        /// </summary>
        public List<(string text, Color color)> GetMetricsReceiverCurve(BinaryClassificationMetrics metrics)
        {
            return new List<(string text, Color color)>
            {
                ("Area Under the ROC Curve (AUC): ", Color.Red),
                ($"{metrics.AreaUnderRocCurve:F2}", Color.Green)
            };
        }

        /// <summary>
        /// returns formatted precision and recall values for both classes
        /// </summary>
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

        /// <summary>
        /// extracts TP, FP, TN, FN counts from confusion matrix for both classes
        /// </summary>
        public List<(string text, Color color)> GetMetricsConfusionMatrix(MulticlassClassificationMetrics metrics)
        {
            var counts = metrics.ConfusionMatrix.Counts;

            // class 0 = ham (not spam)
            int tpHam = (int)counts[0][0]; // predicted 0, actual 0
            int fnHam = (int)counts[0][1]; // predicted 0, actual 1
            int fpHam = (int)counts[1][0]; // predicted 1, actual 0
            int tnHam = (int)counts[1][1]; // predicted 1, actual 1

            // class 1 = spam (mirrored view)
            int tpSpam = tnHam;
            int fnSpam = fpHam;
            int fpSpam = fnHam;
            int tnSpam = tpHam;

            return new List<(string text, Color color)>
            {
                // HAM class metrics
                ("Class 0 (ham) - True Positives (TP): ", Color.Red), ($"{tpHam} ", Color.Green),
                ("Class 0 (ham) - True Negatives (TN): ", Color.Red), ($"{tnHam} ", Color.Green),
                ("Class 0 (ham) - False Positives (FP): ", Color.Red), ($"{fpHam} ", Color.Green),
                ("Class 0 (ham) - False Negatives (FN): ", Color.Red), ($"{fnHam} ", Color.Green),

                // SPAM class metrics
                ("Class 1 (spam) - True Positives (TP): ", Color.Red), ($"{tpSpam} ", Color.Green),
                ("Class 1 (spam) - True Negatives (TN): ", Color.Red), ($"{tnSpam} ", Color.Green),
                ("Class 1 (spam) - False Positives (FP): ", Color.Red), ($"{fpSpam} ", Color.Green),
                ("Class 1 (spam) - False Negatives (FN): ", Color.Red), ($"{fnSpam} ", Color.Green),
            };
        }

        /// <summary>
        /// computes and formats TPR and FPR values for both classes,
        /// based on the confusion matrix
        /// </summary>
        public List<(string text, Color color)> GetMetricsCumulativeGainsChart(MulticlassClassificationMetrics metrics)
        {
            var counts = metrics.ConfusionMatrix.Counts;

            // extract confusion matrix values
            int tpHam = (int)counts[0][0];
            int fnHam = (int)counts[0][1];
            int fpHam = (int)counts[1][0];
            int tnHam = (int)counts[1][1];

            // mirrored values for spam
            int tpSpam = tnHam;
            int fnSpam = fpHam;
            int fpSpam = fnHam;
            int tnSpam = tpHam;

            // TPR = TP / (TP + FN) — proportion of actual positives correctly predicted
            double tprHam = (tpHam + fnHam) > 0 ? (double)tpHam / (tpHam + fnHam) : 0;
            double tprSpam = (tpSpam + fnSpam) > 0 ? (double)tpSpam / (tpSpam + fnSpam) : 0;

            // FPR = FP / (FP + TN) — proportion of actual negatives predicted as positives
            double fprHam = (fpHam + tnHam) > 0 ? (double)fpHam / (fpHam + tnHam) : 0;
            double fprSpam = (fpSpam + tnSpam) > 0 ? (double)fpSpam / (fpSpam + tnSpam) : 0;

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
