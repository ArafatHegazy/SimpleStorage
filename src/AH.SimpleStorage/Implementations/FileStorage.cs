using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AH.SimpleStorage.Implementations
{
    public class FileStorage : IStorage
    {
        public void CreateDirectory(string directoryName)
        {
            bool exists = Directory.Exists(directoryName);

            if (!exists)
            {
                Directory.CreateDirectory(directoryName);
            }
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

        public void WriteTextToFile(string fileName, string content)
        {
            File.WriteAllText(fileName, content);
        }
    }
}
