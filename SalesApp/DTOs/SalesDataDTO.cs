using SalesApp.Domain;

namespace SalesApp.DTOs
{
    public class SalesDataDTO
    {
        public IEnumerable<Sale> Data { get; set; }
        public long Count { get; set; }
    }
}
