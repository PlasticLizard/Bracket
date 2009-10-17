using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Ionic.Zip;

namespace Bracket.Hosting
{
    public class ZipArchiveDirectory : IVirtualDirectory
    {
        private readonly ZipFile _storage;
        private readonly object _cacheLock = new object();
        private string _storagePath;

        private readonly Dictionary<string, int> _cache =
            new Dictionary<string, int>(StringComparer.CurrentCultureIgnoreCase);

        public ZipArchiveDirectory(string storagePath)
        {
            if (String.IsNullOrEmpty(storagePath))
                throw new ArgumentNullException("storagePath");
            if (!File.Exists(storagePath))
                throw new FileNotFoundException(storagePath + " does not resolve to an actual Zip Archive");

            _storagePath = VirtualFileUtils.NormalizePath(Path.GetFullPath(storagePath));

            _storage = ZipFile.Read(_storagePath);
            _storage.CaseSensitiveRetrieval = false;

            for (int i = 0; i < _storage.EntryFileNames.Count; i++)
                _cache.Add(VirtualFileUtils.NormalizePath(_storage.EntryFileNames[i]), i);

        }

        public bool DirectoryExists(string path)
        {
            path = RemoveRoot(path);
            foreach (ZipEntry entry in _storage.Entries)
            {
                string normalizedEntry = VirtualFileUtils.NormalizePath(entry.FileName);
                if (entry.IsDirectory && String.Compare(normalizedEntry, path, true) == 0)
                    return true;
                if (normalizedEntry.Length > path.Length && normalizedEntry.StartsWith(path))
                    return true;
            }
            return false;
        }

        public bool FileExists(string path)
        {
            ZipEntry entry = GetEntry(path);
            return entry != null && entry.IsDirectory == false;
        }

        public string[] GetDirectories(string path, string searchPattern)
        {
            //TODO:Ignoring searchPattern for the moment
            if (searchPattern != "*")
                throw new NotImplementedException("Search patterns other than * are not yet implemented");

            var found = new List<string>();

            if (IsRoot(path))
            {
                foreach (ZipEntry entry in _storage.Entries)
                {
                    string[] parts = entry.FileName.Split(new[] { '\\', '/' }, StringSplitOptions.RemoveEmptyEntries);
                    if ((parts.Length == 1 && entry.IsDirectory) || parts.Length > 1)
                    {
                        string dir = GetFullName(parts[0]);

                        if (!found.Contains(dir))
                            found.Add(dir);
                    }

                }
                return found.ToArray();
            }

            ZipEntry root = GetEntry(path);
            if (root == null)
                return new string[0];

            string target = VirtualFileUtils.NormalizePath(root.FileName);

            foreach (ZipEntry entry in _storage.Entries)
            {
                if (entry.IsDirectory == false)
                    continue;

                string normalizedEntry = VirtualFileUtils.NormalizePath(Path.GetDirectoryName(entry.FileName));
                int lastSlash = normalizedEntry.LastIndexOf('/');
                string entryDirectoryName = lastSlash > 0
                                                ? normalizedEntry.Substring(0, normalizedEntry.LastIndexOf('/'))
                                                : String.Empty;

                if (String.Compare(target, entryDirectoryName, true) == 0)
                    found.Add(GetFullName(normalizedEntry));

            }

            return found.ToArray();

        }

        public string[] GetFiles(string path, string searchPattern)
        {
            //TODO:Ignoring searchPattern for the moment
            if (searchPattern != "*")
                throw new NotImplementedException("Search patterns other than * are not yet implemented");


            var found = new List<string>();

            if (IsRoot(path))
            {
                foreach (ZipEntry entry in _storage.Entries.Where(entry => entry.IsDirectory == false && VirtualFileUtils.IsTopLevel(entry.FileName)))
                {
                    found.Add(GetFullName(entry.FileName));
                }
                return found.ToArray();
            }

            foreach (ZipEntry entry in _storage.Entries)
            {
                if (entry.IsDirectory)
                    continue;

                string normalizedFile = VirtualFileUtils.NormalizePath(entry.FileName);
                string directory = VirtualFileUtils.NormalizePath(Path.GetDirectoryName(normalizedFile));
                string target = RemoveRoot(path);

                if (String.Compare(target, directory, true) == 0)
                    found.Add(GetFullName(normalizedFile));
            }

            return found.ToArray();
        }

        public Assembly LoadAssemblyFromPath(string path)
        {
            ZipEntry entry = GetEntry(path);
            if (entry == null || entry.IsDirectory)
                return null;

            var asmStream = new MemoryStream();
            entry.Extract(asmStream);
            return Assembly.Load(asmStream.GetBuffer());
        }

        public Stream OpenOutputFileStream(string path)
        {
            string directory = VirtualFileUtils.NormalizePath(Path.GetDirectoryName(path));
            string fileName = Path.GetFileName(path);

            Stream output = new ZipArchiveOutputStream(_storage);
            _storage.AddEntry(fileName, directory, output);
            return output;
        }

        public Stream OpenInputFileStream(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize)
        {
            //Unfortunately, the stream the zip reader uses is not seekable, and the DLR requires seekable
            //streams to be returnd from this method.
            ZipEntry entry = GetEntry(path);
            if (entry != null && entry.IsDirectory == false)
            {
                Stream reader = entry.OpenReader();
                if (reader.CanSeek && reader.CanRead)
                    new BufferedStream(reader);

                using (reader)
                {
                    return reader.ToMemoryStream();
                }
            }
            throw new DirectoryNotFoundException(GetFullName(path) + " cannot be found and cannot be opened for reading.");
        }

        public bool IsAbsolutePath(string path)
        {
            path = VirtualFileUtils.NormalizePath(path);
            return path.StartsWith(_storagePath, StringComparison.CurrentCultureIgnoreCase);
        }

        private ZipEntry GetEntry(string path)
        {
            if (String.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");

            path = RemoveRoot(path);

            if (_cache.ContainsKey(path))
                return _storage[_cache[path]];
            
            return null;
        }

        private string RemoveRoot(string path)
        {
            path = VirtualFileUtils.NormalizePath(path);
            string root = _storagePath;

            if (path.StartsWith(root))
                path = path.Substring(root.Length);
            return path.TrimStart('\\', '/');
        }

        private bool IsRoot(string path)
        {
            return String.IsNullOrEmpty(path) ||
                   path == "." ||
                   String.Compare(path, _storagePath, StringComparison.CurrentCultureIgnoreCase) == 0;
        }

        private string GetFullName(string localPath)
        {
            return _storagePath + "/" + VirtualFileUtils.NormalizePath(localPath);
        }

    }
}