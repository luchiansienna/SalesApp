using SalesApp.Services.Contracts;
using System.Text;

namespace SalesApp.Infrastructure;
public class FileManager : IFileManager
{
    public StreamReader StreamReader(string path, Encoding encoding) => new StreamReader(path, encoding);   
}

