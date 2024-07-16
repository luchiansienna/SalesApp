using System.Diagnostics;
using CsvHelper;
using System.Globalization;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using SalesApp.Domain.CustomConverters;

namespace SalesApp.Domain
{
    public class Sale
    {
        [Name("Segment")]
        public string Segment { get; set; }
        [Name("Country")]
        public string Country { get; set; }
        [Name("Product")]
        public string Product { get; set; }
        [Name("Discount Band")]
       
        public string DiscountBand { get; set; }
        [Name("Units Sold")]
        [TypeConverter(typeof(CustomDoubleConverter))]
        public double UnitsSold { get; set; }
        [Name("Manufacturing Price")]
        [TypeConverter(typeof(CustomPriceConverter))]
        public decimal ManufacturingPrice { get; set; }
        [Name("Sale Price")]
        [TypeConverter(typeof(CustomPriceConverter))]
        public decimal SalePrice { get; set; }
        [Name("Date")]
        public DateOnly Date { get; set; }

    }
}
