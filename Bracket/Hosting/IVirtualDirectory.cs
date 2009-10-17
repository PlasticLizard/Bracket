using System.IO;
using System.Reflection;

namespace Bracket.Hosting
{
    public interface IVirtualDirectory
    {
        bool DirectoryExists(string path);
        bool FileExists(string path);
        string[] GetDirectories(string path, string searchPattern);
        string[] GetFiles(string path, string searchPattern);
        
        Assembly LoadAssemblyFromPath(string path);

        Stream OpenOutputFileStream(string path);

        Stream OpenInputFileStream(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize);
    }
}