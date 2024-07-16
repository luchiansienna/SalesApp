using CsvHelper.Configuration;
using CsvHelper;
using CsvHelper.TypeConversion;

namespace SalesApp.Domain.CustomConverters
{
    public class CustomPriceConverter : DecimalConverter
    {
        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            var numberValue = text.Replace("£", "");

            if (decimal.TryParse(numberValue, out var result))
            {
                return result;
            }
            else
            {
                return decimal.Zero;
            }
        }
    }
}
