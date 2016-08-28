using System.Collections.Generic;

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
        void CreateDirectory(string directoryName);

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
        void WriteTextToFile(string fileName, string content);
    }
}
