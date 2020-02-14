namespace TeamBuilder.App.Core.Commands
{
    using System;
    using System.Linq;
    using TeamBuilder.App.Core.Commands.Contracts;
    using TeamBuilder.App.Utilities;
    using TeamBuilder.Data;

    public class DisbandCommand : ICommand
    {
        public string Execute(string[] inputArgs)
        {
            //•	Disband <teamName>

            Check.CheckLength(1, inputArgs);
            
            AuthenticationManager.Authorize();

            string teamName = inputArgs[0];

            using (var context = new TeamBuilderContext())
            {
                var team = context.Teams.FirstOrDefault(t => t.Name == teamName);

                if (team == null)
                {
                    throw new ArgumentException(Constants.ErrorMessages.TeamNotFound, teamName);
                }

                var currentUser = AuthenticationManager.GetCurrentUser();

                if (team.CreatorId != currentUser.Id)
                {
                    throw new InvalidOperationException(Constants.ErrorMessages.NotAllowed);
                }

                context.Teams.Remove(team);

                context.SaveChanges();
            }

            return $"{teamName} has disbanded!";
        }
    }
}
