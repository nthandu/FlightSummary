namespace Core.Models
{
    using Interfaces;

    public class Summary : ISummary
    {
        public bool InvalidInput { get; set; }
        public int TotalNumberOfPassengers { get; set; }
        public int NumberOfGeneralPassengers { get; set; }
        public int NumberOfAirlinePassengers { get; set; }
        public int NumberOfLoyaltyPassengers { get; set; }
        public int NumberOfBags { get; set; }
        public int LoyaltyPointsRedeemed { get; set; }
        public double TotalCostOfFlight { get; set; }
        public double UnadjustedTicketRevenue { get; set; }
        public double AdjustedTicketRevenue { get; set; }
        public bool? CanFlightProceed { get; set; }
    }

}
