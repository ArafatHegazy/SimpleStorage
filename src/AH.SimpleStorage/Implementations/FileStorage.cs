using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AH.SimpleStorage.Implementations
{
    public class FileStorage : IStorage
    {
        public IStorage CreateDirectory(string directoryName)
        {
            bool exists = Directory.Exists(directoryName);

            if (!exists)
            {
                Directory.CreateDirectory(directoryName);
            }
            return this;
        }

        public List<string> GetFiles(string directoryName)
        {
            return Directory.GetFiles(directoryName).ToList();
        }

        public List<string> GetDirectories(string directoryName)
        {
            return Directory.GetDirectories(directoryName).ToList();
        }

        public string ReadTextFromFile(string fileName)
        {
            return File.ReadAllText(fileName);
        }

        public IStorage WriteTextToFile(string fileName, string content)
        {
            File.WriteAllText(fileName, content);
            return this;
        }

        public StreamReader ReadStreamFromFile(string fileName)
        {
            StreamReader stream = new StreamReader(fileName);
            return stream;
        }

        public StreamWriter WriteStreamFromFile(string fileName)
        {
            return new StreamWriter(fileName);
        }

        public IStorage DeleteFile(string fileName)
        {
            File.Delete(fileName);
            return this;
        }

        public IStorage DeleteDirectory(string directoryName)
        {
            Directory.Delete(directoryName, true);
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
            File.Move(fileName, newFileName);
            return this;
        }

        public IStorage MoveDirectory(string directoryName, string newDirectoryName)
        {
            Directory.Move(directoryName, newDirectoryName);
            return this;
        }
    }
}
