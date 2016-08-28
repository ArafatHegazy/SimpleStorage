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
            var storage = new MemoryStorage("BASE_FOLDER");
            storage.CreateDirectory("BASE_FOLDER\\Folder1");
            storage.CreateDirectory("BASE_FOLDER\\Folder2");
            storage.CreateDirectory("BASE_FOLDER\\Folder3");
            storage.WriteTextToFile("BASE_FOLDER\\File1", "File1 Content");
            storage.WriteTextToFile("BASE_FOLDER\\File2", "File2 Content");
            storage.WriteTextToFile("BASE_FOLDER\\File3", "File3 Content");
            storage.WriteTextToFile("BASE_FOLDER\\Folder1\\File11", "File11 Content");
            storage.WriteTextToFile("BASE_FOLDER\\Folder2\\File21", "File21 Content");
            storage.WriteTextToFile("BASE_FOLDER\\Folder2\\File22", "File22 Content");
            storage.WriteTextToFile("BASE_FOLDER\\Folder2\\File23", "File23 Content");
            storage.WriteTextToFile("BASE_FOLDER\\Folder3\\File31", "File31 Content");
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
    }
}
