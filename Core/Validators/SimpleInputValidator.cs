namespace Core.Validators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Interfaces;

    public class SimpleInputValidator : IInputValidator
    {
        

        public bool IsValidInput(IEnumerable<string> inputLines, char[] seperator)
        {
            var hasRoute = false;
            var hasAircraft = false;
            foreach (var elements in inputLines.Where(item => !string.IsNullOrEmpty(item)).Select(inputLine => inputLine.Split(seperator)))
            {
                if (elements.Count() < 4)
                    return false;

                var firstElement = elements.First();  // action element - should be add

                if (!string.Equals(firstElement, Constants.Add, StringComparison.CurrentCultureIgnoreCase))
                {
                    return false;
                }

                var inputType = elements.ElementAt(1); // second element - type of instruction

                if (string.Equals(inputType, Constants.Route, StringComparison.CurrentCultureIgnoreCase))
                {
                    if (!IsValidRoute(elements)) return false;

                    if (hasRoute)
                        return false;
                    hasRoute = true;
                    continue; // proceed with next instruction line
                }

                if (string.Equals(inputType, Constants.Aircraft, StringComparison.CurrentCultureIgnoreCase))
                {
                    if (!IsValidAircraft(elements)) return false;
                    if (hasAircraft)
                        return false;
                    hasAircraft = true;
                    continue; // proceed with next instruction line
                }

                // reaching here means instruction is not aircraft or route
                // if it's loyality - passenger will be treated as loyality customers
                // if it's airline - passenger will be treated as employee of airline
                // any other value will be treated as general passenger

                // any passenger should have name

                var nameOfPassenger = elements.ElementAt(2);

                if (string.IsNullOrWhiteSpace(nameOfPassenger))
                    return false;

                var isValidAge = IsValidAge(elements.ElementAt(3));

                if (!isValidAge)
                    return false;
            }

            if (!hasRoute)
                return false;

            if (!hasAircraft)
                return false;

            return true;
        }

        private bool IsValidAircraft(string[] elements)
        {
            // we are already checking for minimum length 4 for all lines. 
            // element at 3rd index is title. it should not be null or empty.
            var title = elements.ElementAt(2);
            if (string.IsNullOrWhiteSpace(title))
                return false;

            var isValidNumberOfSeats = IsValidNumber(elements.ElementAt(3));
            if (!isValidNumberOfSeats)
                return false;
            return true;
        }

        private bool IsValidRoute(string[] elements)
        {
            if (elements.Count() < 7)
                return false;

            // origin should not be empty
            // origin is 3rd element
            if (string.IsNullOrWhiteSpace(elements.ElementAt(2)))
                return false;

            // destination should not be empty
            // destination is 4th element
            if (string.IsNullOrWhiteSpace(elements.ElementAt(3)))
                return false;

            // cost per passenger should be valid number
            // cost per passenger is 4th element
            var validCostPerPassenger = IsValidNumber(elements.ElementAt(4));
            if (!validCostPerPassenger)
                return false;

            // ticket price should be valid number
            // ticket price is 5th element
            var validTicketPrice = IsValidNumber(elements.ElementAt(5));
            if (!validTicketPrice)
                return false;

            // minimum takeoff load should be valid number
            // min takeoff load is 6th element
            var minTakeOffLoad = IsValidNumber(elements.ElementAt(6));
            if (!minTakeOffLoad)
                return false;
            return true;
        }

        private bool IsValidNumber(string strNumber)
        {
            if (string.IsNullOrWhiteSpace(strNumber))
                return false;

            double n;

            return double.TryParse(strNumber, out n);
        }

        private bool IsValidAge(string strNumber)
        {
            if (string.IsNullOrWhiteSpace(strNumber))
                return false;

            double n;

            bool isValid = double.TryParse(strNumber, out n);
            return isValid && n >= 0;
        }
    }
}
