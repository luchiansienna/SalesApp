using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Extensions.Configuration;
using SalesApp.Infrastructure.Contracts;
using SalesApp.Services.Contracts;
using System.Globalization;
using System.Text;

namespace SalesApp.Infrastructure
{
    public class DataRepository : IDataRepository, IDisposable
    {
        private readonly IFileManager fileManager;
        private readonly CsvConfiguration config;
        private readonly string salesCSVPath;
        private StreamReader? streamReader;
        private CsvReader? csvReader;
        private bool disposed = false;

        public DataRepository(IFileManager fileManager, IConfiguration configuration)
        {
            this.fileManager = fileManager;
            this.config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                Delimiter = ",",
                PrepareHeaderForMatch = (header) => header.Header.Trim()
            };
            salesCSVPath = configuration["SalesCSVPath"] ?? throw new Exception("SalesCSVPath not present in the configuration file.");
        }

        public IAsyncEnumerable<T> GetRecords<T>(CancellationToken cancellationToken)
        {
            streamReader = fileManager.StreamReader(salesCSVPath, Encoding.Latin1);
            csvReader = new CsvReader(streamReader, config);
            return csvReader.GetRecordsAsync<T>(cancellationToken);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    streamReader?.Dispose();
                    csvReader?.Dispose();
                }
                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
