# ML.NET.Classifier

## 🧾 Overview

ML.NET.Classifier is a Windows Forms application built with ML.NET. It demonstrates binary and textual classification using real-world datasets, core machine learning algorithms, and visual performance metrics.

The application enables users to:

- Load and preprocess binary or textual datasets
- Train machine learning models for classification
- Evaluate model performance using key metrics and graphical analysis
- Visualize results through ROC curves, confusion matrices, and cumulative gains charts

## ML.NET.Classifier binary classification example

![ML.NET.Classifier_logistic_regression](/ML.ReadmeExtra/ML.NET_logistic_regression_example.png)

## Binary classification overview

The ML.NET.Classifier applies Logistic regression and Averaged perceptron algorithms to the binary classification of data, using the [Pima Indians Diabetes Database, Kaggle.com, Public Domain / CC BY 4.0](https://www.kaggle.com/datasets/uciml/pima-indians-diabetes-database) to demonstrate how machine learning can categorize data points into two mutually exclusive groups, such as predicting the likelihood of diabetes based on features like blood glucose levels, age, and BMI. 

## ML.NET.Classifier textual classification example

![ML.NET.Classifier_decision_tree](/ML.ReadmeExtra/ML.NET_decision_tree_example.png)

## Textual classification overview

For textual classification, Decision tree and Naive Bayes algorithms are utilized, using the [SMS Spam Collection Dataset, Kaggle.com, CC BY-SA 4.0 ](https://www.kaggle.com/datasets/uciml/sms-spam-collection-dataset) to distinguish spam from ham messages.

## 🤖 Algorithms and Evaluation Methods

### 🧾 Binary Classification: 

Algorithms: Logistic Regression, Averaged Perceptron

Metrics:
Accuracy, Precision, Recall
- Positive Predictive Value (PPV), True Positive Rate (TPR)
- Negative Predictive Value (NPV), True Negative Rate (TNR)
- Area Under ROC Curve (AUC)

Results:

| Model               | Accuracy | Precision | Recall | AUC  | PPV  | TPR  | NPV  | TNR  |
| ------------------- | -------- | --------- | ------ | ---- | ---- | ---- | ---- | ---- |
| Logistic Regression | 0.79     | 0.67      | 0.74   | 0.86 | 0.67 | 0.74 | 0.86 | 0.82 |
| Averaged Perceptron | 0.75     | 0.65      | 0.56   | 0.83 | 0.65 | 0.56 | 0.79 | 0.84 |

 Overview:

- Logistic Regression (Accuracy: 0.79, AUC: 0.86) shows high sensitivity (TPR: 0.74) and strong reliability in ruling out non-diabetic cases (NPV: 0.86, TNR: 0.82), making it suitable for clinical screening tasks.

- Averaged Perceptron (Accuracy: 0.75, AUC: 0.83) performs slightly lower but offers consistent predictions with good generalization.

These models provide a practical balance between sensitivity and specificity for early-stage health risk prediction.

### 🧾 Textual Classification:

Algorithms: Decision Tree (FastForest), Naive Bayes
Metrics:
- Macro Accuracy, Micro Accuracy, Log Loss
- Confusion Matrix
- Cumulative Gains Chart

Results:

| Model         | Macro Accuracy | Micro Accuracy | Log Loss |
| ------------- | -------------- | -------------- | -------- |
| Decision Tree | 0.97           | 0.97           | 0.07     |
| Naive Bayes   | 0.96           | 0.95           | 34.54    |

Overview::

- Decision Tree (Macro Accuracy: 0.97, Log Loss: 0.07) delivers highly confident and balanced classification of spam and ham messages, making it suitable for robust spam filtering.

- Naive Bayes (Macro Accuracy: 0.96) is fast and lightweight, performing well in environments where speed is prioritized over calibration.

Both models classify SMS messages with high overall accuracy, with Decision Tree showing stronger confidence and class separation.

## 🧾 Dataset

- Binary classification dataset: [Pima Indians Diabetes Database, Kaggle.com](https://www.kaggle.com/datasets/uciml/pima-indians-diabetes-database).
- Textual classification dataset: [SMS Spam Collection Dataset, Kaggle.com](https://www.kaggle.com/datasets/uciml/sms-spam-collection-dataset).

## 📦 Libraries

- .NET (v8.0)
- ML.NET (v4.0.2)
- Microsoft.ML.LightGbm (v4.0.2)
- Microsoft.ML.CpuMath (v4.0.2)
- WinForms.DataVisualization (v1.9.2)
- CsvHelper (v32.0.3)
- Microsoft.NET.Test.Sdk (v17.6.0)
- NUnit (v3.13.3)

## 🚀 Launch 

🛠️ Requirements
.NET 8.0 SDK or newer
Visual Studio 2022 (v17.8+) or VS Code with C# Dev Kit

1. Clone the repository:

- git clone https://github.com/Bgajski/ML.NET.Classifier
- cd ML.NET.Classifier

2. Run
   
- dotnet build
- dotnet run --project ML.NET.Classifier
