using System.Data;

namespace ML.Data
{
    /// <summary>
    /// provides methods to analyze a DataTable and detect classification type
    /// </summary>
    public interface ICheckCharacteristic
    {
        string CheckClassificationType(DataTable dataTable);
        string FindBinaryColumn(DataTable dataTable);
        string FindTextualColumn(DataTable dataTable);
    }
}
