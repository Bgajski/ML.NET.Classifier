using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;
using Microsoft.ML.Trainers.FastTree;
using Microsoft.ML.Transforms.Text;
using ML.Model;
using System;
using System.Linq;

namespace ML.Model
{
    /// <summary>
    /// a model for multiclass text classification with support for:
    /// - fastforest wrapped in one-versus-all (default setup for binary or multiclass text classification)
    /// - naive bayes multiclass classifier (faster, probabilistic)
    /// 
    /// features:
    /// - custom decision threshold for binary-like decisions (e.g., spam vs ham)
    /// - easy prediction on a single input string
    /// - full support for model evaluation, transformation, and probability analysis
    /// </summary>
    public class TextualClassificationModel : ITextualClassificationModel
    {
        private readonly MLContext _ml;
        private ITransformer? _model;             // trained ml.net model pipeline
        private double _threshold = 0.5;          // decision threshold for binary-like use case

        public TextualClassificationModel(MLContext ml) => _ml = ml;

        /// <summary>
        /// trains a one-versus-all model using fastforest with word and char tf-idf features
        /// </summary>
        public ITransformer TrainFastForest(IDataView train)
        {
            // configure text featurization using tf-idf for word and char ngrams
            var txtOpts = new TextFeaturizingEstimator.Options
            {
                KeepPunctuations = false, // remove punctuation characters from text (e.g., '.', ',', '!')

                WordFeatureExtractor = new WordBagEstimator.Options
                {
                    NgramLength = 2,                // extract bigrams (e.g., "email spam", "free offer")
                    UseAllLengths = true,          // also include unigrams (e.g., "email", "spam")
                    Weighting = NgramExtractingEstimator.WeightingCriteria.TfIdf // apply tf-idf weighting to word ngrams
                },

                CharFeatureExtractor = new WordBagEstimator.Options
                {
                    NgramLength = 3,                // extract character trigrams (e.g., "fre", "ree", "e o", " of")
                    UseAllLengths = false,         // only extract ngrams of exactly length 3 (no unigrams or bigrams)
                    Weighting = NgramExtractingEstimator.WeightingCriteria.TfIdf // apply tf-idf weighting to character ngrams
                }
            };

            // generate "features" column from "text"
            var textFeats = _ml.Transforms.Text.FeaturizeText("Features", txtOpts, "Text");

            // configure binary fastforest to use in ova
            var ffBin = _ml.BinaryClassification.Trainers.FastForest(new()
            {
                Seed = 42,                             // random seed for reproducibility
                NumberOfLeaves = 40,                   // tree depth
                NumberOfTrees = 120,                   // ensemble size
                MinimumExampleCountPerLeaf = 3,        // minimum samples per leaf
                FeatureFraction = 0.8f                 // subset of features per split
            });

            // build pipeline: key -> features -> ova -> unkey
            var pipe = _ml.Transforms.Conversion.MapValueToKey("Label")
                       .Append(textFeats)
                       .AppendCacheCheckpoint(_ml)
                       .Append(_ml.MulticlassClassification.Trainers.OneVersusAll(ffBin, labelColumnName: "Label"))
                       .Append(_ml.Transforms.Conversion.MapKeyToValue("PredictedLabel"));

            _model = pipe.Fit(train);
            return _model;
        }

        /// <summary>
        /// trains a multiclass naive bayes model using tf-idf text features
        /// </summary>
        public ITransformer TrainNaiveBayes(IDataView train)
        {
            // default featurization with tf-idf on words
            var textFeats = _ml.Transforms.Text.FeaturizeText("Features", "Text");

            var pipe = _ml.Transforms.Conversion.MapValueToKey("Label")
                       .Append(textFeats)
                       .AppendCacheCheckpoint(_ml)
                       .Append(_ml.MulticlassClassification.Trainers.NaiveBayes(
                           labelColumnName: "Label",
                           featureColumnName: "Features"))
                       .Append(_ml.Transforms.Conversion.MapKeyToValue("PredictedLabel"));

            _model = pipe.Fit(train);
            return _model;
        }

        /// <summary>
        /// sets custom decision threshold in [0,1] for binary-like prediction
        /// </summary>
        public void SetThreshold(double threshold) => _threshold =
            (threshold is < 0 or > 1)
                ? throw new ArgumentOutOfRangeException(nameof(threshold), "threshold must be in [0,1]")
                : threshold;

        /// <summary>
        /// gets the current threshold
        /// </summary>
        public double GetThreshold() => _threshold;

        /// <summary>
        /// predicts if a single text sample is spam using thresholded score
        /// </summary>
        public bool PredictIsSpam(string text)
        {
            if (_model == null)
                throw new InvalidOperationException("model must be trained before making predictions");

            // create prediction engine for one-row inference
            var engine = _ml.Model.CreatePredictionEngine<InputRow, ScoreRow>(_model);
            float score = engine.Predict(new InputRow { Text = text }).Score;

            return score > _threshold; // apply threshold
        }

        /// <summary>
        /// evaluates trained model on a test dataset and returns multiclass metrics
        /// </summary>
        public MulticlassClassificationMetrics EvaluateModel(IDataView test)
        {
            if (_model == null)
                throw new InvalidOperationException("model must be trained before evaluation");

            return _ml.MulticlassClassification.Evaluate(_model.Transform(test));
        }

        /// <summary>
        /// transforms the data using the trained model pipeline
        /// </summary>
        public IDataView Transform(IDataView data) =>
            _model?.Transform(data) ?? throw new InvalidOperationException("model must be trained");

        /// <summary>
        /// returns predicted probabilities and ground-truth labels from a test set
        /// </summary>
        public (float[] probabilities, string[] actuals) GetPredictionsAndLabels(IDataView test)
        {
            var scored = Transform(test);

            return (
                scored.GetColumn<float[]>("Score").Select(v => v.Max()).ToArray(),
                _ml.Data.CreateEnumerable<LabelRow>(test, reuseRowObject: false)
                      .Select(r => r.Label).ToArray());
        }

        // helper class for extracting true labels from test data
        private sealed class LabelRow
        {
            public string Label { get; set; } // original string label (used for evaluation comparison)
        }

        // helper class used when making a single prediction from input string
        private sealed class InputRow
        {
            public string Text { get; set; } // text input to be classified
        }

        // helper class for extracting score from model output
        private sealed class ScoreRow
        {
            public float Score { get; set; } // raw model score (used to apply threshold)
        }
    }
}
