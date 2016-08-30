using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using Shouldly;
using AH.SimpleStorage.Implementations;

namespace AH.SimpleStorage.Test
{
    [TestFixture]
    class TestMemoryStorage
    {
        [Test]
        public void OneLevelTest()
        {
            var storage = CreateDefaultFilesStructure();
            storage.BaseDirectory.Children.Count.ShouldBe(6);
            storage.BaseDirectory.Children[0].ShouldBeOfType<DirectoryNode>();
            storage.BaseDirectory.Children[1].ShouldBeOfType<DirectoryNode>();
            storage.BaseDirectory.Children[2].ShouldBeOfType<DirectoryNode>();
            storage.BaseDirectory.Children[3].ShouldBeOfType<FileNode>();

            ((FileNode)storage.BaseDirectory.Children[3]).Content.ShouldBe("File1 Content");

            ((DirectoryNode)storage.BaseDirectory.Children[0]).Children.Count.ShouldBe(1);
            ((DirectoryNode)storage.BaseDirectory.Children[1]).Children.Count.ShouldBe(3);
            ((DirectoryNode)storage.BaseDirectory.Children[2]).Children.Count.ShouldBe(1);

            storage.GetDirectories("BASE_FOLDER").Count.ShouldBe(3);
            storage.GetFiles("BASE_FOLDER").Count.ShouldBe(3);

            storage.ReadTextFromFile("BASE_FOLDER\\File1").ShouldBe("File1 Content");
            storage.ReadTextFromFile("BASE_FOLDER\\File2").ShouldBe("File2 Content");
            storage.ReadTextFromFile("BASE_FOLDER\\Folder1\\File11").ShouldBe("File11 Content");
            storage.ReadTextFromFile("BASE_FOLDER\\Folder2\\File21").ShouldBe("File21 Content");
            storage.ReadTextFromFile("BASE_FOLDER\\Folder2\\File22").ShouldBe("File22 Content");
            storage.ReadTextFromFile("BASE_FOLDER\\Folder2\\File23").ShouldBe("File23 Content");
            storage.ReadTextFromFile("BASE_FOLDER\\Folder3\\File31").ShouldBe("File31 Content");
        }

        [Test]
        public void SteamReaderTest()
        {
            string content;
            var storage = CreateDefaultFilesStructure();
            using (var streamReader = storage.ReadStreamFromFile("BASE_FOLDER\\File1"))
            {
                content = streamReader.ReadToEnd();
            }
            content.ShouldBe("File1 Content");
        }

        [Test]
        public void SteamReaderMultipleLinesTest()
        {
            string content;
            var storage = CreateDefaultFilesStructure();
            using (var streamReader = storage.ReadStreamFromFile("BASE_FOLDER\\File3"))
            {
                content = streamReader.ReadToEnd();
            }
            content.ShouldBe("File3 Line1\nFile3 Line2");
        }

        [Test]
        public void SteamReaderMultipleLinesReadLineTest()
        {
            var storage = CreateDefaultFilesStructure();
            using (var streamReader = storage.ReadStreamFromFile("BASE_FOLDER\\File3"))
            {
                var lines = readStream(streamReader);
                lines.Count.ShouldBe(2);
                lines[0].ShouldBe("File3 Line1");
                lines[1].ShouldBe("File3 Line2");
            }
        }

        [Test]
        public void StreamWriterTest_Writeln()
        {
            var fileName = "BASE_FOLDER\\File4";
            var storage = CreateDefaultFilesStructure();
            using (var streamWriter = storage.WriteStreamFromFile(fileName))
            {
                streamWriter.WriteLine("File4 Line1");
                streamWriter.WriteLine("File4 Line2");
                streamWriter.Flush();
            }
            var content = storage.ReadTextFromFile(fileName);
            content.ShouldBe("File4 Line1\r\nFile4 Line2\r\n");

            var contentStream = storage.ReadStreamFromFile(fileName);
            var list = readStream(contentStream);
            list.Count.ShouldBe(2);
            list[0].ShouldBe("File4 Line1");
            list[1].ShouldBe("File4 Line2");

        }

        [Test]
        public void StreamWriterTest_Write()
        {
            var fileName = "BASE_FOLDER\\File4";
            var storage = CreateDefaultFilesStructure();
            using (var streamWriter = storage.WriteStreamFromFile(fileName))
            {
                streamWriter.Write("File4 Line1");
                streamWriter.Write("File4 Line2");
                streamWriter.Flush();
            }
            var content = storage.ReadTextFromFile(fileName);
            content.ShouldBe("File4 Line1File4 Line2");

            var contentStream = storage.ReadStreamFromFile(fileName);
            var list = readStream(contentStream);
            list.Count.ShouldBe(1);
            list[0].ShouldBe("File4 Line1File4 Line2");
        }


        private List<string> readStream(StreamReader streamReader)
        {
            List<string> list = new List<string>();
            while (!streamReader.EndOfStream)
            {
                list.Add(streamReader.ReadLine());
            }
            return list;
        }


        private static MemoryStorage CreateDefaultFilesStructure()
        {
            var storage = new MemoryStorage("BASE_FOLDER");
            storage.CreateDirectory("BASE_FOLDER\\Folder1");
            storage.CreateDirectory("BASE_FOLDER\\Folder2");
            storage.CreateDirectory("BASE_FOLDER\\Folder3");
            storage.WriteTextToFile("BASE_FOLDER\\File1", "File1 Content");
            storage.WriteTextToFile("BASE_FOLDER\\File2", "File2 Content");
            storage.WriteTextToFile("BASE_FOLDER\\File3", "File3 Line1\nFile3 Line2");
            storage.WriteTextToFile("BASE_FOLDER\\Folder1\\File11", "File11 Content");
            storage.WriteTextToFile("BASE_FOLDER\\Folder2\\File21", "File21 Content");
            storage.WriteTextToFile("BASE_FOLDER\\Folder2\\File22", "File22 Content");
            storage.WriteTextToFile("BASE_FOLDER\\Folder2\\File23", "File23 Content");
            storage.WriteTextToFile("BASE_FOLDER\\Folder3\\File31", "File31 Content");
            return storage;
        }


    }
}
