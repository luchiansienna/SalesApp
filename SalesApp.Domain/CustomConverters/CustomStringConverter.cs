using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace SalesApp.Domain.CustomConverters
{
    public class CustomStringConverter : DoubleConverter
    {
        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            return text.Trim();
        }
    }
}
