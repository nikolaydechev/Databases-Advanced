namespace TeamBuilder.App.Core.Commands
{
    using System;
    using System.Linq;
    using TeamBuilder.App.Core.Commands.Contracts;
    using TeamBuilder.App.Utilities;
    using TeamBuilder.Data;
    using TeamBuilder.Models;

    public class CreateTeamCommand : ICommand
    {
        public string Execute(string[] inputArgs)
        {
            //•	CreateTeam <name> <acronym> <description>

            //optional description
            if (inputArgs.Length != 2 && inputArgs.Length != 3)
            {
                throw new FormatException(Constants.ErrorMessages.InvalidArgumentsCount);
            }
            var currentUser = AuthenticationManager.GetCurrentUser();

            if (currentUser == null)
            {
                throw new InvalidOperationException(Constants.ErrorMessages.LoginFirst);
            }
            
            string teamName = inputArgs[0];
            string acronym = inputArgs[1];
            
            if (acronym.Length != 3)
            {
                throw new ArgumentException(Constants.ErrorMessages.InvalidAcronym, acronym);
            }
            
            using (var context = new TeamBuilderContext())
            {
                var teamExists = context.Teams.Any(t => t.Name == teamName);
                if (teamExists)
                {
                    throw new ArgumentException(Constants.ErrorMessages.TeamExists, teamName);
                }

                var team = new Team
                {
                    Name = teamName,
                    Description = inputArgs.Length == 3 ? inputArgs[2] : null,
                    Acronym = acronym,
                    CreatorId = currentUser.Id
                };

                context.Teams.Add(team);

                context.SaveChanges();
            }

            return $"Team {teamName} successfully created!";
        }
    }
}
