namespace TeamBuilder.App.Core.Commands
{
    using System;
    using System.Globalization;
    using TeamBuilder.App.Core.Commands.Contracts;
    using TeamBuilder.App.Utilities;
    using TeamBuilder.Data;
    using TeamBuilder.Models;

    public class CreateEventCommand : ICommand
    {
        //•	CreateEvent <name> <description> <startDate> <endDate>


        public string Execute(string[] inputArgs)
        {
            Check.CheckLength(6, inputArgs);
            AuthenticationManager.Authorize();

            string date1 = inputArgs[2] + " " + inputArgs[3];
            string date2 = inputArgs[4] + " " + inputArgs[5];

            DateTime startDate;
            DateTime endDate;

            DateTime.TryParseExact(date1, Constants.DateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out startDate);

            DateTime.TryParseExact(date2, Constants.DateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out endDate);

            if (startDate == default(DateTime) || endDate == default(DateTime))
            {
                throw new ArgumentException(Constants.ErrorMessages.InvalidDateFormat, Constants.DateTimeFormat);
            }

            if ((endDate - startDate).TotalDays < 0)
            {
                throw new ArgumentException($"Start date should be before end date.");
            }

            var currentUserId = AuthenticationManager.GetCurrentUser().Id;

            using (var context = new TeamBuilderContext())
            {
                var newEvent = new Event
                {
                    Name = inputArgs[0],
                    Description = inputArgs[1],
                    StartDate = startDate,
                    EndDate = endDate,
                    CreatorId = currentUserId
                };

                context.Events.Add(newEvent);

                context.SaveChanges();
            }

            return $"Event {inputArgs[0]} was created successfully!";
        }
    }
}
