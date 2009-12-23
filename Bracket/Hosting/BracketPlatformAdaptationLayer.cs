using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Bracket.Events;
using IronRuby.Runtime;
using Microsoft.Scripting;

namespace Bracket.Hosting
{
    public class BracketPlatformAdaptationLayer : PlatformAdaptationLayer, IVirtualDirectory
    {

        private readonly Dictionary<string, IVirtualDirectory> _directories =
            new Dictionary<string, IVirtualDirectory>(StringComparer.CurrentCultureIgnoreCase);



        private readonly object _directoryLock = new object();


        public override bool DirectoryExists(string path)
        {
            path = ExpandPath(path);

            IVirtualDirectory dir = GetVirtualDirectoryForPath(path);

            if (dir == null)
                return base.DirectoryExists(path);
            return dir.DirectoryExists(path);
        }

        public override bool FileExists(string path)
        {
            path = ExpandPath(path);

            IVirtualDirectory dir = GetVirtualDirectoryForPath(path);
            if (dir == null)
                return base.FileExists(path);
            return dir.FileExists(path);
        }

        public override string[] GetDirectories(string path, string searchPattern)
        {
            path = ExpandPath(path);

            IVirtualDirectory dir = GetVirtualDirectoryForPath(path);
            if (dir == null)
                return base.GetDirectories(path, searchPattern);
            return dir.GetDirectories(path, searchPattern);
        }

        public override string[] GetFiles(string path, string searchPattern)
        {
            path = ExpandPath(path);

            IVirtualDirectory dir = GetVirtualDirectoryForPath(path);
            if (dir == null)
                return base.GetFiles(path, searchPattern);
            return dir.GetFiles(path, searchPattern);
        }

        public override Assembly LoadAssemblyFromPath(string path)
        {
            BracketEvent.Trace(this,String.Format("Loading assembly: {0}", path));
            path = ExpandPath(path);

            IVirtualDirectory dir = GetVirtualDirectoryForPath(path);
            if (dir == null)
                return base.LoadAssemblyFromPath(path);
            return dir.LoadAssemblyFromPath(path);
        }

        public override Stream OpenOutputFileStream(string path)
        {
            path = ExpandPath(path);

            IVirtualDirectory dir = GetVirtualDirectoryForPath(path);

            Stream output = (dir == null ? base.OpenOutputFileStream(path) : dir.OpenOutputFileStream(path));

            BracketEvent.PublishEvent(this, BracketEventType.CompleteOperation, BracketEvents.OpenOutputFile, path);

            return output;
        }

        public override Stream OpenInputFileStream(string path)
        {
            return OpenInputFileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
        }

        public override Stream OpenInputFileStream(string path, FileMode mode, FileAccess access, FileShare share)
        {
            return OpenInputFileStream(path, mode, access, share, 1024);
        }

        public override Stream OpenInputFileStream(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize)
        {
            path = ExpandPath(path);

            IVirtualDirectory dir = GetVirtualDirectoryForPath(path);
            Stream input = (dir == null
                                ? base.OpenInputFileStream(path, mode, access, share, bufferSize)
                                : dir.OpenInputFileStream(path, mode, access, share, bufferSize));

            BracketEvent.PublishEvent(this, BracketEventType.CompleteOperation, BracketEvents.OpenInputFile, path);

            return input;
        }

        private string ExpandPath(string path)
        {
            return RubyUtils.ExpandPath(this, path);
        }

        private IVirtualDirectory GetVirtualDirectoryForPath(string path)
        {
            //Yes, this is a lot of work to do in a lock. However my belief is that, in practice,
            // it will be pretty unlikely that multiple threads will contend for this lock, especially
            //as it will only be held for any substantial period of time while constructing a previously
            //unseen archive
            lock (_directoryLock)
            {
                //After profiling, it turns out ZipFileAtRoot is very expensive, so
                //I'm going to some trouble to avoid it.
                foreach (string key in _directories.Keys)
                {
                    if (path.StartsWith(key, StringComparison.CurrentCultureIgnoreCase))
                        return _directories[key];
                }

                if (path.IndexOf(".zip", StringComparison.CurrentCultureIgnoreCase) <= 0 &&
                    path.IndexOf(VirtualFileUtils.OBSCURED_ARCHIVE_EXTENSION, StringComparison.CurrentCultureIgnoreCase) <= 0)
                    return null;

                string zipRoot = VirtualFileUtils.ZipFileAtRoot(path);
                if (zipRoot == null)
                    return null;

                if (File.Exists(zipRoot))
                {
                    _directories.Add(zipRoot, new ZipArchiveDirectory(zipRoot));
                    return _directories[zipRoot];

                }
                return null;
            }
        }
    }
}
