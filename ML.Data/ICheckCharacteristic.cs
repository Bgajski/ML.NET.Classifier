using System.Data;

namespace ML.Data
{
    public interface ICheckCharacteristic
    {
        string CheckClassificationType(DataTable dataTable);
        string FindBinaryColumn(DataTable dataTable);
        string FindTextualColumn(DataTable dataTable);
    }
}