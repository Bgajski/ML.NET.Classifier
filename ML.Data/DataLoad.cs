using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq; 
using CsvHelper;
using CsvHelper.Configuration;

namespace ML.Data
{
    /// <summary>
    /// loads CSV data into a DataTable and calculates the number of input features
    /// uses CsvHelper with ML.NET-compatible header options
    /// </summary>
    public class DataLoad : IDataLoad
    {
        /// <summary>
        /// loads data from a CSV file and returns the parsed DataTable and feature count
        /// automatically selects the label column (binary if possible, otherwise last column)
        /// </summary>
        public (DataTable dataTable, int featureCount) LoadData(string filePath)
        {
            // check if file path is valid and file exists
            if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
                throw new ArgumentException("File path does not exist.", nameof(filePath));

            var dataTable = new DataTable();

            // configure CsvHelper for consistent parsing and ML.NET compatibility
            var cfg = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ",",
                HasHeaderRecord = true,
                IgnoreBlankLines = true,
                BadDataFound = null,
                HeaderValidated = null, // skip strict header checks
                MissingFieldFound = null,
                TrimOptions = TrimOptions.Trim,
                PrepareHeaderForMatch = h => h.Header?.Trim()
            };

            try
            {
                using var reader = new StreamReader(filePath);
                using var csv = new CsvReader(reader, cfg);
                using var dr = new CsvDataReader(csv);
                dataTable.Load(dr);
            }
            catch (HeaderValidationException hex)
            {
                // provide clearer error message if header is malformed or duplicated
                throw new Exception("CSV header mismatch – duplicate / missing column?", hex);
            }

            // validate loaded data table
            if (dataTable.Rows.Count == 0 || dataTable.Columns.Count == 0)
                throw new Exception("Loaded data table is empty.");

            // try to detect binary label column automatically
            var checker = new CheckCharacteristic();
            string label = checker.FindBinaryColumn(dataTable)
                          ?? dataTable.Columns[^1].ColumnName; // fallback: last column

            // count all columns except the label column
            int featureCnt = dataTable.Columns
                                      .Cast<DataColumn>()
                                      .Count(c => !c.ColumnName
                                          .Equals(label, StringComparison.OrdinalIgnoreCase));

            return (dataTable, featureCnt);
        }
    }
}
