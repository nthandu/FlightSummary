namespace Core.Writers
{
    using System.IO;

    public class SimpleFileWriter
    {
        // TODO: Should be using System.IO.Abstractions 
        // Enables unit testing 

        // TODO : extract interface - IWriter 
        // 

        public bool SaveSummary(string filePath,string contentToWrite)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                return false;

            if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                return false;
            
            File.WriteAllText(filePath, contentToWrite);

            return true;
        }
    }
}
