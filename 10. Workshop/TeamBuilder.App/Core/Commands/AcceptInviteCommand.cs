namespace TeamBuilder.App.Core.Commands
{
    using System;
    using System.Linq;
    using TeamBuilder.App.Core.Commands.Contracts;
    using TeamBuilder.App.Utilities;
    using TeamBuilder.Data;
    using TeamBuilder.Models;

    public class AcceptInviteCommand : ICommand
    {
        public string Execute(string[] inputArgs)
        {
            //•	AcceptInvite <teamName>

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

                if (!context.Invitations.Any(i => i.InvitedUserId == currentUser.Id && i.TeamId == team.Id))
                {
                    throw new ArgumentException(Constants.ErrorMessages.InviteNotFound, teamName);
                }

                var invitation =
                    context.Invitations.FirstOrDefault(
                        i => i.InvitedUserId == currentUser.Id && i.Team.Name == teamName);

                context.Invitations.Remove(invitation);

                context.UserTeams.Add(new UserTeam
                {
                    UserId = currentUser.Id,
                    TeamId = team.Id
                });

                context.SaveChanges();

                return $"User {currentUser.Username} joined team {teamName}!";
            }
        }
    }
}
