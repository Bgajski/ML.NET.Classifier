using Microsoft.ML;
using Microsoft.ML.Data;
using System.Collections.Generic;
using System.Linq;

namespace ML.Performance
{
    /// <summary>
    /// static helper class for threshold optimization in binary classification.
    /// selects optimal decision threshold (0–1) based on best F1-score from validation data.
    /// </summary>
    public static class BinaryThresholdOptimizer
    {
        /// <summary>
        /// searches for threshold that maximizes F1-score on the validation set.
        /// </summary>
        /// <param name="model">trained binary classifier</param>
        /// <param name="mlContext">ml.net context instance</param>
        /// <param name="validationData">data for threshold tuning</param>
        /// <param name="featureColumn">name of the main input feature column</param>
        /// <param name="hasProbability">true if model outputs probability, false if only raw score</param>
        /// <param name="bestF1">returns best F1-score found</param>
        /// <returns>threshold in range [0.01, 0.99] that gives best F1</returns>
        public static double OptimizeThresholdByF1(
            ITransformer model,
            MLContext mlContext,
            IDataView validationData,
            string featureColumn,
            bool hasProbability,
            out double bestF1)
        {
            // normalize input feature to reduce scale-related influence on model behavior
            var valNorm = mlContext.Transforms
                .NormalizeMeanVariance(featureColumn)
                .Fit(validationData)
                .Transform(validationData);

            // run model inference on normalized validation data
            var predictions = model.Transform(valNorm);

            // extract (score, label) pairs depending on model output type
            var rows = hasProbability
                ? mlContext.Data
                    .CreateEnumerable<PredWithProb>(predictions, reuseRowObject: false)
                    .Select(r => (r.Probability, r.Label)).ToList()
                : mlContext.Data
                    .CreateEnumerable<PredWithScore>(predictions, reuseRowObject: false)
                    .Select(r => (r.Score, r.Label)).ToList();

            bestF1 = 0;
            double bestThreshold = 0.5;

            // test thresholds in [0.01, 0.99] in steps of 0.01
            for (double t = 0.01; t < 1.0; t += 0.01)
            {
                int tp = 0, fp = 0, fn = 0;

                // count tp, fp, fn based on predictions at current threshold
                foreach (var row in rows)
                {
                    bool predicted = row.Item1 >= t;

                    // true positive: predicted true, actually true
                    if (predicted && row.Label) tp++;

                    // false positive: predicted true, actually false
                    else if (predicted) fp++;

                    // false negative: predicted false, actually true
                    else if (row.Label) fn++;
                }

                // precision = tp / (tp + fp): how many predicted positives are correct
                double precision = tp + fp > 0 ? (double)tp / (tp + fp) : 0;

                // recall = tp / (tp + fn): how many actual positives are correctly predicted
                double recall = tp + fn > 0 ? (double)tp / (tp + fn) : 0;

                // f1 = harmonic mean of precision and recall
                // handles imbalance between fp and fn gracefully
                double f1 = (precision + recall > 0)
                    ? 2 * precision * recall / (precision + recall)
                    : 0;

                // store the threshold if current f1 is best so far
                if (f1 > bestF1)
                {
                    bestF1 = f1;
                    bestThreshold = t;
                }
            }

            return bestThreshold;
        }

        // class used when prediction has calibrated probability
        private sealed class PredWithProb
        {
            public float Probability { get; set; } // model output (0–1)
            public bool Label { get; set; } // ground truth label
        }

        // class used when prediction is a raw score (uncalibrated)
        private sealed class PredWithScore
        {
            public float Score { get; set; } // model output (e.g., logistic regression raw score)
            public bool Label { get; set; }
        }
    }
}
