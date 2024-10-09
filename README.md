# ML.NET.Classifier

## Overview

ML.NET.Classifier is a .NET Windows Forms application that utilizes the ML.NET library to demonstrate binary and textual data classification process using relevant metrics and visual charts. 

## ML.NET.Classifier binary classification example

![ML.NET.Classifier_logistic_regression](/ML.ReadmeExtra/ML.NET_logistic_regression_example.png)

## Binary classification overview

The ML.NET.Classifier applies logistic regression and averaged perceptron algorithms to the binary classification of data, using the [Pima Indians Diabetes Database, Kaggle.com](https://www.kaggle.com/datasets/uciml/pima-indians-diabetes-database). to demonstrate how machine learning can categorize data points into two mutually exclusive groups, such as predicting the likelihood of diabetes based on features like blood glucose levels, age, and BMI.

## ML.NET.Classifier textual classification example

![ML.NET.Classifier_decision_tree](/ML.ReadmeExtra/ML.NET_decision_tree_example.png)

## Textual classification overview

For textual classification, decision tree and naive Bayes algorithms are utilized, using the [SMS Spam Collection Dataset, Kaggle.com](https://www.kaggle.com/datasets/uciml/sms-spam-collection-dataset) to distinguish spam from ham messages.

## Algorithms and Evaluation Methods

### Binary Classification: 

- Algorithm model - logistic regression and averaged perceptron. 
- Model metric - accuracy, precision, recall.
- Performance metric - performance evaluation of predictions, receiver operating characteristic curve.

### Textual Classification:

- Algorithm model - decision tree and naive Bayes classifier.
- Model metric - macro accuracy, micro accuracy, log Loss.
- Performance metric - confusion matrix evaluation, cumulative gains chart.

## Dataset

- Binary classification dataset: [Pima Indians Diabetes Database, Kaggle.com](https://www.kaggle.com/datasets/uciml/pima-indians-diabetes-database).
- Textual classification dataset: [SMS Spam Collection Dataset, Kaggle.com](https://www.kaggle.com/datasets/uciml/sms-spam-collection-dataset).

## Libraries

- .NET (v8.0)
- ML.NET (v3.0.1)
- Microsoft.ML.LightGbm (v3.0.1)
- Microsoft.ML.CpuMath (v3.0.1)
- WinForms.DataVisualization (v1.9.2)
- CsvHelper (v32.0.3)
- Microsoft.NET.Test.Sdk (v17.6.0)
- NUnit (v3.13.3)