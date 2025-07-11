using System.Data;

namespace ML.Data
{
    /// <summary>
    /// provides method to load data from CSV and count input features
    /// </summary>
    public interface IDataLoad
    {
        (DataTable dataTable, int featureCount) LoadData(string filePath);
    }
}
