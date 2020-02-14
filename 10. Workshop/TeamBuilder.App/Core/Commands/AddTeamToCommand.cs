namespace TeamBuilder.App.Core.Commands
{
    using System;
    using System.Linq;
    using TeamBuilder.App.Core.Commands.Contracts;
    using TeamBuilder.App.Utilities;
    using TeamBuilder.Data;
    using TeamBuilder.Models;

    public class AddTeamToCommand : ICommand
    {
        public string Execute(string[] inputArgs)
        {
            //•	AddTeamTo <eventName> <teamName>

            Check.CheckLength(2, inputArgs);

            AuthenticationManager.Authorize();

            string eventName = inputArgs[0];
            string teamName = inputArgs[1];

            using (var context = new TeamBuilderContext())
            {
                var currentEvent = context.Events.Where(e => e.Name == eventName).OrderByDescending(e => e.StartDate.Date)
                    .FirstOrDefault();

                if (currentEvent == null)
                {
                    throw new ArgumentException(Constants.ErrorMessages.EventNotFound, eventName);
                }

                var team = context.Teams.FirstOrDefault(t => t.Name == teamName);

                if (team == null)
                {
                    throw new ArgumentException(Constants.ErrorMessages.TeamNotFound, teamName);
                }

                var currentUser = AuthenticationManager.GetCurrentUser();

                if (currentUser.Id != currentEvent.CreatorId)
                {
                    throw new InvalidOperationException(Constants.ErrorMessages.NotAllowed);
                }

                if (context.EventTeams.Any(et => et.EventId == currentEvent.Id && et.TeamId == team.Id))
                {
                    throw new InvalidOperationException(Constants.ErrorMessages.CannotAddSameTeamTwice);
                }

                context.EventTeams.Add(new TeamEvent
                {
                    TeamId = team.Id,
                    Team = team,
                    EventId = currentEvent.Id,
                    Event = currentEvent
                });

                context.SaveChanges();
            }

            return $"Team {teamName} added for {eventName}!";
        }
    }
}
