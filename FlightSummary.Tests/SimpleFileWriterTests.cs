using System.IO;
using System.Net;
using Core.Writers;

namespace FlightSummary.Tests
{
    using Moq;
    using NUnit.Framework;
    using Core.Interfaces;

    [TestFixture]
    public class SimpleFileWriterTests
    {
        // target path should not be empty
        // target directory should exists
        // create an empty file if summary is null
        // file output should be same as expected
        [Test]
        public void ShouldReturnFalseIfTargetPathIsEmpty()
        {
            var writtenToFile = WriteSummaryToFile("","");
            Assert.IsFalse(writtenToFile);
        }

        [Test]
        public void ShouldReturnFalseIfTargetDirectoryNotExists()
        {
            var targetPath = Path.Combine(Path.GetTempPath(), System.DateTime.Now.ToShortDateString());
            var writtenToFile = WriteSummaryToFile(targetPath,"");
            Assert.IsFalse(writtenToFile);
        }

        [Test]
        public void CreateAnEmptyFileIfSummaryIsNull()
        {
            var targetPath = Path.Combine( Path.GetTempPath(), $"{Path.GetRandomFileName()}.txt");
            var writtenToFile = WriteSummaryToFile(targetPath, null);
            Assert.IsTrue(File.Exists(targetPath));
        }

        [Test]
        public void FileContentIsNotNullWhenSummaryIsNotNull()
        {
            var targetPath = Path.Combine(Path.GetTempPath(), $"{Path.GetRandomFileName()}.txt");
            var writtenToFile = WriteSummaryToFile(targetPath, "test");
            var fileContent = File.ReadAllText(targetPath);
            Assert.True(writtenToFile);
            Assert.IsFalse(string.IsNullOrEmpty(fileContent));
        }

        [Test]
        public void FileContentShouldBeSameAsExpected()
        {
            var targetPath = Path.Combine(Path.GetTempPath(), $"{Path.GetRandomFileName()}.txt");
            var writtenToFile = WriteSummaryToFile(targetPath, "12 8 3 1 12 12 445 322 123 TRUE");

            // read file content
            var fileContent = File.ReadAllText(targetPath);
            var expectedOutput = "12 8 3 1 12 12 445 322 123 TRUE";
            Assert.That(expectedOutput,Is.EqualTo(fileContent));
        }

        private static bool WriteSummaryToFile(string targetPath,string strContent)
        {
            var fileWriter = new SimpleFileWriter();
            bool writtenToFile = fileWriter.SaveSummary(targetPath, strContent);
            return writtenToFile;
        }

        
    }

    
}
