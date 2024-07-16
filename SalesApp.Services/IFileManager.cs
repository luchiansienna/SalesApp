using System.Text;

namespace SalesApp.Services
{
    public interface IFileManager
    {
        public StreamReader StreamReader(string path, Encoding encoding);
    }
}
