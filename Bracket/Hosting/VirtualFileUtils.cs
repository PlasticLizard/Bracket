using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using IronRuby.Runtime;

namespace Bracket.Hosting
{
    public static class VirtualFileUtils
    {
        private static readonly Regex PathSeparatorRegex = new Regex(@".*(\\|/)+.*",
                                                                    RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private static readonly Regex ZipRootRegex = new Regex(@"(([a-z]:(\\|/)|~))?(\w|\s|\\|/|\.|\$)*?(\.zip)(?=$|\\|/)", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public static string ZipFileAtRoot(string path)
        {
            MatchCollection zips = ZipRootRegex.Matches(path);
            if (zips.Count > 0)
                return zips[0].Value;
            return null;
        }


        public static bool IsTopLevel(string path)
        {
            if (String.IsNullOrEmpty(path))
                return false;
            return PathSeparatorRegex.IsMatch(path) == false;
        }

        public static string GetRoot(string path)
        {
            if (String.IsNullOrEmpty(path))
                return String.Empty;
            return path.Split(new[] { '\\', '/' }, StringSplitOptions.RemoveEmptyEntries)[0];
        }

        public static void CopyStream(Stream input, Stream output)
        {
            var buffer = new byte[32768];
            while (true)
            {
                int read = input.Read(buffer, 0, buffer.Length);
                if (read <= 0)
                    return;
                output.Write(buffer, 0, read);
            }

        }

        public static MemoryStream ToMemoryStream(this Stream input)
        {
            var output = new MemoryStream();
            CopyStream(input, output);
            output.Position = 0;
            return output;
        }

        public static string NormalizePath(string path)
        {
            if (String.IsNullOrEmpty(path))
                return path;
            path = RubyUtils.CanonicalizePath(path);
            return path.TrimEnd(new[] {'\\', '/'});
        }
    }
}
