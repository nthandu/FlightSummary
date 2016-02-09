namespace FlightSummary.Tests
{
    using System.Collections.Generic;
    using Core.Validators;
    using NUnit.Framework;

    [TestFixture]
    public class SimpleInputValidatorTests
    {
        private IList<string> _inputLines = null;
        [SetUp]
        public void Setup()
        {
            _inputLines = new List<string>()
            {
                "add route London Dublin 100 150 75",
                "add aircraft Gulfstream-G550 8"
            };
        }

        [Test]
        public void EachLineMustHaveAtLeastFourWords()
        {
            _inputLines.Add("add airline trevor");
            AssertForInvalidInput(_inputLines);
        }

        [Test]
        public void EachLineShouldOnlyStartWithAddIfNotSummaryShouldStateInvalidStatus()
        {
            _inputLines.Add("addition add route");
            AssertForInvalidInput(_inputLines);
        }

        [Test]
        public void EntriesShouldContainOnlyOneAddRoute()
        {
            _inputLines.Add("add route London Dublin 100 150 75");
            AssertForInvalidInput(_inputLines);
        }

        [Test]
        public void EntriesShouldContainAddRoute()
        {
            _inputLines.RemoveAt(0);
            AssertForInvalidInput(_inputLines);
        }

        [Test]
        public void EntriesShouldContainOnlyOneAircraft()
        {
            _inputLines.Add("add aircraft Gulfstream-G550 8");
            AssertForInvalidInput(_inputLines);
        }


        [Test]
        public void EntriesShouldContainOneAddAircraft()
        {
            _inputLines.RemoveAt(1);
            AssertForInvalidInput(_inputLines);
        }

        [Test]
        public void AddRouteShouldContainSevenEntries()
        {
            _inputLines.RemoveAt(0);
            _inputLines.Add("add route London 100 150 75"); // only 6 entries 

            AssertForInvalidInput(_inputLines);
        }

        [Test]
        public void AddRouteShouldContainOrigin()
        {
            _inputLines.RemoveAt(0);
            _inputLines.Add("add route  London 100 150 75"); // only 6 entries 

            AssertForInvalidInput(_inputLines);
        }

        [Test]
        public void AddRouteShouldContainDestination()
        {
            _inputLines.RemoveAt(0);
            _inputLines.Add("add route London  100 150 75"); // empty space 

            AssertForInvalidInput(_inputLines);
        }

        [Test]
        public void AddRouteShouldContainValidInputForCostForPassenger()
        {
            _inputLines.RemoveAt(0);
            _inputLines.Add("add route London Dublin test 150 75"); // empty space 

            AssertForInvalidInput(_inputLines);
        }

        [Test]
        public void AddRouteShouldContainValidInputForTicketPrice()
        {
            _inputLines.RemoveAt(0);
            _inputLines.Add("add route London Dublin 120.75 ticket 75"); // empty space 
            AssertForInvalidInput(_inputLines);
        }

        [Test]
        public void AddRouteShouldContainValidMinimumTakeOffLoad()
        {
            _inputLines.RemoveAt(0);
            _inputLines.Add("add route London Dublin 120.75 12.12 load"); // empty space 
            AssertForInvalidInput(_inputLines);
        }

        [Test]
        public void AddAircraftShouldContainTitle()
        {
            _inputLines.RemoveAt(1);
            _inputLines.Add("add aircraft  8");
            AssertForInvalidInput(_inputLines);
        }

        [Test]
        public void AddAircraftShouldContainValidNumberOfSeats()
        {
            _inputLines.RemoveAt(1);
            _inputLines.Add("add aircraft test-tile test");
            AssertForInvalidInput(_inputLines);
        }

        //[Test]
        //public void PassengerShouldBeValidType()
        //{//}

        [Test]
        public void PassengerNameShouldNotBeNullOrWhiteSpace()
        {
            _inputLines.Add("add airline  54");
            AssertForInvalidInput(_inputLines);
        }

        [Test]
        public void PassengerAgeShouldBeNumber()
        {
            _inputLines.Add("add airline wonga-wonga age");
            AssertForInvalidInput(_inputLines);
        }

        [Test]
        public void PassengerAgeShouldNotBeNegative()
        {
            _inputLines.Add("add airline wonga-wonga -20");
            AssertForInvalidInput(_inputLines);
        }

        private static void AssertForInvalidInput(IEnumerable<string> inputLines)
        {
            var validator = new SimpleInputValidator();
            var isvalidInput = validator.IsValidInput(inputLines, null);
            Assert.IsFalse(isvalidInput);
        }
    }
}
