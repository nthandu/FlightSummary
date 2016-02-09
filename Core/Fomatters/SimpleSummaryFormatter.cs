namespace Core.Fomatters
{
    using Interfaces;

    public class SimpleSummaryFormatter : ISummaryFormatter
    {
        public string GetFormattedSummary(ISummary summary, char seperator)
        {
            var outPutString = string.Empty;
            if (summary != null)
            {
                // get output
                outPutString = summary.TotalNumberOfPassengers.ToString() + seperator
                               + summary.NumberOfGeneralPassengers + seperator
                               + summary.NumberOfAirlinePassengers + seperator
                               + summary.NumberOfLoyaltyPassengers + seperator
                               + summary.NumberOfBags + seperator
                               + summary.LoyaltyPointsRedeemed + seperator
                               + summary.TotalCostOfFlight + seperator
                               + summary.UnadjustedTicketRevenue + seperator
                               + summary.AdjustedTicketRevenue + seperator
                               + (summary.CanFlightProceed?.ToString().ToUpperInvariant() ?? "FALSE");
            }
            return outPutString;
        }
    }
}
