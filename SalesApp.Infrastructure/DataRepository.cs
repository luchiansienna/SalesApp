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
        private readonly IFileManager _fileManager;
        private readonly CsvConfiguration config;
        private readonly string salesCSVPath;
        private StreamReader? _streamReader;
        private CsvReader? _csvReader;
        private bool disposed = false;
        public DataRepository(IFileManager fileManager, IConfiguration configuration)
        {
            config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                Delimiter = ",",
                PrepareHeaderForMatch = (header) => header.Header.Trim()
            };
            salesCSVPath = configuration["SalesCSVPath"] ?? throw new Exception("SalesCSVPath not present in the configuration file.");
            _fileManager = fileManager;
        }

        public IAsyncEnumerable<T> GetRecords<T>(CancellationToken cancellationToken)
        {
            _streamReader = _fileManager.StreamReader(salesCSVPath, Encoding.Latin1);
            _csvReader = new CsvReader(_streamReader, config);
            return _csvReader.GetRecordsAsync<T>(cancellationToken);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _streamReader?.Dispose();
                    _csvReader?.Dispose();
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
