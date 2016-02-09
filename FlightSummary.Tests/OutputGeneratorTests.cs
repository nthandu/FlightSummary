namespace FlightSummary.Tests
{
    using Core.Interfaces;
    using Core.Output;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class AbnfOutputGeneratorTests
    {
        // takes summary object
        // returns output
        // returns empty string if summary is null or is invalid
        [Test]
        public void ReturnsEmptyTextIfSummaryIsNull()
        {
            var mockSummaryFormatter = new Mock<ISummaryFormatter>().Object;
            var abnfOutPutProducer = new AbnfOutputProducer(mockSummaryFormatter);
            var outPut = abnfOutPutProducer.GetSummaryAsText(null);
            Assert.IsTrue(string.IsNullOrEmpty(outPut));
        }

        [Test]
        public void ReturnsEmptyTextIfSummaryIsInvalid()
        {
            var mockSummry = new Mock<ISummary>();
            mockSummry.SetupGet(x => x.InvalidInput).Returns(true);
            var mockSummaryFormatter = new Mock<ISummaryFormatter>().Object;
            var abnfOutPutProducer = new AbnfOutputProducer(mockSummaryFormatter);
            var outPut = abnfOutPutProducer.GetSummaryAsText(mockSummry.Object);
            Assert.IsTrue(string.IsNullOrEmpty(outPut));
        }

        [Test]
        public void ReturnsOutputAsExpectedWithValidInput()
        {
            var mockSummry = GetMocSummary();
            var mockSummaryFormatter = new Mock<ISummaryFormatter>();
            mockSummaryFormatter.Setup(sum => sum.GetFormattedSummary(mockSummry.Object,' ')).Returns("12 8 3 1 12 12 445 322 123 TRUE");
            var abnfOutPutProducer = new AbnfOutputProducer(mockSummaryFormatter.Object);
            var outPut = abnfOutPutProducer.GetSummaryAsText(mockSummry.Object);
            var expectedOutput = "12 8 3 1 12 12 445 322 123 TRUE";
            Assert.That(expectedOutput, Is.EqualTo(outPut));
        }

        private static Mock<ISummary> GetMocSummary()
        {
            var mocSummary = new Moq.Mock<ISummary>();
            mocSummary.SetupAllProperties();
            mocSummary.Object.TotalNumberOfPassengers = 12;
            mocSummary.Object.AdjustedTicketRevenue = 123;
            mocSummary.Object.CanFlightProceed = true;
            mocSummary.Object.InvalidInput = false;
            mocSummary.Object.LoyaltyPointsRedeemed = 12;
            mocSummary.Object.NumberOfAirlinePassengers = 3;
            mocSummary.Object.NumberOfBags = 12;
            mocSummary.Object.NumberOfGeneralPassengers = 8;
            mocSummary.Object.NumberOfLoyaltyPassengers = 1;
            mocSummary.Object.TotalCostOfFlight = 445;
            mocSummary.Object.UnadjustedTicketRevenue = 322;
            return mocSummary;
        }
    }

    

    

    
}
