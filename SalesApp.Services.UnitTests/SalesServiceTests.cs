using Microsoft.Extensions.Configuration;
using Moq;
using SalesApp.Domain;
using SalesApp.Services.Contracts;
using System.Text;

namespace SalesApp.Services.UnitTests
{
    public class Tests
    {
        SalesService SetupService(string content )
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
            var result = await _salesService.Count();

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

            Assert.That(resultedRows[0].Date, Is.EqualTo(new DateOnly(2014,1,1)));
        }
    }
}