using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

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

        public IStorage CreateDirectory(string directoryName)
        {
            var folder = BaseDirectory.GetDirecotry(directoryName);
            if (folder == null)
            {
                BaseDirectory.CreateDirectory(directoryName);
            }
            return this;
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

        public IStorage WriteTextToFile(string fileName, string content)
        {
            var file = BaseDirectory.GetFile(fileName);
            if (file == null)
            {
                file = BaseDirectory.CreateFile(fileName);
            }
            file.Content = content;
            return this;
        }

        public StreamReader ReadStreamFromFile(string fileName)
        {
            var fileContent = ReadTextFromFile(fileName);
            MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(fileContent ?? ""));
            StreamReader reader = new StreamReader(stream);
            return reader;
        }

        public StreamWriter WriteStreamFromFile(string fileName)
        {
            var file = BaseDirectory.GetFile(fileName);
            if (file == null)
            {
                file = BaseDirectory.CreateFile(fileName);
            }
            return new MemoryStreamWriter(file);
        }

        public IStorage DeleteFile(string fileName)
        {
            var parentDirectory = BaseDirectory.GetParentDirectory(fileName);
            var childName = BaseDirectory.RemovePath(fileName);
            parentDirectory.RemoveChild(childName);
            return this;
        }

        public IStorage DeleteDirectory(string directoryName)
        {
            var parentDirectory = BaseDirectory.GetParentDirectory(directoryName);
            var child = BaseDirectory.RemovePath(directoryName);
            parentDirectory.RemoveChild(child);
            return this;
        }

        public IStorage RenameFile(string fileName, string newFileName)
        {
            return MoveFile(fileName, newFileName);
        }

        public IStorage RenameDirectory(string directoryName, string newDirectoryName)
        {
            return MoveDirectory(directoryName, newDirectoryName);
        }

        public IStorage MoveFile(string fileName, string newFileName)
        {
            var content = ReadTextFromFile(fileName);
            DeleteFile(fileName);
            WriteTextToFile(newFileName, content);
            return this;
        }

        public IStorage MoveDirectory(string directoryName, string newDirectoryName)
        {
            var parentDirectory = BaseDirectory.GetParentDirectory(directoryName);
            var childName = BaseDirectory.RemovePath(directoryName);
            var child = parentDirectory.RemoveChild(childName);
            child.Name = BaseDirectory.RemovePath(newDirectoryName);
            var destinationParentDirecotry = BaseDirectory.GetParentDirectory(newDirectoryName);
            destinationParentDirecotry.Children.Add(child);
            ((DirectoryNode)child).FixPath(destinationParentDirecotry.FullPath);
            return this;
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

        public void FixPath(string parentPath)
        {
            FullPath = parentPath + "\\" + Name;
            Children.ForEach(c =>
            {
                if (c is FileNode)
                    c.FullPath = parentPath + "\\" + c.Name;
                else
                    ((DirectoryNode)c).FixPath(FullPath);
            });
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
            DirectoryNode child =
                (DirectoryNode) Children.FirstOrDefault(c => c is DirectoryNode && c.Name == firstNode);
            if (child != null)
                return child.GetDirecotry(path.Skip(1).ToArray());
            return null;
        }

        public void CreateDirectory(string directoryName)
        {
            DirectoryNode parentDirecotry = GetParentDirectory(directoryName);
            parentDirecotry.CreateDirecotry(RemovePath(directoryName));
        }

        public DirectoryNode GetParentDirectory(string name)
        {
            var parentDirecotry = GetDirecotry(GetPathList(name));
            return parentDirecotry;
        }

        public string RemovePath(string fullName)
        {
            var path = fullName.Split('\\');
            return path.Last();
        }

        public string[] GetPathList(string fullName)
        {
            var path = fullName.Split('\\');
            var parentDirectoryPath = path.ToList();
            parentDirectoryPath.Remove(parentDirectoryPath.Last());
            return parentDirectoryPath.ToArray();
        }

        public void CreateDirecotry(string directoryName)
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
            return (FileNode) parentDirecotry.Children.FirstOrDefault(c => c is FileNode && c.Name == path.Last());
        }

        public FileNode CreateFile(string fileName)
        {
            var path = fileName.Split('\\');
            var parentDirectoryPath = path.ToList();
            parentDirectoryPath.Remove(parentDirectoryPath.Last());
            var parentDirecotry = GetDirecotry(parentDirectoryPath.ToArray());
            var newFile = new FileNode() {Name = path.Last(), FullPath = parentDirecotry.FullPath + "\\" + path.Last()};
            parentDirecotry.Children.Add(newFile);
            return newFile;
        }

        public Node RemoveChild(string childName)
        {
            var element = GetChild(childName);
            Children.Remove(element);
            return element;
        }

        public Node GetChild(string childName)
        {
            return Children.First(c => c.Name == childName);
        }
    }
}
