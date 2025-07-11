using Microsoft.ML.Data;

namespace ML.Model
{
    /// <summary>
    /// provides algorithm descriptions and metric explanations for both binary and textual classification models
    /// </summary>
    public class ModelDescription : IModelDescription
    {
        /// <summary>
        /// returns a textual description of the selected machine learning algorithm and its key evaluation metrics
        /// </summary>
        public string GetDescription(string algorithmName)
        {
            return algorithmName switch
            {
                "Logistic regression" =>
                    "Logistic regression is an algorithm for binary \n" +
                    "classification. \n\n" +
                    "It is used to predict the probability of an outcome \n" +
                    "based on input features, where the output is \n" +
                    "mapped to a range between 0 and 1 using the \n" +
                    "logistic function. \n\n" +
                    "Accuracy measures the overall correctness of the \n" +
                    "model, assessing how accurate the model's \n" +
                    "predictions are. \n\n" +
                    "Precision measures the accuracy of positive \n" +
                    "predictions, indicating how many of the predicted \n" +
                    "positive cases are truly positive. \n\n" +
                    "Recall measures how well the model recognizes \n" +
                    "positive examples, indicating how many of the \n" +
                    "truly positive cases the model successfully \n" +
                    "predicts as positive.",

                "Averaged Perceptron" =>
                    "Averaged Perceptron is an algorithm for binary \n" +
                    "classification. \n\n" +
                    "It is used for simple classification problems \n" +
                    "where the data is linearly separable, applying \n" +
                    "linear separation of data and the average values \n" +
                    "of weights. \n\n" +
                    "Accuracy measures the overall correctness of the \n" +
                    "model, assessing how accurate the model's \n" +
                    "predictions are. \n\n" +
                    "Precision measures the accuracy of positive \n" +
                    "predictions, indicating how many of the predicted \n" +
                    "positive cases are truly positive. \n\n" +
                    "Recall measures how well the model recognizes \n" +
                    "positive examples, indicating how many of the truly \n" +
                    "positive cases the model successfully predicts \n" +
                    "as positive.",

                "Decision tree" =>
                    "A decision tree is an algorithm for text classification. \n\n" +
                    "It uses a tree structure to make decisions based on \n" +
                    "feature values. \n\n" +
                    "Macro accuracy measures the average accuracy \n" +
                    "for each class, providing insight into the \n" +
                    "model's balance between different classes. \n\n" +
                    "Micro accuracy measures the overall accuracy \n" +
                    "of the model, taking into account all examples \n" +
                    "regardless of class. \n\n" +
                    "Log-loss measures the average log-loss of the \n" +
                    "classifier. Lower log-loss indicates a better \n" +
                    "model.",

                "Naive Bayes classifier" =>
                    "Naive Bayes is an algorithm for text classification. \n\n" +
                    "It is based on Bayes' theorem, assuming feature\n" +
                    "independence which simplifies the computation. \n\n" +
                    "Macro accuracy measures the average accuracy for \n" +
                    "each class, providing insight into the model's \n" +
                    "balance between different classes. \n\n" +
                    "Micro accuracy measures the overall accuracy of the \n" +
                    "model, taking into account all examples regardless \n" +
                    "of class. \n\n" +
                    "Log-loss measures the average log-loss of the \n" +
                    "classifier. Lower log-loss indicates a better model."
            };
        }

        /// <summary>
        /// formats binary classification metrics as a colored list for UI rendering
        /// </summary>
        public List<(string text, Color color)> GetMetricsDescriptionBinary(BinaryClassificationMetrics metrics)
        {
            return new List<(string text, Color color)>
            {
                ("Accuracy: ", Color.Red), ($"{metrics.Accuracy:F2} ", Color.Green),
                ("Precision: ", Color.Red), ($"{metrics.PositivePrecision:F2} ", Color.Green),
                ("Recall: ", Color.Red), ($"{metrics.PositiveRecall:F2} ", Color.Green),
            };
        }

        /// <summary>
        /// formats multiclass classification metrics as a colored list for UI rendering
        /// </summary>
        public List<(string text, Color color)> GetMetricsDescriptionsTextual(MulticlassClassificationMetrics metrics)
        {
            var metricDescriptions = new List<(string text, Color color)>
            {
                ("Macro Accuracy: ", Color.Red), ($"{metrics.MacroAccuracy:F2} ", Color.Green),
                ("Micro Accuracy: ", Color.Red), ($"{metrics.MicroAccuracy:F2} ", Color.Green),
                ("Log Loss: ", Color.Red), ($"{metrics.LogLoss:F2} ", Color.Green),
            };

            return metricDescriptions;
        }
    }
}
