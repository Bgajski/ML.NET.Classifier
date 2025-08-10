using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ML.Data
{
    /// <summary>
    /// analyzes a DataTable and determines whether it's suitable for classification tasks
    /// classification type can be binary or textual (small category set)
    /// </summary>
    public class CheckCharacteristic : ICheckCharacteristic
    {
        /// <summary>
        /// detects the classification type (binary/textual) based on column values and structure
        /// returns a string description of dataset suitability for ML tasks
        /// </summary>
        public string CheckClassificationType(DataTable table)
        {
            // check if table is empty or null
            if (table == null || table.Rows.Count == 0)
                return "The dataset is not suitable for processing";

            // try to find a valid binary column (with no nulls)
            var bin = FindBinaryColumn(table);
            if (bin != null && !ColumnContainsNulls(table, bin))
                return "The dataset is suitable for binary classification";

            // if no binary column found, try to find a valid textual column
            var txt = FindTextualColumn(table);
            if (txt != null)
                return "The dataset is suitable for textual classification";

            // if no valid column found
            return "The dataset is not suitable for processing";
        }

        /// <summary>
        /// finds a column with exactly two distinct binary-compatible values
        /// used to identify binary label columns in classification datasets
        /// </summary>
        public string FindBinaryColumn(DataTable table)
        {
            foreach (DataColumn col in table.Columns)
            {
                var distinct = new HashSet<object>();

                foreach (DataRow row in table.Rows)
                {
                    var v = row[col];
                    if (v != DBNull.Value)
                        distinct.Add(v);

                    if (distinct.Count > 2)
                        break;
                }

                // check if number of distinct values and type is acceptable for classification
                if (distinct.Count == 2 && distinct.All(IsBinaryValue))
                    return col.ColumnName;
            }
            return null;
        }

        /// <summary>
        /// finds a string column that could be used for categorical classification
        /// selects columns with 2–10 distinct non-null string values
        /// </summary>
        public string FindTextualColumn(DataTable table)
        {
            foreach (DataColumn col in table.Columns
                                            .Cast<DataColumn>() // cast to allow LINQ filters
                                            .Where(c => c.DataType == typeof(string)))
            {
                var uniques = new HashSet<string>();

                foreach (DataRow row in table.Rows)
                {
                    var s = row[col]?.ToString();
                    if (s != null)
                        uniques.Add(s);

                    if (uniques.Count > 10)
                        break;
                }

                // check if number of distinct values is acceptable for classification
                if (uniques.Count is > 1 and <= 10)
                    return col.ColumnName;
            }
            return null;
        }

        /// <summary>
        /// checks if a given object is a valid binary value
        /// </summary>
        private static bool IsBinaryValue(object v) =>
            v is int or bool or byte ||
            (v is string s && s.Trim() is "0" or "1");

        /// <summary>
        /// checks whether a specific column contains any null values
        /// used to ensure binary columns are clean
        /// </summary>
        private static bool ColumnContainsNulls(DataTable t, string col) =>
            t.AsEnumerable().Any(r => r.IsNull(col));
    }
}
