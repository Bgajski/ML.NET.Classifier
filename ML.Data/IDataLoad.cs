using System.Data;

namespace ML.Data
{
    public interface IDataLoad
    {
        (DataTable dataTable, int featureCount) LoadData(string filePath);
    }
}