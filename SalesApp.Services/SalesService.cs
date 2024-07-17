using CsvHelper;
using System.Globalization;
using CsvHelper.Configuration;
using System.Text;

namespace SalesApp.Services
{
    public class SalesService : ISalesService
    {
        IFileManager fileManager;
        readonly CsvConfiguration config;
        public SalesService(IFileManager fileManager)
        {
            this.fileManager = fileManager;
            this.config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                Delimiter = ",",
                PrepareHeaderForMatch = (header) => header.Header.Trim()
            };
        }
        public IEnumerable<T> FetchFromCSVFile<T, TClassMap>(string csvPath, int pageIndex, int pageSize) where TClassMap : ClassMap
        {
            using (var streamReader = fileManager.StreamReader(csvPath, Encoding.Latin1))
            using (var csvReader = new CsvReader(streamReader, config))
            {
                csvReader.Context.RegisterClassMap<TClassMap>();
                return csvReader.GetRecords<T>().Skip(pageIndex * pageSize).Take(pageSize).ToList();
            }
        }
        public long CountAllRecords<T, TClassMap>(string csvPath) where TClassMap : ClassMap
        {
            using (var streamReader = fileManager.StreamReader(csvPath, Encoding.Latin1))
            using (var csvReader = new CsvReader(streamReader, config))
            {
                csvReader.Context.RegisterClassMap<TClassMap>();
                return csvReader.GetRecords<T>().Count();
            }
        }
    }
}

