using Microsoft.Extensions.Configuration;
using Moq;
using SalesApp.Services.Contracts;
using System.Text;

namespace SalesApp.Services.UnitTests
{
    public class Tests
    {
        SalesService SetupService(string content)
        {
            var mockFileManager = new Mock<IFileManager>();
            var inMemorySettings = new Dictionary<string, string?>
            {
                { "SalesCSVPath", "test.txt" }
            };

            IConfiguration mockConfiguration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            string fakeFileContents = content;
            byte[] fakeFileBytes = Encoding.UTF8.GetBytes(fakeFileContents);
            MemoryStream fakeMemoryStream = new MemoryStream(fakeFileBytes);

            mockFileManager.Setup(fileManager => fileManager.StreamReader(It.IsAny<string>(), It.IsAny<Encoding>()))
                           .Returns(() => new StreamReader(fakeMemoryStream, Encoding.UTF8));

            return new SalesService(mockFileManager.Object, mockConfiguration);
        }

        [Test]
        public async Task Should_Return_1_Row()
        {
            string fakeFileContents = "Segment,Country, Product , Discount Band ,Units Sold,Manufacturing Price,Sale Price,Date\r\nGovernment,Canada, Carretera , None ,1618.5,£3.00,£20.00,01/01/2014";
            var _salesService = SetupService(fakeFileContents);
            var result = await _salesService.Count(new SalesFilter() { });

            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public async Task The_Manufacturing_Price_Check()
        {
            string fakeFileContents = "Segment,Country, Product , Discount Band ,Units Sold,Manufacturing Price,Sale Price,Date\r\nGovernment,Canada, Carretera , None ,1618.5,£3.00,£20.00,01/01/2014";
            var _salesService = SetupService(fakeFileContents);
            var result = await _salesService.GetSales(new SalesFilter() { PageIndex = 0, PageSize = 10 });
            var resultedRows = result.ToList();

            Assert.That(resultedRows[0].ManufacturingPrice, Is.EqualTo(3));
        }

        [Test]
        public async Task The_Date_Check()
        {
            var _salesService = SetupService("Segment,Country, Product , Discount Band ,Units Sold,Manufacturing Price,Sale Price,Date\r\nGovernment,Canada, Carretera , None ,1618.5,£3.00,£20.00,01/01/2014");
            var result = await _salesService.GetSales(new SalesFilter() { PageIndex = 0, PageSize = 10 });
            var resultedRows = result.ToList();

            Assert.That(resultedRows[0].Date, Is.EqualTo(new DateOnly(2014, 1, 1)));
        }

        [Test]
        public async Task Should_Filter_By_Segment()
        {
            string fakeFileContents = "Segment,Country, Product , Discount Band ,Units Sold,Manufacturing Price,Sale Price,Date\r\nGovernment,Canada, Carretera , None ,1618.5,£3.00,£20.00,01/01/2014\r\nCorporate,USA, Velo , None ,1000,£5.00,£30.00,01/01/2015";
            var _salesService = SetupService(fakeFileContents);
            var result = await _salesService.GetSales(new SalesFilter() { Segment = "Government" });
            var resultedRows = result.ToList();

            Assert.That(resultedRows.Count, Is.EqualTo(1));
            Assert.That(resultedRows[0].Segment, Is.EqualTo("Government"));
        }

        [Test]
        public async Task Should_Filter_By_Country()
        {
            string fakeFileContents = "Segment,Country, Product , Discount Band ,Units Sold,Manufacturing Price,Sale Price,Date\r\nGovernment,Canada, Carretera , None ,1618.5,£3.00,£20.00,01/01/2014\r\nCorporate,USA, Velo , None ,1000,£5.00,£30.00,01/01/2015";
            var _salesService = SetupService(fakeFileContents);
            var result = await _salesService.GetSales(new SalesFilter() { Country = "USA" });
            var resultedRows = result.ToList();

            Assert.That(resultedRows.Count, Is.EqualTo(1));
            Assert.That(resultedRows[0].Country, Is.EqualTo("USA"));
        }

        [Test]
        public async Task Should_Filter_By_Product()
        {
            string fakeFileContents = "Segment,Country, Product , Discount Band ,Units Sold,Manufacturing Price,Sale Price,Date\r\nGovernment,Canada, Carretera , None ,1618.5,£3.00,£20.00,01/01/2014\r\nCorporate,USA, Velo , None ,1000,£5.00,£30.00,01/01/2015";
            var _salesService = SetupService(fakeFileContents);
            var result = await _salesService.GetSales(new SalesFilter() { Product = "Velo" });
            var resultedRows = result.ToList();

            Assert.That(resultedRows.Count, Is.EqualTo(1));
            Assert.That(resultedRows[0].Product, Is.EqualTo("Velo"));
        }

        [Test]
        public async Task Should_Filter_By_DiscountBand()
        {
            string fakeFileContents = "Segment,Country, Product , Discount Band ,Units Sold,Manufacturing Price,Sale Price,Date\r\nGovernment,Canada, Carretera , None ,1618.5,£3.00,£20.00,01/01/2014\r\nCorporate,USA, Velo , Low ,1000,£5.00,£30.00,01/01/2015";
            var _salesService = SetupService(fakeFileContents);
            var result = await _salesService.GetSales(new SalesFilter() { DiscountBand = "Low" });
            var resultedRows = result.ToList();

            Assert.That(resultedRows.Count, Is.EqualTo(1));
            Assert.That(resultedRows[0].DiscountBand, Is.EqualTo("Low"));
        }

        [Test]
        public async Task Should_Return_No_Records_For_Invalid_Filter()
        {
            string fakeFileContents = "Segment,Country, Product , Discount Band ,Units Sold,Manufacturing Price,Sale Price,Date\r\nGovernment,Canada, Carretera , None ,1618.5,£3.00,£20.00,01/01/2014\r\nCorporate,USA, Velo , Low ,1000,£5.00,£30.00,01/01/2015";
            var _salesService = SetupService(fakeFileContents);
            var result = await _salesService.GetSales(new SalesFilter() { Segment = "NonExistentSegment" });
            var resultedRows = result.ToList();

            Assert.That(resultedRows.Count, Is.EqualTo(0));
        }
    }
}
