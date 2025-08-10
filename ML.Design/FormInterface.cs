using System.Data;
using Microsoft.ML;
using ML.Data;
using ML.DataPreparation;
using ML.Model;
using ML.Performance;
using System.Windows.Forms.DataVisualization.Charting;
using ML.Graph;

namespace ML.Design
{
    public partial class FormInterface : Form
    {
        #region Fields

        private BinaryClassificationModel _binaryModel;
        private TextualClassificationModel _textualModel;
        private IDataView _trainingData;
        private IDataView _testData;
        private readonly ICheckCharacteristic _checkCharacteristic;
        private readonly IDataLoad _dataLoad;
        private readonly IPerformanceDescription _performanceDescription;
        private readonly IChartTypeProvider _chartTypeProvider;
        private readonly IChartDisplay _binaryChartDisplay;
        private readonly IChartDisplay _textualChartDisplay;
        private readonly IModelDescription _modelDescription;
        private readonly IPerformanceVisualizer _performanceVisualizer;
        private SeriesChartType defaultChartType;
        private readonly MLContext _mlContext;

        private string _lastSelectedGraphP2;
        private bool _placeholderRemoved;

        #endregion

        #region Constructor

        public FormInterface()
        {
            InitializeComponent();
            _mlContext = new MLContext();

            _checkCharacteristic = new CheckCharacteristic();
            _dataLoad = new DataLoad();
            _performanceDescription = new PerformanceDescription();
            _chartTypeProvider = new ChartTypeProvider();
            _binaryChartDisplay = new BinaryChartDisplay();
            _textualChartDisplay = new TextualChartDisplay();
            _modelDescription = new ModelDescription();
            _performanceVisualizer = new PerformanceVisualizer();

            InitializeEventHandlers();

            comboBox_graph_p1.Items.Clear();
            comboBox_graph_p1.Text = "";
            comboBox_graph_p2.Items.Clear();
            comboBox_graph_p2.Text = "";

            _lastSelectedGraphP2 = "";
            _placeholderRemoved = false;
        }

        #endregion

        #region Initialization

        private void InitializeEventHandlers()
        {
            comboBox_graph_p1.SelectedIndexChanged += comboBox_graph_p1_SelectedIndexChanged;
            algorithmSelection_p2.SelectedIndexChanged += algorithmSelection_p2_SelectedIndexChanged;
            train_Button_p2.Click += train_Button_p2_Click;
            button_csv_reset.Click += button_csv_reset_Click;
            comboBox_graph_p2.SelectedIndexChanged += comboBox_graph_p2_SelectedIndexChanged;
            button_exit.Click += button_exit_Click;
        }

        #endregion

        #region Event Handlers

        private void button_csv_p1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*",
                RestoreDirectory = true
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    HandleFileLoading(openFileDialog.FileName);
                    txtBoxPerformanceMetric_p2.Clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading data: " + ex.Message);
                }
            }
        }

        private void button_csv_reset_Click(object sender, EventArgs e)
        {
            ResetForm();
            txtBoxPerformanceMetric_p2.Clear();
        }

        private void train_Button_p2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox_csv_p1.Text))
            {
                Console.WriteLine("A file must be selected.");
                return;
            }

            HandleModelTraining();
            PopulateGraphOptions();
        }

        private void comboBox_graph_p2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_graph_p2.SelectedItem != null)
            {
                string selectedGraph = comboBox_graph_p2.SelectedItem.ToString();
                if (selectedGraph == _lastSelectedGraphP2)
                {
                    return;
                }

                _lastSelectedGraphP2 = selectedGraph;

                if (selectedGraph != "Select a method to evaluate model performance")
                {
                    if (!_placeholderRemoved)
                    {
                        comboBox_graph_p2.Items.Remove("Select a method to evaluate model performance");
                        _placeholderRemoved = true;
                    }
                    UpdateGraphDisplay();
                }
            }
        }

        private void button_exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void comboBox_graph_p1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_graph_p1.SelectedItem != null && comboBox_graph_p1.SelectedItem.ToString() != "")
            {
                string selectedChartName = comboBox_graph_p1.SelectedItem.ToString();
                SeriesChartType chartType = _chartTypeProvider.GetChartTypeFromName(selectedChartName);

                defaultChartType = chartType;
                DisplayDataOnChart(textBox_validation_p1.Text, (DataTable)dataGrid_csv_p1.DataSource);
            }
        }

        private void algorithmSelection_p2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (algorithmSelection_p2.SelectedItem != null && algorithmSelection_p2.SelectedItem.ToString() != "")
            {
                string selectedAlgorithm = algorithmSelection_p2.SelectedItem.ToString();
                train_TextBox_p2.Text = _modelDescription.GetDescription(selectedAlgorithm);

                comboBox_graph_p2.Items.Clear();
                comboBox_graph_p2.Text = "";
                txtBoxPerformanse_p2.Clear();
                txtBoxPerformanceMetric_p2.Clear();
                txtBoxMetric_p2.Clear();
                dataChar_csv_p2.Series.Clear();
                dataChar_csv_p2.ChartAreas.Clear();
                dataChar_csv_p2.Titles.Clear();
                dataChar_csv_p2.Annotations.Clear();
                dataChar_csv_p2.Invalidate();
            }
        }

        #endregion

        #region Methods

        private void HandleFileLoading(string filePath)
        {
            var (dataTable, featureCount) = _dataLoad.LoadData(filePath);

            textBox_csv_p1.Text = Path.GetFileName(filePath);
            textBox_validation_p1.Text = "";
            txtBoxMetric_p2.Text = "";
            txtBoxPerformanceMetric_p2.Text = "";
            train_TextBox_p2.Text = "";
            txtBoxPerformanse_p2.Text = "";

            comboBox_graph_p1.Items.Clear();
            comboBox_graph_p1.Text = "";
            comboBox_graph_p2.Items.Clear();
            comboBox_graph_p2.Text = "";
            dataChar_csv_p2.Series.Clear();
            dataChar_csv_p2.ChartAreas.Clear();
            dataChar_csv_p2.Titles.Clear();
            dataChar_csv_p2.Invalidate();

            algorithmSelection_p2.SelectedIndex = -1;
            algorithmSelection_p2.Text = "";
            _binaryModel = null;
            _textualModel = null;

            string classificationResult = _checkCharacteristic.CheckClassificationType(dataTable);
            textBox_validation_p1.Text = classificationResult;

            if (classificationResult.Contains("binary classification"))
            {
                LoadBinaryData(dataTable);
                _binaryModel = new BinaryClassificationModel(_mlContext);
                SetInitialTrainingOptions("binary classification");
                SplitDataForTraining(filePath, new BinaryDataPreparation(), featureCount);
            }
            else if (classificationResult.Contains("textual classification"))
            {
                LoadTextualData(dataTable);
                _textualModel = new TextualClassificationModel(_mlContext);
                SetInitialTrainingOptions("textual classification");
                SplitDataForTraining(filePath, new TextualDataPreparation(), featureCount);
            }

            SetInitialChartOptions(classificationResult);
            DisplayDataOnChart(classificationResult, dataTable);
            UpdateChartOptions(classificationResult);
            ResetAdditionalChartAndTextbox();
        }

        private void HandleModelTraining()
        {
            string selectedAlgorithm = algorithmSelection_p2.Text;
            string algorithmName = "";

            if (_binaryModel != null)
            {
                TrainBinaryModel(selectedAlgorithm, ref algorithmName);
                var binaryMetrics = _binaryModel.EvaluateModel(_testData);
                var descriptions = _modelDescription.GetMetricsDescriptionBinary(binaryMetrics);

                DisplayMetricsInColor(txtBoxMetric_p2, descriptions);
            }
            else if (_textualModel != null)
            {
                TrainTextualModel(selectedAlgorithm, ref algorithmName);
                var textualMetrics = _textualModel.EvaluateModel(_testData);
                var descriptions = _modelDescription.GetMetricsDescriptionsTextual(textualMetrics);

                DisplayMetricsInColor(txtBoxMetric_p2, descriptions);
            }

            PopulateGraphOptions();
        }

        /// <summary>
        /// trains a binary classification model using the selected algorithm and optimizes threshold on validation set
        /// </summary>
        private void TrainBinaryModel(string selectedAlgorithm, ref string algorithmName)
        {
            // perform a double split: 70% train, 15% validation, 15% test
            var split1 = _mlContext.Data.TrainTestSplit(_trainingData, testFraction: 0.30, seed: 42); // 70% train
            var split2 = _mlContext.Data.TrainTestSplit(split1.TestSet, testFraction: 0.50, seed: 42); // 15% val / 15% test
            var train = split1.TrainSet;
            var validation = split2.TrainSet;
            var test = split2.TestSet;

            // train selected algorithm
            switch (selectedAlgorithm)
            {
                case "Logistic regression":
                    _binaryModel.TrainLogisticRegression(train);
                    algorithmName = "LbfgsLogisticRegression"; 
                    break;

                case "Averaged Perceptron":
                    _binaryModel.TrainAveragedPerceptron(train);
                    algorithmName = "AveragedPerceptron";
                    break;
            }

            // tune the decision threshold using validation set (based on F1-score)
            double optimizedThreshold = _binaryModel.OptimizeThresholdByF1(validation);

            // store final test set for downstream evaluation/graphing
            _testData = test;
        }

        /// <summary>
        /// trains a multiclass (or binary) text classification model and prepares test set
        /// </summary>
        private void TrainTextualModel(string selectedAlgorithm, ref string algorithmName)
        {
            // perform a double split: 70% train, 15% validation, 15% test
            var split1 = _mlContext.Data.TrainTestSplit(_trainingData, testFraction: 0.30, seed: 42); // 70% train
            var split2 = _mlContext.Data.TrainTestSplit(split1.TestSet, testFraction: 0.50, seed: 42); // 15% val / 15% test
            var train = split1.TrainSet;
            var validation = split2.TrainSet;
            var test = split2.TestSet;

            // train selected algorithm
            switch (selectedAlgorithm)
            {
                case "Decision tree":
                    _textualModel.TrainFastForest(train);
                    algorithmName = "FastForest";  
                    break;

                case "Naive Bayes classifier":
                    _textualModel.TrainNaiveBayes(train);
                    algorithmName = "NaiveBayes"; 
                    break;

                default:
                    throw new InvalidOperationException($"Unrecognized algorithm for textual classification: {selectedAlgorithm}");
            }

            // store final test set for downstream evaluation/graphing
            _testData = test;
        }

        private void PopulateGraphOptions()
        {
            comboBox_graph_p2.Items.Clear();
            comboBox_graph_p2.Items.Add("Select a method to evaluate model performance");
            comboBox_graph_p2.SelectedIndex = 0;

            if (_binaryModel != null)
            {
                comboBox_graph_p2.Items.Add("Performance evaluation of predictions");
                comboBox_graph_p2.Items.Add("Receiver operating characteristic curve");
            }
            else if (_textualModel != null)
            {
                comboBox_graph_p2.Items.Add("Confusion matrix evaluation");
                comboBox_graph_p2.Items.Add("Cumulative gains chart");
            }

            _placeholderRemoved = false;
        }

        private void UpdateGraphDisplay()
        {
            if (comboBox_graph_p2.SelectedItem != null && comboBox_graph_p2.SelectedItem.ToString() != "Select a method to evaluate model performance")
            {
                string selectedGraph = comboBox_graph_p2.SelectedItem.ToString();

                if (_binaryModel != null)
                {
                    var (probabilities, actuals) = _binaryModel.GetPredictionsAndLabels(_testData);

                    switch (selectedGraph)
                    {
                        case "Performance evaluation of predictions":
                            dataChar_csv_p2.Annotations.Clear();
                            _performanceVisualizer.VisualizePerformanceMetrics(_binaryModel.EvaluateModel(_testData), dataChar_csv_p2, "Binary Classification - Performance Evaluation");
                            txtBoxPerformanse_p2.Text = _performanceDescription.GetPredictionDistributionDescription();

                            var predictionMetrics = _binaryModel.EvaluateModel(_testData);
                            var predictionDescriptions = _performanceDescription.GetMetricsPredictionDistribution(predictionMetrics);
                            DisplayMetricsInColor(txtBoxPerformanceMetric_p2, predictionDescriptions);
                            break;

                        case "Receiver operating characteristic curve":
                            dataChar_csv_p2.Annotations.Clear();
                            _performanceVisualizer.VisualizeReceiverCurve(probabilities, actuals, dataChar_csv_p2, "Binary Classification - ROC Curve");
                            txtBoxPerformanse_p2.Text = _performanceDescription.GetReceiverCurveDescription();

                            var rocMetrics = _binaryModel.EvaluateModel(_testData);
                            var rocDescriptions = _performanceDescription.GetMetricsReceiverCurve(rocMetrics);
                            DisplayMetricsInColor(txtBoxPerformanceMetric_p2, rocDescriptions);
                            break;
                    }
                }
                else if (_textualModel != null)
                {
                    var (probabilities, actuals) = _textualModel.GetPredictionsAndLabels(_testData);

                    switch (selectedGraph)
                    {
                        case "Confusion matrix evaluation":
                            dataChar_csv_p2.Annotations.Clear();
                            _performanceVisualizer.VisualizeConfusionMatrix(_textualModel.EvaluateModel(_testData), dataChar_csv_p2, "Textual Classification - Confusion Matrix");
                            txtBoxPerformanse_p2.Text = _performanceDescription.GetConfusionMatrixDescription();

                            var confusionMetrics = _textualModel.EvaluateModel(_testData);
                            var confusionDescriptions = _performanceDescription.GetMetricsConfusionMatrix(confusionMetrics);
                            DisplayMetricsInColor(txtBoxPerformanceMetric_p2, confusionDescriptions);
                            break;

                        case "Cumulative gains chart":
                            dataChar_csv_p2.Annotations.Clear();
                            _performanceVisualizer.VisualizeCumulativeGainsChart(probabilities, actuals, dataChar_csv_p2, "Textual Classification - Cumulative Gains Chart");
                            txtBoxPerformanse_p2.Text = _performanceDescription.GetCumulativeGainsChartDescription();

                            var gainsMetrics = _textualModel.EvaluateModel(_testData);
                            var gainsDescriptions = _performanceDescription.GetMetricsCumulativeGainsChart(gainsMetrics);
                            DisplayMetricsInColor(txtBoxPerformanceMetric_p2, gainsDescriptions);
                            break;
                    }
                }
            }
        }
        private void SplitDataForTraining(string filePath, IDataPreparation dataPreparation, int featureCount)
        {
            if (dataPreparation is IBinaryDataPreparation binaryDataPreparation)
            {
                var (trainData, testData) = binaryDataPreparation.PrepareData(_mlContext, filePath, featureCount);
                _trainingData = trainData;
                _testData = testData;
            }
            else if (dataPreparation is ITextualDataPreparation textualDataPreparation)
            {
                string selectedAlgorithm = algorithmSelection_p2.SelectedItem?.ToString();
                if (selectedAlgorithm == null)
                {
                    throw new InvalidOperationException("No algorithm was selected for textual classification");
                }

                switch (selectedAlgorithm)
                {
                    case "Naive Bayes classifier":
                        var (trainDataNB, testDataNB) = textualDataPreparation.PrepareDataForNaiveBayes(_mlContext, filePath, featureCount);
                        _trainingData = trainDataNB;
                        _testData = testDataNB;
                        break;
                    case "Decision tree":
                        var (trainDataFF, testDataFF) = textualDataPreparation.PrepareDataForFastForest(_mlContext, filePath, featureCount);
                        _trainingData = trainDataFF;
                        _testData = testDataFF;
                        break;
                    default:
                        throw new InvalidOperationException("No algorithm was selected for textual classification");
                }
            }
        }

        private void ResetForm()
        {
            textBox_csv_p1.Text = "";
            textBox_validation_p1.Text = "";
            txtBoxMetric_p2.Text = "";
            txtBoxPerformanse_p2.Text = "";
            train_TextBox_p2.Text = "";
            txtBoxPerformanceMetric_p2.Text = "";

            comboBox_graph_p1.Items.Clear();
            comboBox_graph_p1.Text = "";
            comboBox_graph_p2.Items.Clear();
            comboBox_graph_p2.Text = "";

            dataChar_csv_p2.Series.Clear();
            dataChar_csv_p2.ChartAreas.Clear();
            dataChar_csv_p2.Titles.Clear();
            dataChar_csv_p2.Annotations.Clear();
            dataChar_csv_p2.Invalidate();

            dataGrid_csv_p1.DataSource = null;
            dataGrid_csv_p1.Refresh();

            dataChar_csv_p1.Series.Clear();
            dataChar_csv_p1.ChartAreas.Clear();

            algorithmSelection_p2.Items.Clear();
            algorithmSelection_p2.SelectedIndex = -1;
            algorithmSelection_p2.Text = "";

            _binaryModel = null;
            _textualModel = null;
            _trainingData = null;
            _testData = null;

            SetInitialChartOptions("reset");
            ResetAdditionalChartAndTextbox();

            _lastSelectedGraphP2 = "";
        }

        private void SetInitialChartOptions(string classificationResult)
        {
            comboBox_graph_p1.Items.Clear();
            comboBox_graph_p1.Text = "";

            if (classificationResult.Contains("binary classification"))
            {
                comboBox_graph_p1.Items.Add("Column Chart (vertical)");
                comboBox_graph_p1.SelectedItem = "Column Chart (vertical)";
            }
            else if (classificationResult.Contains("textual classification"))
            {
                comboBox_graph_p1.Items.Add("Pie Chart");
                comboBox_graph_p1.SelectedItem = "Pie Chart";
            }
        }

        private void UpdateChartOptions(string classificationResult)
        {
            string[] availableChartTypes = _chartTypeProvider.GetAvailableChartTypes(classificationResult);

            foreach (string chartType in availableChartTypes)
            {
                if (!comboBox_graph_p1.Items.Contains(chartType))
                {
                    comboBox_graph_p1.Items.Add(chartType);
                }
            }
        }

        private void DisplayDataOnChart(string classificationResult, DataTable dataTable)
        {
            if (classificationResult.Contains("binary classification"))
            {
                var outcomeColumn = _checkCharacteristic.FindBinaryColumn(dataTable);
                _binaryChartDisplay.DisplayDataOnChart(dataChar_csv_p1, dataTable, defaultChartType, outcomeColumn);
            }
            else if (classificationResult.Contains("textual classification"))
            {
                var classificationColumn = _checkCharacteristic.FindTextualColumn(dataTable);
                _textualChartDisplay.DisplayDataOnChart(dataChar_csv_p1, dataTable, defaultChartType, classificationColumn);
            }
        }

        private void DisplayMetricsInColor(RichTextBox richTextBox, List<(string text, Color color)> descriptions)
        {
            richTextBox.Clear();
            foreach (var (text, color) in descriptions)
            {
                AppendText(richTextBox, text + "\n", color);
            }
        }

        private void AppendText(RichTextBox box, string text, Color color)
        {
            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;

            box.SelectionColor = color;
            box.AppendText(text);
            box.SelectionColor = box.ForeColor;
        }

        private void LoadBinaryData(DataTable dataTable)
        {
            dataGrid_csv_p1.DataSource = dataTable;
            dataGrid_csv_p1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGrid_csv_p1.Refresh();

            foreach (DataGridViewColumn column in dataGrid_csv_p1.Columns)
            {
                column.Width = 40;
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
        }

        private void LoadTextualData(DataTable dataTable)
        {
            dataGrid_csv_p1.DataSource = dataTable;
            dataGrid_csv_p1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGrid_csv_p1.Refresh();

            foreach (DataGridViewColumn column in dataGrid_csv_p1.Columns)
            {
                column.Width = 40;
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
        }

        private void ResetAdditionalChartAndTextbox()
        {
            dataChar_csv_p2.Series.Clear();
            dataChar_csv_p2.ChartAreas.Clear();
            dataChar_csv_p2.Titles.Clear();
            dataChar_csv_p2.Annotations.Clear();
            dataChar_csv_p2.Invalidate();

            txtBoxPerformanse_p2.Clear();
            txtBoxPerformanceMetric_p2.Clear();
        }

        private void SetInitialTrainingOptions(string classificationResult)
        {
            algorithmSelection_p2.Items.Clear();

            if (classificationResult.Contains("binary classification"))
            {
                algorithmSelection_p2.Items.Add("Logistic regression");
                algorithmSelection_p2.Items.Add("Averaged Perceptron");
                algorithmSelection_p2.SelectedItem = "Logistic regression";
            }
            else if (classificationResult.Contains("textual classification"))
            {
                algorithmSelection_p2.Items.Add("Decision tree");
                algorithmSelection_p2.Items.Add("Naive Bayes classifier");
                algorithmSelection_p2.SelectedItem = "Decision tree";
            }
        }

        #endregion
    }
}