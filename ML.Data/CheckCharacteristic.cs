using System.Data;

namespace ML.Data
{
    public class CheckCharacteristic : ICheckCharacteristic
    {
        public string CheckClassificationType(DataTable dataTable)
        {
            if (dataTable == null || dataTable.Rows.Count == 0)
            {
                return "The dataset is not suitable for processing";
            }

            var binaryOutcomeColumn = FindBinaryColumn(dataTable);
            if (binaryOutcomeColumn != null)
            {
                return "The dataset is suitable for binary classification";
            }

            var textualClassificationColumn = FindTextualColumn(dataTable);
            if (textualClassificationColumn != null)
            {
                return "The dataset is suitable for textual classification";
            }

            return "The dataset is not suitable for processing";
        }

        public string FindBinaryColumn(DataTable dataTable)
        {
            foreach (DataColumn column in dataTable.Columns)
            {
                var distinctValues = dataTable.AsEnumerable()
                    .Select(row => row[column])
                    .Distinct()
                    .ToArray();

                if (distinctValues.Length == 2 && distinctValues.All(v => v is int || v is bool || v is byte || (v is string str && IsBinaryString(str))))
                {
                    return column.ColumnName;
                }
            }
            return null;
        }

        public string FindTextualColumn(DataTable dataTable)
        {
            foreach (DataColumn column in dataTable.Columns)
            {
                if (column.DataType == typeof(string))
                {
                    var valueCounts = dataTable.AsEnumerable()
                        .GroupBy(row => row[column]?.ToString())
                        .Select(g => new { Value = g.Key, Count = g.Count() })
                        .ToList();

                    if (valueCounts.Any(vc => vc.Count > 1) && valueCounts.Count <= 10)
                    {
                        return column.ColumnName;
                    }
                }
            }
            return null;
        }

        public bool IsBinaryString(string value)
        {
            return value == "0" || value == "1";
        }
    }
}