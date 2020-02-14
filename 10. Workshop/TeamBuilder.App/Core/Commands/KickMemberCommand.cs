namespace TeamBuilder.App.Core.Commands
{
    using System;
    using System.Linq;
    using TeamBuilder.App.Core.Commands.Contracts;
    using TeamBuilder.App.Utilities;
    using TeamBuilder.Data;
    using TeamBuilder.Models;
    using Constants = TeamBuilder.App.Utilities.Constants;

    public class KickMemberCommand : ICommand
    {
        public string Execute(string[] inputArgs)
        {
            //•	KickMember <teamName> <username>

            Check.CheckLength(2, inputArgs);

            AuthenticationManager.Authorize();

            string teamName = inputArgs[0];
            string userName = inputArgs[1];

            using (var context = new TeamBuilderContext())
            {
                var team = context.Teams.FirstOrDefault(t => t.Name == teamName);
                var user = context.Users.FirstOrDefault(u => u.Username == userName);
                if (team == null)
                {
                    throw new ArgumentException(Constants.ErrorMessages.TeamNotFound, teamName);
                }
                if (user == null)
                {
                    throw new ArgumentException(Constants.ErrorMessages.UserNotFound, userName);
                }
                if (!context.Teams.Any(t => t.Members.Any(m => m.UserId == user.Id && m.TeamId == team.Id)))
                {
                    throw new ArgumentException($"User {userName} is not a member in {teamName}!");
                }

                var currentUser = AuthenticationManager.GetCurrentUser();

                if (team.CreatorId != currentUser.Id)
                {
                    throw new InvalidOperationException(Constants.ErrorMessages.NotAllowed);
                }

                if (user.Id == team.CreatorId)
                {
                    throw new ArgumentException(Constants.ErrorMessages.CommandNotAllowed, "DisbandTeam");
                }

                var userTeam = context.UserTeams.FirstOrDefault(ut => ut.UserId == user.Id && ut.TeamId == team.Id);

                context.UserTeams.Remove(userTeam);

                context.SaveChanges();

                return $"User {userName} was kicked from {teamName}!";
            }
        }
    }
}
