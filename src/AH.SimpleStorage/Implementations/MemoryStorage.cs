using System.Collections.Generic;
using System.Linq;

namespace AH.SimpleStorage.Implementations
{
    /// <summary>
    /// An implementation of the IStorage that provides memory file operations. It is very useful in unit testins.
    /// </summary>
    public class MemoryStorage : IStorage
    {
        protected DirectoryNode _baseDirectory = new DirectoryNode();

        public DirectoryNode BaseDirectory => _baseDirectory;

        public MemoryStorage(string baseFolderName)
        {
            BaseDirectory.Name = baseFolderName;
            BaseDirectory.FullPath = baseFolderName;
        }

        public void CreateDirectory(string directoryName)
        {
            var folder = BaseDirectory.GetDirecotry(directoryName);
            if (folder == null)
            {
                BaseDirectory.CreateDirectory(directoryName);
            }
        }

        public List<string> GetFiles(string directoryName)
        {
            var folder = BaseDirectory.GetDirecotry(directoryName);
            return folder.GetFiles();
        }

        public List<string> GetDirectories(string directoryName)
        {
            var folder = BaseDirectory.GetDirecotry(directoryName);
            return folder.GetDirecotries();
        }

        public string ReadTextFromFile(string fileName)
        {
            var file = BaseDirectory.GetFile(fileName);
            return file.Content;
        }

        public void WriteTextToFile(string fileName, string content)
        {
            var file = BaseDirectory.GetFile(fileName);
            if (file == null)
            {
                BaseDirectory.CreateFile(fileName);
                file = BaseDirectory.GetFile(fileName);
            }
            file.Content = content;
        }
    }

    public class Node
    {
        public string Name;
        public string FullPath;
    }

    public class FileNode : Node
    {
        public string Content;
    }

    public class DirectoryNode : Node
    {
        public List<Node> Children = new List<Node>();

        public DirectoryNode GetDirecotry(string name)
        {
            return GetDirecotry(name.Split('\\'));
        }

        public DirectoryNode GetDirecotry(string[] path)
        {
            var firstNode = path[0];
            if (firstNode == Name && path.Length == 1)
            {
                return this;
            }

            if (firstNode == Name)
            {
                firstNode = path[1];
            }
            DirectoryNode child = (DirectoryNode)Children.FirstOrDefault(c => c is DirectoryNode && c.Name == firstNode);
            if (child != null)
                return child.GetDirecotry(path.Skip(1).ToArray());
            return null;
        }

        public void CreateDirectory(string directoryName)
        {
            var path = directoryName.Split('\\');
            var parentDirectoryPath = path.ToList();
            parentDirectoryPath.Remove(parentDirectoryPath.Last());
            var parentDirecotry = GetDirecotry(parentDirectoryPath.ToArray());
            parentDirecotry.CreateDirecotry(path.Last());
        }

        private void CreateDirecotry(string directoryName)
        {
            Children.Add(new DirectoryNode() {Name = directoryName, FullPath = this.FullPath + "\\" + directoryName});
        }

        public List<string> GetFiles()
        {
            return Children.FindAll(c => c is FileNode).Select(c => c.FullPath).ToList();
        }

        public List<string> GetDirecotries()
        {
            return Children.FindAll(c => c is DirectoryNode).Select(c => c.FullPath).ToList();
        }

        public FileNode GetFile(string fileName)
        {
            var path = fileName.Split('\\');
            var parentDirectoryPath = path.ToList();
            parentDirectoryPath.Remove(parentDirectoryPath.Last());
            var parentDirecotry = GetDirecotry(parentDirectoryPath.ToArray());
            return (FileNode)parentDirecotry.Children.FirstOrDefault(c => c is FileNode && c.Name == path.Last());
        }

        public void CreateFile(string fileName)
        {
            var path = fileName.Split('\\');
            var parentDirectoryPath = path.ToList();
            parentDirectoryPath.Remove(parentDirectoryPath.Last());
            var parentDirecotry = GetDirecotry(parentDirectoryPath.ToArray());
            parentDirecotry.Children.Add(new FileNode() { Name = path.Last(), FullPath = parentDirecotry.FullPath + "\\" + path.Last() });
        }
    }


}
