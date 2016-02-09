using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Fomatters;
using Core.Interfaces;
using Moq;
using NUnit.Framework;

namespace FlightSummary.Tests
{
    [TestFixture]
    public class FormatterTests
    {
        [Test]
        public void ReturnsEmptyStringIfSummaryIsNull()
        {
            var formatter = new SimpleSummaryFormatter();
            var outPut = formatter.GetFormattedSummary(null, ',');
            Assert.IsTrue(string.IsNullOrEmpty(outPut));
        }

        [Test]
        public void ReturnsCommaSeperatedOutputWhenPassInComma()
        {
            var formatter = new SimpleSummaryFormatter();
            var outPut = formatter.GetFormattedSummary(GetMocSummary().Object, ',');
            var expectedOutput = "12,8,3,1,12,12,445,322,123,TRUE";
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
