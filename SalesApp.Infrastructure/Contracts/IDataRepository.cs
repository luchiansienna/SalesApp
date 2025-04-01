using SalesApp.Domain;

namespace SalesApp.Infrastructure.Contracts
{
    public interface IDataRepository
    {
        IAsyncEnumerable<T> GetRecords<T>(CancellationToken cancellationToken);
    }
}