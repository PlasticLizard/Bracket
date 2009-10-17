using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Ionic.Zip;

namespace Bracket.Hosting
{
    public class ZipArchiveOutputStream : MemoryStream
    {
        private readonly ZipFile _zipFile;

        public ZipArchiveOutputStream(ZipFile zipFile)
        {
            _zipFile = zipFile;
        }

        public override void Close()
        {
            Flush();
            base.Close();
            _zipFile.Save();

        }
    }
}
