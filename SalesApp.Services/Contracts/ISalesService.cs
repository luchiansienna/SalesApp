using SalesApp.Domain;

namespace SalesApp.Services.Contracts
{
    public interface ISalesService
    {
        Task<IEnumerable<Sale>> GetSales(SalesFilter filter, CancellationToken cancellationToken = default);
        Task<long> Count(SalesFilter filter, CancellationToken cancellationToken = default);
    }
}
