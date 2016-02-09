namespace Core.Readers
{
    using System.Collections.Generic;
    using System.IO;
    using Interfaces;

    public class SimpleFileReader : IReader
    {
        public IEnumerable<string> Read(string inputFilePath)
        {
            if (!File.Exists(inputFilePath))
                throw new FileNotFoundException("File not found", inputFilePath);
            return System.IO.File.ReadLines(inputFilePath);
        }
    }
}
