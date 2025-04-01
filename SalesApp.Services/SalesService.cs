using Microsoft.Extensions.Configuration;
using SalesApp.Services.Contracts;
using SalesApp.Domain;
using SalesApp.Infrastructure;
using SalesApp.Domain.Extensions;

namespace SalesApp.Services
{
    public class SalesService : ISalesService
    {
        private readonly IFileManager _fileManager;
        private readonly IConfiguration _configuration;

        public SalesService(IFileManager fileManager, IConfiguration configuration)
        {
            _fileManager = fileManager;
            _configuration = configuration;
        }

        private IAsyncEnumerable<Sale> ApplyFilters(IAsyncEnumerable<Sale> records, SalesFilter filter)
        {
            if (!string.IsNullOrEmpty(filter.Segment))
                records = records.Where(x => x.Segment.Trim() == filter.Segment.Trim());
            if (!string.IsNullOrEmpty(filter.Country))
                records = records.Where(x => x.Country.Trim() == filter.Country.Trim());
            if (!string.IsNullOrEmpty(filter.Product))
                records = records.Where(x => x.Product.Trim() == filter.Product.Trim());
            if (!string.IsNullOrEmpty(filter.DiscountBand))
                records = records.Where(x => x.DiscountBand.Trim() == filter.DiscountBand.Trim());
            return records;
        }

        public async Task<IEnumerable<Sale>> GetSales(SalesFilter filter, CancellationToken cancellationToken = default)
        {
            using var salesRepository = new DataRepository(_fileManager, _configuration);
            var records = salesRepository.GetRecords<Sale>(cancellationToken);

            records = ApplyFilters(records, filter);

            if (filter.PageIndex is not null && filter.PageSize is not null)
                records = records.Skip((int)filter.PageIndex * (int)filter.PageSize).Take((int)filter.PageSize);

            return (await records.ToListAsync(cancellationToken)).TrimAllStringFields();
        }

        public async Task<long> Count(SalesFilter filter, CancellationToken cancellationToken = default)
        {
            using var salesRepository = new DataRepository(_fileManager, _configuration);
            var records = salesRepository.GetRecords<Sale>(cancellationToken);

            records = ApplyFilters(records, filter);

            return await records.CountAsync(cancellationToken);
        }
    }
}
