namespace FlightSummary.Tests
{
    using System.Collections.Generic;
    using NUnit.Framework;
    using Core.Interfaces;
    using Core.Processors;

    [TestFixture]
    public class AbnfFileProcessorTests
    {
        private IList<string> _inputLines = null;
        [SetUp]
        public void Setup()
        {
            _inputLines = new List<string>()
            {
                "add route London Dublin 100 150 75",
                "add aircraft Gulfstream-G550 8",
                "add airline Trevor 54",
                "add general Mark 35",
                "add loyalty Joan 56 100 FALSE TRUE"
            };
        }
        // Processor takes input lines
        // validate each line - should contain at least 4 words
        // process valid lines
        // returns summary
        [Test]
        public void ReturnsEmptySummaryIfInputIsNullOrEmpty()
        {
            var mocValidator = new Moq.Mock<IInputValidator>();
            mocValidator.Setup(val => val.IsValidInput(new List<string>(), null)).Returns(true);
            var abnfFileProcess = new AbnfContentProcessor(mocValidator.Object);
            var flightSummary = abnfFileProcess.Process(null);
            Assert.IsNotNull(flightSummary);
        }

        [Test]
        public void SummaryShouldContainTotalNumberOfPassengers()
        {
            var flightSumary = GetTestFlightSumary();
            Assert.AreEqual(3, flightSumary.TotalNumberOfPassengers);
        }

        [Test]
        public void SummaryShouldGiveGeneralPassengerCount()
        {
            var flightSumary = GetTestFlightSumary();
            Assert.AreEqual(1, flightSumary.NumberOfGeneralPassengers);
        }


        [Test]
        public void SummaryShouldGiveAirlinePassengerCount()
        {
            var flightSumary = GetTestFlightSumary();
            Assert.AreEqual(1, flightSumary.NumberOfAirlinePassengers);
        }

        [Test]
        public void SummaryShouldGiveLoyaltyPassengerCount()
        {
            var flightSumary = GetTestFlightSumary();
            Assert.AreEqual(1, flightSumary.NumberOfLoyaltyPassengers);
        }

        [Test]
        public void SummaryShouldStateNumberOfBags()
        {
            var flightSumary = GetTestFlightSumary();
            Assert.AreEqual(4, flightSumary.NumberOfBags);
        }

        [Test]
        public void SummaryShouldStateTotalLoyaltyPointsRedeemed()
        {
            _inputLines.Add("add loyalty Naresh 32 150 TRUE TRUE");
            var flightSumary = GetTestFlightSumary();
            Assert.AreEqual(150, flightSumary.LoyaltyPointsRedeemed);
        }

        [Test]
        public void SummaryShouldStateTotalCostOfTheFlight()
        {
            var flightSumary = GetTestFlightSumary();
            Assert.AreEqual(300, flightSumary.TotalCostOfFlight);
        }

        [Test]
        public void SummaryShouldStateUnadjustedTicketRevenue()
        {
            var flightSumary = GetTestFlightSumary();
            Assert.AreEqual(450, flightSumary.UnadjustedTicketRevenue);
        }

        [Test]
        public void SummaryShouldStateAdjustedRevenue()
        {
            var flightSumary = GetTestFlightSumary();
            Assert.AreEqual(300, flightSumary.AdjustedTicketRevenue);
        }

        [Test]
        public void SummaryShouldFlightCanTakeOffOrNot()
        {
            var flightSumary = GetTestFlightSumary();
            Assert.IsNotNull(flightSumary.CanFlightProceed);
        }

        [Test]
        public void FlightShouldProceedIfAdjustedRevenueExceedsCostOfFlight()
        {
            _inputLines.Clear();
            _inputLines = new List<string>()
            {
                "add route London Dublin 100 150 75",
                "add aircraft Gulfstream-G550 8",
                "add general Tom 15",
                "add general James 72",
                "add airline Trevor 54",
                "add loyalty Alan 65 50 FALSE FALSE",
                "add loyalty Susie 21 40 TRUE FALSE",
                "add loyalty Joan 56 100 FALSE TRUE",
                "add general Jack 50"
            };
            var flightSumary = GetTestFlightSumary();
            Assert.IsTrue(flightSumary.CanFlightProceed);
        }

        [Test]
        public void FlightShouldNotProceedIfNotReachedTargetOccupancy()
        {
            _inputLines.Clear();
            _inputLines = new List<string>()
            {
                "add route London Dublin 100 150 75",
                "add aircraft Gulfstream-G550 12",
                "add general Tom 15",
                "add general James 72",
                "add airline Trevor 54",
                "add loyalty Alan 65",
                "add loyalty Susie 21",
                "add loyalty Joan 56"
            };
            var flightSumary = GetTestFlightSumary();
            Assert.IsFalse(flightSumary.CanFlightProceed);
        }

        [Test]
        public void FlightShouldNotProceedIfNumberOfPassengersExceedsNumberOfSeatsOnPlane()
        {
            _inputLines.Clear();
            _inputLines = new List<string>()
            {
                "add route London Dublin 100 150 75",
                "add aircraft Gulfstream-G550 4",
                "add general Tom 15",
                "add general James 72",
                "add airline Trevor 54",
                "add loyalty Alan 65",
                "add loyalty Susie 21",
                "add loyalty Joan 56"
            };
            var flightSumary = GetTestFlightSumary();
            Assert.IsFalse(flightSumary.CanFlightProceed);
        }
        private ISummary GetTestFlightSumary()
        {
            var mocValidator = new Moq.Mock<IInputValidator>();
            mocValidator.Setup(val => val.IsValidInput(_inputLines, null)).Returns(true);
            var abnfFileProcess = new AbnfContentProcessor(mocValidator.Object);
            return abnfFileProcess.Process(_inputLines);
        }
    }
}



