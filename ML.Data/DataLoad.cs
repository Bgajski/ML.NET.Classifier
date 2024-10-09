using System.Globalization;
using System.Data;
using CsvHelper;
using CsvHelper.Configuration;

namespace ML.Data
{
    public class DataLoad : IDataLoad
    {
        public (DataTable dataTable, int featureCount) LoadData(string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
                throw new ArgumentException("The file path does not exist.", nameof(filePath));

            var dataTable = new DataTable();
            int featureCount = 0;

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ",",
                PrepareHeaderForMatch = args => args.Header.ToLower(),
                HasHeaderRecord = true
            };

            try
            {
                using var reader = new StreamReader(filePath);
                using var csv = new CsvReader(reader, config);
                using var dr = new CsvDataReader(csv);
                dataTable.Load(dr);
                featureCount = dataTable.Columns.Count - 1;
            }
            catch (Exception ex)
            {
                throw new Exception("Error loading data from CSV file.", ex);
            }

            if (dataTable.Rows.Count == 0 || dataTable.Columns.Count == 0)
                throw new Exception("Loaded data table is empty or has no columns.");

            return (dataTable, featureCount);
        }
    }
}