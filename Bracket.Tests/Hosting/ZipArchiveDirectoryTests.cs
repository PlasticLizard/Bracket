using System;
using System.IO;
using Bracket.Hosting;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace Bracket.Tests.Hosting
{
    [TestFixture]
    public class ZipArchiveDirectoryTests
    {
        private string zipName = "Fresh.zip";

        [Test]
        public void DirectoryExistsShouldIdentifyTopLevelDirectory()
        {
            //Given
            var dir = new ZipArchiveDirectory("Fresh.zip");
            //When
            bool wasFound = dir.DirectoryExists("Fresh");
            //Then
            Assert.IsTrue(wasFound);
        }

        [Test]
        public void DirectoryExistsShouldFailToFindNonExistentDirectory()
        {
            //Given
            var dir = new ZipArchiveDirectory("Fresh.zip");
            //When
            bool wasFound = dir.DirectoryExists(@"Fresh\ban");
            //Then
            Assert.IsFalse(wasFound);
        }

        [Test]
        public void DirectoryExistsShouldFindDirectoryWithoutRoot()
        {
            //Given
            var dir = new ZipArchiveDirectory("Fresh.zip");
            //when
            bool wasFound = dir.DirectoryExists(@"fresh\bin");
            //then
            Assert.IsTrue(wasFound);
        }

        [Test]
        public void DirectoryExistsRequiresFullPathAndWillNotRecurse()
        {
            //Given
            var dir = new ZipArchiveDirectory("Fresh.zip");
            //when
            bool wasFound = dir.DirectoryExists(@"bin");
            //then
            Assert.IsFalse(wasFound);
        }

        [Test]
        public void DirectoryExistsShouldNotFindFiles()
        {
            //Given
            var dir = new ZipArchiveDirectory("Fresh.zip");
            //when
            bool wasFound = dir.DirectoryExists("Fresh/README.txt");
            //then
            Assert.IsFalse(wasFound);
        }
       

        [Test]
        public void FileExistsShouldFindRootedFilePath()
        {
            //Given
            var dir = new ZipArchiveDirectory("Fresh.zip");
            //when
            bool wasFound = dir.FileExists(FullPath("fresh/readMe.txt"));
            //then
            Assert.IsTrue(wasFound);
        }

        [Test]
        public void FileExistsShouldFindUnrootedFilePath()
        {
            //Given
            var dir = new ZipArchiveDirectory("Fresh.zip");
            //when
            bool wasFound = dir.FileExists("fresh/README.txt");
            //then
            Assert.IsTrue(wasFound);
        }

        [Test]
        public void FileExistsShouldNotFindDirectory()
        {
            //Given
            var dir = new ZipArchiveDirectory("Fresh.zip");
            //when
            bool wasFound = dir.FileExists("fresh/bin");
            //then
            Assert.IsFalse(wasFound);
        }

        [Test]
        public void GetDirectoriesShouldReturnDirectoriesInRoot()
        {
            //Given
            var dir = new ZipArchiveDirectory("Fresh.zip");
            //when
            string[] directories = dir.GetDirectoryEntires(".", "*");
            //Then
            Assert.That(directories.Length,Is.EqualTo(1));
            Assert.That(directories[0], Is.EqualTo(FullPath("Fresh")));
        }

        [Test]
        public void GetDirectoriesShouldReturnDirectoriesInNestedDirectory()
        {
            //Given
            var dir = new ZipArchiveDirectory("Fresh.zip");
            //when
            string[] directories = dir.GetDirectoryEntires(@"Fresh\lib\IronRuby", "*");
            //then
            Assert.That(directories.Length,Is.EqualTo(2));
            Assert.That(String.Join(",", directories), Is.EqualTo(FullPath(@"Fresh/lib/IronRuby/digest") + "," + FullPath("Fresh/lib/IronRuby/test")));
        }

        [Test]
        public void GetFilesShouldReturnFilesInRoot()
        {
            //Given
            var dir = new ZipArchiveDirectory("Fresh.zip");
            //when
            string[] files = dir.GetFileEntries(".", "*");
            //then
            Assert.That(files.Length, Is.EqualTo(2));
            Assert.That(files[1], Is.EqualTo(FullPath("file1.txt")));
        }

        [Test]
        public void GetFilesShouldReturnFilesInNestedDirectory()
        {
            //given
            var dir = new ZipArchiveDirectory("Fresh.zip");
            //when
            string[] files = dir.GetFileEntries("Fresh", "*");
            //then
            Assert.That(files.Length, Is.EqualTo(4));
            Assert.That(files[files.Length - 1], Is.EqualTo(FullPath("Fresh/README.txt")));
        }

        [Test]
        public void IsAbsolutePathShouldMatchNameOfZipArchive()
        {
            //given
            var dir = new ZipArchiveDirectory("Fresh.zip");
            //when
            bool isAbsolute = dir.IsAbsolutePath(FullPath(@"happy\sad/whatever"));
            //then
            Assert.IsTrue(isAbsolute);

        }

        [Test]
        public void IsAbsolutePathShouldNotMatchRelativePath()
        {
            //given
            var dir = new ZipArchiveDirectory("Fresh.zip");
            //when
            bool isAbsolute = dir.IsAbsolutePath("Fresh/bin");
            //then
            Assert.IsFalse(isAbsolute);
        }

        [Test]
        public void IsAbsolutePathShouldNotMatchOtherZipArchives()
        {
            //given
            var dir = new ZipArchiveDirectory("Fresh.zip");
            //when
            bool isAbsolute = dir.IsAbsolutePath("Fresher.zip/tada");
            //then
            Assert.IsFalse(isAbsolute);
        }

        [Test]
        public void OpenInputFileStreamShouldProvideStreamForRequestedFileInRoot()
        {
            //given
            var dir = new ZipArchiveDirectory("Fresh.zip");
            //when
            string result = null;
            using(var reader = new StreamReader(dir.OpenInputFileStream("file1.txt",FileMode.Open,FileAccess.Read,FileShare.Read,0)))
            {
                result = reader.ReadToEnd();
            }
            //then
            Assert.AreEqual("Well hello there little fellah!", result);
        }
      

        private string FullPath(string path)
        {
            return VirtualFileUtils.NormalizePath(Path.Combine(Path.GetFullPath(zipName), path));
        }
    }
}
