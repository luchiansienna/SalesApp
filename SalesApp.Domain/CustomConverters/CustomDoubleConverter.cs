using CsvHelper.Configuration;
using CsvHelper;
using CsvHelper.TypeConversion;

namespace SalesApp.Domain.CustomConverters
{
    public class CustomDoubleConverter : DoubleConverter
    {
        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            if (double.TryParse(text, out var result))
            {
                return result;
            }
            else
            {
                return (double)0;
            }
        }
    }
}
