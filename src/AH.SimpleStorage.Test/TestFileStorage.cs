using System.IO;
using AH.SimpleStorage.Implementations;
using NUnit.Framework;
using Shouldly;

namespace AH.SimpleStorage.Test
{
    [TestFixture]
    class TestFileStorage
    {
        private readonly string baseFolder = "c:\\AH.SimpleStorage.Test";
        [SetUp]
        public void SetUp()
        {
            if(Directory.Exists(baseFolder))
                Directory.Delete(baseFolder, true);
            Directory.CreateDirectory(baseFolder);
        }

        [TearDown]
        public void TearDown()
        {
            if (Directory.Exists(baseFolder))
                Directory.Delete(baseFolder, true);
        }

        [Test]
        public void ReadTextTest()
        {
            var storage = CreateDefaultFilesStructure();
            var content = storage.ReadTextFromFile(baseFolder + "\\File1");
            content.ShouldBe("File1 Content");
        }


        [Test]
        public void StreamWriterTest()
        {
            var storage = CreateDefaultFilesStructure();
            using (var stream = storage.WriteStreamFromFile(baseFolder + "\\File3"))
            {
                stream.Write("File1 content");
                stream.Flush();
                stream.Close();
            }
            var content = storage.ReadTextFromFile(baseFolder + "\\File3");
            content.ShouldBe("File1 content");
        }

        [Test]
        public void StreamReaderTest()
        {
            var storage = CreateDefaultFilesStructure();
            var fileName = baseFolder + "\\File1";
            using (var stream = storage.ReadStreamFromFile(fileName))
            {
                var content = stream.ReadToEnd();
                content.ShouldBe("File1 Content");
            }
        }

        [Test]
        public void DeleteFileTest()
        {
            var storage = CreateDefaultFilesStructure();
            storage.DeleteFile(baseFolder + "\\File1");
            storage.GetFiles(baseFolder).Count.ShouldBe(2);
        }

        [Test]
        public void DeleteDirectoryTest()
        {
            var storage = CreateDefaultFilesStructure();
            storage.DeleteDirectory(baseFolder + "\\Folder1");
            storage.GetFiles(baseFolder).Count.ShouldBe(3);
            storage.GetDirectories(baseFolder).Count.ShouldBe(2);
        }


        [Test]
        public void RenameFileTest()
        {
            var storage = CreateDefaultFilesStructure();
            storage.RenameFile(baseFolder + "\\File1", baseFolder + "\\File555");
            storage.GetFiles(baseFolder).Count.ShouldBe(3);
            storage.ReadTextFromFile(baseFolder + "\\File555").ShouldBe("File1 Content");
        }

        [Test]
        public void RenameFileTestWithMove()
        {
            var storage = CreateDefaultFilesStructure();
            storage.RenameFile(baseFolder + "\\File1", baseFolder + "\\Folder1\\File555");
            storage.GetFiles(baseFolder).Count.ShouldBe(2);
            storage.ReadTextFromFile(baseFolder + "\\Folder1\\File555").ShouldBe("File1 Content");
        }


        [Test]
        public void RenameDirectoryTest()
        {
            var storage = CreateDefaultFilesStructure();
            storage.RenameDirectory(baseFolder + "\\Folder1", baseFolder + "\\Folder111");
            storage.GetDirectories(baseFolder).Count.ShouldBe(3);
            storage.ReadTextFromFile(baseFolder + "\\Folder111\\File11").ShouldBe("File11 Content");
        }

        [Test]
        public void RenameDirectoryWithMoveTest()
        {
            var storage = CreateDefaultFilesStructure();
            storage.RenameDirectory(baseFolder + "\\Folder2", baseFolder + "\\Folder1\\Folder222");
            storage.GetDirectories(baseFolder).Count.ShouldBe(2);
            storage.ReadTextFromFile(baseFolder + "\\Folder1\\File11").ShouldBe("File11 Content");
            storage.ReadTextFromFile(baseFolder + "\\Folder1\\Folder222\\File21").ShouldBe("File21 Content");
            storage.ReadTextFromFile(baseFolder + "\\Folder1\\Folder222\\File22").ShouldBe("File22 Content");

            storage.RenameDirectory(baseFolder + "\\Folder1", baseFolder + "\\Folder3\\Folder111");
            storage.GetDirectories(baseFolder).Count.ShouldBe(1);
            storage.ReadTextFromFile(baseFolder + "\\Folder3\\Folder111\\File11").ShouldBe("File11 Content");
            storage.ReadTextFromFile(baseFolder + "\\Folder3\\Folder111\\Folder222\\File21").ShouldBe("File21 Content");
            storage.ReadTextFromFile(baseFolder + "\\Folder3\\Folder111\\Folder222\\File22").ShouldBe("File22 Content");
        }



        private FileStorage CreateDefaultFilesStructure()
        {
            var storage = new FileStorage();
            storage.CreateDirectory(baseFolder + "\\Folder1");
            storage.CreateDirectory(baseFolder + "\\Folder2");
            storage.CreateDirectory(baseFolder + "\\Folder3");
            storage.WriteTextToFile(baseFolder + "\\File1", "File1 Content");
            storage.WriteTextToFile(baseFolder + "\\File2", "File2 Content");
            storage.WriteTextToFile(baseFolder + "\\File3", "File3 Line1\nFile3 Line2");
            storage.WriteTextToFile(baseFolder + "\\Folder1\\File11", "File11 Content");
            storage.WriteTextToFile(baseFolder + "\\Folder2\\File21", "File21 Content");
            storage.WriteTextToFile(baseFolder + "\\Folder2\\File22", "File22 Content");
            storage.WriteTextToFile(baseFolder + "\\Folder2\\File23", "File23 Content");
            storage.WriteTextToFile(baseFolder + "\\Folder3\\File31", "File31 Content");
            return storage;
        }

    }
}
