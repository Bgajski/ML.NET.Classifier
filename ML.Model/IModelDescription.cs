using Microsoft.ML.Data;

namespace ML.Model
{
    /// <summary>
    /// provides descriptions of models and evaluation metrics for display purposes.
    /// </summary>
    public interface IModelDescription
    {
        // returns short description of model based on algorithm name
        string GetDescription(string algorithmName);

        // formats binary classification metrics as colored text for UI
        List<(string text, Color color)> GetMetricsDescriptionBinary(BinaryClassificationMetrics metrics);

        // formats textual (multiclass) classification metrics as colored text
        List<(string text, Color color)> GetMetricsDescriptionsTextual(MulticlassClassificationMetrics metrics);
    }
}
