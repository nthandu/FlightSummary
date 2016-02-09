namespace Core.Processors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Models;
    using Interfaces;

    public class AbnfContentProcessor : IContentProcessor
    {
        private readonly IInputValidator _inputrValidator;
        
        public AbnfContentProcessor(IInputValidator validator)
        {
            // TODO: Inject summary provider/generator along with validator
            // generating summary can be moved to summary provider and invoke summary provider from Process method.
            _inputrValidator = validator;
        }

        public ISummary Process(IEnumerable<string> inputLines)
        {
            var summary = new Summary();

            if (inputLines == null)
                return summary;

            var listInputLines = inputLines.ToList();

            if (listInputLines.Count == 0)
                return summary;

            var validInput = _inputrValidator.IsValidInput(listInputLines, null);

            if (!validInput)
            {
                summary.InvalidInput = true;
                return summary;
            }

            double costPerPassenger = 0;
            double ticketPrice = 0;
            double targetOccupancy = 0d;
            int totalSeats = 0;
            // TODO: following code should be moved to another class and should be injected through constructor
            foreach (var elements in listInputLines.Where(item => !string.IsNullOrWhiteSpace(item)).Select(item => item.Split(null)))
            {
                // all input lines are validated. 
                // element at index 0 is action, index 1 is type of action

                var instructionType = elements.ElementAt(1);

                if (string.Equals(Constants.Route, instructionType, StringComparison.InvariantCultureIgnoreCase))
                {
                    //TODO: Following should be moved to another method and return Tuple
                    double.TryParse(elements.ElementAt(4).Trim(), out costPerPassenger);
                    double.TryParse(elements.ElementAt(5).Trim(), out ticketPrice);
                    double.TryParse(elements.ElementAt(6).Trim(), out targetOccupancy);
                    continue;
                }
                if (string.Equals(Constants.Aircraft, instructionType, StringComparison.InvariantCultureIgnoreCase))
                {
                    int.TryParse(elements.ElementAt(3).Trim(), out totalSeats);
                    continue;
                }
                if (string.Equals(Constants.Loyalty, instructionType, StringComparison.InvariantCultureIgnoreCase))
                {
                    summary.NumberOfLoyaltyPassengers++;
                    ParseExtraBaggage(elements, summary);
                    ParseLoyaltyPoints(elements, summary);
                    summary.NumberOfBags++;
                    continue;
                }

                if (string.Equals(Constants.Airline, instructionType, StringComparison.InvariantCultureIgnoreCase))
                {
                    summary.NumberOfAirlinePassengers++;
                    summary.NumberOfBags++;
                    continue;
                }
                // each passenger will carry at least bag
                summary.NumberOfBags++;
                summary.NumberOfGeneralPassengers++;
            }

            summary.TotalNumberOfPassengers = summary.NumberOfGeneralPassengers + summary.NumberOfAirlinePassengers +
                                              summary.NumberOfLoyaltyPassengers;

            summary.TotalCostOfFlight = summary.TotalNumberOfPassengers * costPerPassenger;
            summary.UnadjustedTicketRevenue = summary.TotalNumberOfPassengers * ticketPrice;
            summary.AdjustedTicketRevenue = summary.UnadjustedTicketRevenue - (summary.LoyaltyPointsRedeemed + summary.NumberOfAirlinePassengers * ticketPrice);
            var actualOccupancy = 100d; // assuming actual occupancy 100%
            if (totalSeats > 0)
                actualOccupancy = ((double)summary.TotalNumberOfPassengers / totalSeats) * 100;
            summary.CanFlightProceed = (summary.AdjustedTicketRevenue > summary.TotalCostOfFlight)
                && (actualOccupancy > targetOccupancy)
                && (totalSeats >= summary.TotalNumberOfPassengers);
            return summary;
        }

        private static void ParseExtraBaggage(string[] elements, Summary summary)
        {
            if (elements.Length > 6)
            {
                // loyalty passenger allowed to carry an extra bag;
                var usingExtraBaggage = elements.ElementAt(6);
                if (usingExtraBaggage != null &&
                    string.Equals("TRUE", usingExtraBaggage.Trim(), StringComparison.CurrentCultureIgnoreCase))
                {
                    // passenger bringing extra baggage
                    summary.NumberOfBags++;
                }
            }
        }

        private static void ParseLoyaltyPoints(string[] elements, Summary summary)
        {
            if (elements.Length > 4)
            {
                // if not provided the value for whether using loyalty points, it would be treated as false
                // and if it's false, no need to parse the value of number of loyalty points
                var strLoyaltyPoints = elements.ElementAt(4);
                var usingLoyaltyPoints = elements.ElementAt(5);
                if (usingLoyaltyPoints != null &&
                    string.Equals("TRUE", usingLoyaltyPoints.Trim(), StringComparison.CurrentCultureIgnoreCase))
                {
                    var loyaltyPoints = ParseStringToNumber(strLoyaltyPoints);
                    // passenger bringing extra baggage
                    summary.LoyaltyPointsRedeemed += loyaltyPoints;
                }
            }
        }

        private static int ParseStringToNumber(string strLoyaltyPoints)
        {
            if (string.IsNullOrWhiteSpace(strLoyaltyPoints))
                return 0;

            var loyaltyPoints = 0;

            int.TryParse(strLoyaltyPoints.Trim(), out loyaltyPoints);

            return loyaltyPoints;
        }
    }
}
