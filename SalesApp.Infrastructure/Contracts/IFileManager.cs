using System.Text;

namespace SalesApp.Services.Contracts
{
    public interface IFileManager
    {
        public StreamReader StreamReader(string path, Encoding encoding);
    }
}
