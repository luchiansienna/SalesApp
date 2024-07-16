using CsvHelper.Configuration;
using SalesApp.Domain;
using SalesApp.Domain.CustomConverters;

namespace SalesApp.Services
{
    public class SaleClassMap : ClassMap<Sale>
    {
        public SaleClassMap() {
            Map(m => m.Segment).Name("Segment").TypeConverter(new CustomStringConverter());
            Map(m => m.Country).Name("Country").TypeConverter(new CustomStringConverter());
            Map(m => m.Product).Name("Product").TypeConverter(new CustomStringConverter());
            Map(m => m.DiscountBand).Name("Discount Band").TypeConverter(new CustomStringConverter());
            Map(m => m.UnitsSold).Name("Units Sold").TypeConverter(new CustomDoubleConverter());
            Map(m => m.ManufacturingPrice).Name("Manufacturing Price").TypeConverter(new CustomPriceConverter());
            Map(m => m.SalePrice).Name("Sale Price").TypeConverter(new CustomPriceConverter()); ;
            Map(m => m.Date).Name("Date");
        }
    }
}
