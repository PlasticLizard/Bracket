using System.IO;
using System.Reflection;

namespace Bracket.Hosting
{
    public interface IVirtualDirectory
    {
        bool DirectoryExists(string path);
        bool FileExists(string path);
        string[] GetDirectoryEntires(string path, string searchPattern);
        string[] GetFileEntries(string path, string searchPattern);
        
        Assembly LoadAssemblyFromPath(string path);

        Stream OpenOutputFileStream(string path);

        Stream OpenInputFileStream(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize);
    }
}