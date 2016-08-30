using System.IO;
using System.Text;

namespace AH.SimpleStorage.Implementations
{
    public class MemoryStreamWriter : StreamWriter
    {
        public MemoryStreamWriter(string path) : base(path)
        {
        }

        public MemoryStreamWriter(Stream stream) : base(stream)
        {

        }

        protected FileNode FileNode { get; set; }
        public MemoryStreamWriter(FileNode fileNode) : base(new MemoryStream())
        {
            FileNode = fileNode;
        }

        public MemoryStreamWriter(string path, bool append) : base(path, append)
        {
        }

        public MemoryStreamWriter(Stream stream, Encoding encoding) : base(stream, encoding)
        {
        }

        public MemoryStreamWriter(string path, bool append, Encoding encoding) : base(path, append, encoding)
        {
        }

        public MemoryStreamWriter(Stream stream, Encoding encoding, int bufferSize) : base(stream, encoding, bufferSize)
        {
        }

        public MemoryStreamWriter(string path, bool append, Encoding encoding, int bufferSize) : base(path, append, encoding, bufferSize)
        {
        }

        public MemoryStreamWriter(Stream stream, Encoding encoding, int bufferSize, bool leaveOpen) : base(stream, encoding, bufferSize, leaveOpen)
        {
        }

        public override void Flush()
        {
            base.Flush();
            var bs = (MemoryStream) BaseStream;
            string result = Encoding.UTF8.GetString(bs.ToArray(), 0, (int)bs.Length);
            FileNode.Content = result;
        }

    }
}
