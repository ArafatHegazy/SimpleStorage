using System.Collections.Generic;
using System.IO;

namespace AH.SimpleStorage
{
    /// <summary>
    /// Interface for the basic supported operations for any storge implementation.
    /// </summary>
    public interface IStorage
    {
        /// <summary>
        /// Creates a new directory
        /// </summary>
        /// <param name="directoryName">The full path of the directory</param>
        IStorage CreateDirectory(string directoryName);

        /// <summary>
        /// Geta list of file names (with full path) for a specific direcotry.
        /// </summary>
        /// <param name="directoryName">The direcotyr name including the files.</param>
        /// <returns>List of full file names</returns>
        List<string> GetFiles(string directoryName);

        /// <summary>
        /// Geta list of directories names (with full path) for a specific direcotry.
        /// </summary>
        /// <param name="directoryName">The direcotyr name including the direcories.</param>
        /// <returns>List of full direcotyr names</returns>
        List<string> GetDirectories(string directoryName);

        /// <summary>
        /// Read file content and return it as one string
        /// </summary>
        /// <param name="fileName">The file name contains the full path</param>
        /// <returns>The file content as string</returns>
        string ReadTextFromFile(string fileName);

        /// <summary>
        /// Writes a string to a file.
        /// </summary>
        /// <param name="fileName">The taregt file name</param>
        /// <param name="content">The text fontent</param>
        IStorage WriteTextToFile(string fileName, string content);

        /// <summary>
        /// Returns a stream from a file.
        /// </summary>
        /// <param name="fileName">The file name contains the full path</param>
        /// <returns>Stream from file</returns>
        StreamReader ReadStreamFromFile(string fileName);

        /// <summary>
        /// Returns a stream from a file.
        /// </summary>
        /// <param name="fileName">The file name contains the full path</param>
        /// <returns>Stream from file</returns>
        StreamWriter WriteStreamFromFile(string fileName);

        /// <summary>
        /// Delete file
        /// </summary>
        /// <param name="fileName">The file name to be deleted</param>
        /// <returns></returns>
        IStorage DeleteFile(string fileName);

        /// <summary>
        /// Delete direcotry recursivly
        /// </summary>
        /// <param name="directoryName">Direcotry name to be deleted</param>
        /// <returns></returns>
        IStorage DeleteDirectory(string directoryName);

        /// <summary>
        /// Rename File. If the path is different, the file will be moved.
        /// </summary>
        /// <param name="fileName">Old file name</param>
        /// <param name="newFileName">New file name</param>
        /// <returns></returns>
        IStorage RenameFile(string fileName, string newFileName);

        /// <summary>
        /// Rename directory. If the path is different, the directory will be moved.
        /// </summary>
        /// <param name="directoryName">Old direcotry name</param>
        /// <param name="newDirectoryName">New direcotry name</param>
        /// <returns></returns>
        IStorage RenameDirectory(string directoryName, string newDirectoryName);

        /// <summary>
        /// Move a file from a location to another location
        /// </summary>
        /// <param name="fileName">Old file name and location</param>
        /// <param name="newFileName">New file name and location</param>
        /// <returns></returns>
        IStorage MoveFile(string fileName, string newFileName);

        /// <summary>
        /// Move a direcotry from a location to another location
        /// </summary>
        /// <param name="directoryName">Old direcotry location</param>
        /// <param name="newDirectoryName">New directory location</param>
        /// <returns></returns>
        IStorage MoveDirectory(string directoryName, string newDirectoryName);
    }
}
