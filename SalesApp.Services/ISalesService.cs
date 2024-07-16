using CsvHelper.Configuration;

namespace SalesApp.Services
{
    public interface ISalesService
    {
        public IEnumerable<T> FetchFromCSVFile<T, TClassMap>(string salesCSVPath, int pageIndex, int pageSize) where TClassMap : ClassMap;
        public long CountAllRecords<T, TClassMap>(string salesCSVPath, int pageIndex, int pageSize) where TClassMap : ClassMap;
    }
}
