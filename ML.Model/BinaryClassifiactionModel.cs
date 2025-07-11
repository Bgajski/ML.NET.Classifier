using Microsoft.ML;
using Microsoft.ML.Data;
using ML.DataPreparation;
using ML.Performance;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ML.Model
{
    /// <summary>
    /// a model for binary classification that supports two algorithms:
    /// - logistic regression (with l1/l2 regularization and support for class weighting)
    /// - averaged perceptron (faster, but doesn't output probabilities)
    /// 
    /// this class handles training, evaluation, threshold tuning (based on f1-score),
    /// and extracting predictions (score or probability) for charting or analysis
    /// </summary>
    public class BinaryClassificationModel : IBinaryClassificationModel
    {
        private readonly MLContext _ml;
        private ITransformer _model;               // trained ml.net model
        private IDataView _trainCache;             // cached training set for threshold tuning or evaluation
        private double _threshold = 0.5;           // default classification threshold
        private bool _hasProbability = false;      // whether the model outputs calibrated probabilities

        private const string LabelCol = "Label";           // label column name
        private const string FeatCol = "Features";         // feature vector column name
        private const string PredCol = "PredictedLabel";   // output predicted label
        private const string ScoreCol = "Score";           // raw model output (e.g., log-odds)
        private const string ProbCol = "Probability";      // predicted probability (if supported)

        /// <summary>
        /// creates an instance of the binary classifier using a shared mlcontext
        /// </summary>
        public BinaryClassificationModel(MLContext mlContext)
        {
            _ml = mlContext;
        }

        /// <summary>
        /// trains the logistic regression model with l1/l2 regularization and weighting support
        /// </summary>
        public ITransformer TrainLogisticRegression(IDataView train)
        {
            _trainCache = train;

            // automatically compute instance weights for class imbalance
            var weightedTrain = WeightingHelper.CreateWeightEstimator(_ml, train)
                                               .Fit(train)
                                               .Transform(train);

            var opts = new Microsoft.ML.Trainers.LbfgsLogisticRegressionBinaryTrainer.Options
            {
                LabelColumnName = LabelCol,             // binary label column
                FeatureColumnName = FeatCol,            // input feature vector
                ExampleWeightColumnName = "Weight",     // optional weight column to handle imbalance
                L1Regularization = 0.01f,               // promotes sparse models (set some weights to 0)
                L2Regularization = 0.05f,               // avoids large weights, improves generalization
                MaximumNumberOfIterations = 150         // max optimization steps
            };

            var pipeline = _ml.Transforms.NormalizeMeanVariance(FeatCol)
                .Append(_ml.BinaryClassification.Trainers.LbfgsLogisticRegression(opts));

            _model = pipeline.Fit(weightedTrain);
            _hasProbability = true; // logistic regression supports probability output

            return _model;
        }

        /// <summary>
        /// trains the averaged perceptron model
        /// </summary>
        public ITransformer TrainAveragedPerceptron(IDataView train)
        {
            _trainCache = train;

            var opts = new Microsoft.ML.Trainers.AveragedPerceptronTrainer.Options
            {
                LabelColumnName = LabelCol,     // label column
                FeatureColumnName = FeatCol,    // feature column
                NumberOfIterations = 20         // how many passes over the data
            };

            var pipeline = _ml.Transforms.NormalizeMeanVariance(FeatCol)
                .Append(_ml.BinaryClassification.Trainers.AveragedPerceptron(opts));

            _model = pipeline.Fit(train);
            _hasProbability = false; // perceptron does not produce probability

            return _model;
        }

        /// <summary>
        /// evaluates the model using test data and returns binary classification metrics
        /// </summary>
        public BinaryClassificationMetrics EvaluateModel(IDataView test)
        {
            if (_model == null) throw new InvalidOperationException("model must be trained before evaluation");

            // normalize test data in same way as training
            var testNorm = _ml.Transforms.NormalizeMeanVariance(FeatCol).Fit(test).Transform(test);
            var predictions = _model.Transform(testNorm);

            IDataView thresholded;

            if (_hasProbability)
            {
                // apply threshold to probability column to generate predicted labels
                var thresholdPipe = _ml.Transforms.CustomMapping<PredWithProb, PredOut>(
                    (src, dst) => { dst.PredictedLabel = src.Probability >= (float)_threshold; }, null);

                thresholded = thresholdPipe.Fit(predictions).Transform(predictions);

                // evaluate using calibrated metrics (requires probability column)
                return _ml.BinaryClassification.Evaluate(
                    data: thresholded,
                    labelColumnName: LabelCol,        // ground truth
                    scoreColumnName: ScoreCol,        // raw score
                    probabilityColumnName: ProbCol,   // predicted prob
                    predictedLabelColumnName: PredCol // output decision
                );
            }
            else
            {
                // apply threshold to score column for uncalibrated models
                var thresholdPipe = _ml.Transforms.CustomMapping<PredWithScore, PredOut>(
                    (src, dst) => { dst.PredictedLabel = src.Score >= 0f; }, null);

                thresholded = thresholdPipe.Fit(predictions).Transform(predictions);

                // evaluate using uncalibrated metrics (no probability column)
                return _ml.BinaryClassification.EvaluateNonCalibrated(
                    data: thresholded,
                    labelColumnName: LabelCol,
                    scoreColumnName: ScoreCol,
                    predictedLabelColumnName: PredCol
                );
            }
        }

        /// <summary>
        /// applies the trained model to new data and returns full output
        /// </summary>
        public IDataView Transform(IDataView data)
        {
            var norm = _ml.Transforms.NormalizeMeanVariance(FeatCol).Fit(data).Transform(data);
            return _model.Transform(norm);
        }

        /// <summary>
        /// extracts score or probability + true labels from a dataset for visualization or analysis
        /// </summary>
        public (List<float> probs, List<bool> labels) GetPredictionsAndLabels(IDataView testData)
        {
            var testNorm = _ml.Transforms.NormalizeMeanVariance(FeatCol).Fit(testData).Transform(testData);
            var predictions = _model.Transform(testNorm);

            if (_hasProbability)
            {
                var rows = _ml.Data.CreateEnumerable<PredWithProb>(predictions, reuseRowObject: false).ToList();
                return (rows.Select(r => r.Probability).ToList(), rows.Select(r => r.Label).ToList());
            }
            else
            {
                var rows = _ml.Data.CreateEnumerable<PredWithScore>(predictions, reuseRowObject: false).ToList();
                return (rows.Select(r => r.Score).ToList(), rows.Select(r => r.Label).ToList());
            }
        }

        /// <summary>
        /// finds the threshold that maximizes f1 score on validation data
        /// </summary>
        public double OptimizeThresholdByF1(IDataView validationData)
        {
            _threshold = BinaryThresholdOptimizer.OptimizeThresholdByF1(
                model: _model,
                _ml,
                validationData,
                FeatCol,
                _hasProbability,
                out _ // ignore f1 value
            );
            return _threshold;
        }

        // helper class for models that return both score and probability
        private sealed class PredWithProb
        {
            public float Probability { get; set; }   // predicted probability
            public float Score { get; set; }         // raw model score
            public bool Label { get; set; }          // ground truth label
        }

        // helper class for models that return only score (like perceptron)
        private sealed class PredWithScore
        {
            public float Score { get; set; }         // raw score only
            public bool Label { get; set; }          // ground truth label
        }

        // used to output final predicted label after applying threshold
        private sealed class PredOut
        {
            public bool PredictedLabel { get; set; } // final decision based on threshold
        }
    }
}
