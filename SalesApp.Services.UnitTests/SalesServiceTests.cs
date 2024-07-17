using CsvHelper;
using Moq;
using SalesApp.Domain;
using System.Text;

namespace SalesApp.Services.UnitTests
{
    public class Tests
    {
        SalesService SetupService(string content )
        {
            var mockFileManager = new Mock<IFileManager>();
            string fakeFileContents = content;
            byte[] fakeFileBytes = Encoding.UTF8.GetBytes(fakeFileContents);
            MemoryStream fakeMemoryStream = new MemoryStream(fakeFileBytes);

            mockFileManager.Setup(fileManager => fileManager.StreamReader(It.IsAny<string>(), It.IsAny<Encoding>()))
                           .Returns(() => new StreamReader(fakeMemoryStream, Encoding.UTF8));

            return new SalesService(mockFileManager.Object);
        }

        [Test]
        public void Should_Return_1_Row()
        {
           string fakeFileContents = "Segment,Country, Product , Discount Band ,Units Sold,Manufacturing Price,Sale Price,Date\r\nGovernment,Canada, Carretera , None ,1618.5,£3.00,£20.00,01/01/2014";
            var _salesService = SetupService(fakeFileContents);
            var result = _salesService.CountAllRecords<Sale,SaleClassMap>("test.txt");

            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public void The_Manufacturing_Price_Check()
        {
            string fakeFileContents = "Segment,Country, Product , Discount Band ,Units Sold,Manufacturing Price,Sale Price,Date\r\nGovernment,Canada, Carretera , None ,1618.5,£3.00,£20.00,01/01/2014";
            var _salesService = SetupService(fakeFileContents);
            var result = _salesService.FetchFromCSVFile<Sale, SaleClassMap>("test.txt", 0, 10);
            var resultedRows = result.ToList();

            Assert.That(resultedRows[0].ManufacturingPrice, Is.EqualTo(3));
        }

        [Test]
        public void The_Date_Check()
        {
            var _salesService = SetupService("Segment,Country, Product , Discount Band ,Units Sold,Manufacturing Price,Sale Price,Date\r\nGovernment,Canada, Carretera , None ,1618.5,£3.00,£20.00,01/01/2014");
            var result = _salesService.FetchFromCSVFile<Sale, SaleClassMap>("test.txt", 0, 10);
            var resultedRows = result.ToList();

            Assert.That(resultedRows[0].Date, Is.EqualTo(new DateOnly(2014,1,1)));
        }
    }
}