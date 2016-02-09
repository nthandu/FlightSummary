using Core.Readers;

namespace FlightSummary.Tests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using NUnit.Framework;

    [TestFixture]
    public class SimpleFileReaderTests
    {

        [Test]
        public void TestShouldThrowFileNotFoundExceptionIfFileNotExists()
        {
            var fileReader = new SimpleFileReader();
            string inputFilePath = Path.Combine("TestData",DateTime.Now.ToShortDateString());
            Assert.Throws(typeof(FileNotFoundException),()=>fileReader.Read(inputFilePath));
        }

        [Test]
        public void TestShouldReadFileAndReturnAllLines()
        {
            var fileReader = new SimpleFileReader();
            string inputFilePath = Path.Combine(TestContext.CurrentContext.TestDirectory,"TestData\\testinput.txt");
            IEnumerable<string> inputLines = fileReader.Read(inputFilePath);
            Assert.AreEqual(5,inputLines.Count());
        }
    }
}