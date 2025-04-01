using CsvHelper;
using System.Globalization;
using CsvHelper.Configuration;
using System.Text;
using Microsoft.Extensions.Configuration;
using SalesApp.Services.Contracts;
using SalesApp.Domain;


namespace SalesApp.Services
{
    public class SalesService : ISalesService
    {
        IFileManager fileManager;
        readonly CsvConfiguration config;
        private readonly string _salesCSVPath;


        public SalesService(IFileManager fileManager, IConfiguration configuration)
        {
            this.fileManager = fileManager;
            this.config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                Delimiter = ",",
                PrepareHeaderForMatch = (header) => header.Header.Trim()
            };
            _salesCSVPath = configuration["SalesCSVPath"] ?? throw new Exception("SalesCSVPath not present in the configuration file.");

        }
        public async Task<IEnumerable<Sale>> GetSales(SalesFilter filter, CancellationToken cancellationToken = default)
        {
            using var streamReader = fileManager.StreamReader(_salesCSVPath, Encoding.Latin1);
            using var csvReader = new CsvReader(streamReader, config);
            var records = csvReader.GetRecordsAsync<Sale>(cancellationToken);
            if (!string.IsNullOrEmpty(filter.Segment))
                records = records.Where(x => x.Segment.Trim() == filter.Segment.Trim());
            if (!string.IsNullOrEmpty(filter.Country))
                records = records.Where(x => x.Country.Trim() == filter.Country.Trim());
            if (!string.IsNullOrEmpty(filter.Product))
                records = records.Where(x => x.Product.Trim() == filter.Product.Trim());
            if (!string.IsNullOrEmpty(filter.DiscountBand))
                records = records.Where(x => x.DiscountBand.Trim() == filter.DiscountBand.Trim());
            if (filter.PageIndex is not null && filter.PageSize is not null)
                records = records.Skip((int)filter.PageIndex * (int)filter.PageSize).Take((int)filter.PageSize);

            return await records.ToListAsync(cancellationToken);
        }
        

        public async Task<long> Count(SalesFilter filter, CancellationToken cancellationToken = default)
        {
            using var streamReader = fileManager.StreamReader(_salesCSVPath, Encoding.Latin1);
            using var csvReader = new CsvReader(streamReader, config);
            var records = csvReader.GetRecordsAsync<Sale>(cancellationToken);
            if (!string.IsNullOrEmpty(filter.Segment))
                records = records.Where(x => x.Segment.Trim() == filter.Segment.Trim());
            if (!string.IsNullOrEmpty(filter.Country))
                records = records.Where(x => x.Country.Trim() == filter.Country.Trim());
            if (!string.IsNullOrEmpty(filter.Product))
                records = records.Where(x => x.Product.Trim() == filter.Product.Trim());
            if (!string.IsNullOrEmpty(filter.DiscountBand))
                records = records.Where(x => x.DiscountBand.Trim() == filter.DiscountBand.Trim());
            return await records.CountAsync(cancellationToken);
        }
        
    }
}

