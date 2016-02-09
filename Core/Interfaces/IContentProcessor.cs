namespace Core.Interfaces
{
    using System.Collections.Generic;
    /// <summary>
    /// To be implemented by content processors.
    /// </summary>
    public interface IContentProcessor
    {
        ISummary Process(IEnumerable<string> inputLines);
    }

    /// <summary>
    /// To be implemented by Readers
    /// </summary>
    public interface IReader
    {
        IEnumerable<string> Read(string inputFilePath);
    }

    /// <summary>
    /// To be implemented by content validators
    /// </summary>
    public interface IInputValidator
    {
        // TODO: Should return invalid lines - ?
        bool IsValidInput(IEnumerable<string> inputLines, char[] seperator);
    }

    public interface ISummary
    {
        bool InvalidInput { get; set; }
        int TotalNumberOfPassengers { get; set; }
        int NumberOfGeneralPassengers { get; set; }
        int NumberOfAirlinePassengers { get; set; }
        int NumberOfLoyaltyPassengers { get; set; }
        int NumberOfBags { get; set; }
        int LoyaltyPointsRedeemed { get; set; }
        double TotalCostOfFlight { get; set; }
        double UnadjustedTicketRevenue { get; set; }
        double AdjustedTicketRevenue { get; set; }
        bool? CanFlightProceed { get; set; }
    }

    public interface ISummaryFormatter
    {
        string GetFormattedSummary(ISummary summary, char seperator);
    }
}
