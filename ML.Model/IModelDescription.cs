using Microsoft.ML.Data;

namespace ML.Model
{
    public interface IModelDescription
    {
        string GetDescription(string algorithmName);
        List<(string text, Color color)> GetMetricsDescriptionBinary(BinaryClassificationMetrics metrics);
        List<(string text, Color color)> GetMetricsDescriptionsTextual(MulticlassClassificationMetrics metrics);
    }
}