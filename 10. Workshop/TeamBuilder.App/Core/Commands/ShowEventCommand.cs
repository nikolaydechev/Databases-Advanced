namespace TeamBuilder.App.Core.Commands
{
    using System;
    using System.Linq;
    using System.Text;
    using Microsoft.EntityFrameworkCore;
    using TeamBuilder.App.Core.Commands.Contracts;
    using TeamBuilder.App.Utilities;
    using TeamBuilder.Data;

    public class ShowEventCommand : ICommand
    {
        public string Execute(string[] inputArgs)
        {
            //•	ShowEvent<eventName>
            Check.CheckLength(1, inputArgs);

            string eventName = inputArgs[0];
            var sb = new StringBuilder();

            using (var context = new TeamBuilderContext())
            {
                var currentEvent = context.Events.FirstOrDefault(e => e.Name == eventName);

                if (currentEvent == null)
                {
                    throw new ArgumentException(Constants.ErrorMessages.EventNotFound, eventName);
                }

                sb.AppendLine($"{eventName} {currentEvent.StartDate} {currentEvent.EndDate}");
                var description = currentEvent.Description ?? "[no description]";
                sb.AppendLine(description);
                sb.AppendLine("Teams:");

                var teams = context.EventTeams
                    .Include(e => e.Team)
                    .Include(e => e.Event)
                    .Where(e => e.EventId == currentEvent.Id)
                    .Select(x => x.Team.Name)
                    .ToArray();

                foreach (var team in teams)
                {
                    sb.AppendLine($"-{team}");
                }
            }

            return sb.ToString().Trim();
        }
    }
}
