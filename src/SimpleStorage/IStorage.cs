using System.Collections.Generic;

namespace SimpleStorage
{
    public interface IStorage
    {
        void CreateDirectory(string directoryName);
        List<string> GetFiles(string directoryName);
        List<string> GetDirectories(string directoryName);
        string ReadFromFile(string fileName);
        void WriteToFile(string fileName, string content);
    }
}
