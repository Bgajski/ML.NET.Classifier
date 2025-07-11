using Microsoft.ML;
using Microsoft.ML.Data;
using System.Linq;

namespace ML.DataPreparation
{
    /// <summary>
    /// helper for applying instance weighting to imbalanced binary datasets
    /// adjusts weight so that both classes contribute equally to training
    /// </summary>
    public static class WeightingHelper
    {
        /// <summary>
        /// creates a weighting estimator based on label distribution in the dataset
        /// assigns higher weight to the minority class to balance class influence
        /// </summary>
        public static IEstimator<ITransformer> CreateWeightEstimator(MLContext ml, IDataView data)
        {
            // read all label values from the dataset
            var labelStats = ml.Data.CreateEnumerable<LabelRow>(data, reuseRowObject: false)
                .GroupBy(r => r.Label)
                .ToDictionary(g => g.Key, g => g.Count());

            // count number of true and false labels
            int countTrue = labelStats.ContainsKey(true) ? labelStats[true] : 0;
            int countFalse = labelStats.ContainsKey(false) ? labelStats[false] : 0;

            // calculate weights inversely proportional to class frequency -> (total weight contribution of both classes should be similar)
            float weightTrue = countTrue > 0 ? (float)(countTrue + countFalse) / (2 * countTrue) : 1;
            float weightFalse = countFalse > 0 ? (float)(countTrue + countFalse) / (2 * countFalse) : 1;

            // define a custom transformation that sets the Weight column based on label
            return ml.Transforms.CustomMapping<LabelRow, WeightRow>((input, output) =>
            {
                output.Weight = input.Label ? weightTrue : weightFalse;
            }, contractName: null); // contractName null = no need for reuse
        }

        // helper class to extract the Label column from the dataset
        private sealed class LabelRow
        {
            public bool Label { get; set; }
        }

        // defines a new Weight column to be added to the dataset
        private sealed class WeightRow
        {
            public float Weight { get; set; }
        }
    }
}
