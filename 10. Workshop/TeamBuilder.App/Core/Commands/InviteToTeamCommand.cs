namespace TeamBuilder.App.Core.Commands
{
    using System;
    using System.Linq;
    using TeamBuilder.App.Core.Commands.Contracts;
    using TeamBuilder.App.Utilities;
    using TeamBuilder.Data;
    using TeamBuilder.Models;

    public class InviteToTeamCommand : ICommand
    {
        //•	InviteToTeam <teamName> <username>
        private const string SuccessMessage = "Team {0} invited {1}!";
        
        public string Execute(string[] inputArgs)
        {
            Check.CheckLength(2, inputArgs);

            AuthenticationManager.Authorize();
            
            string teamName = inputArgs[0];
            string userName = inputArgs[1];

            using (var context = new TeamBuilderContext())
            {
                var user = context.Users.FirstOrDefault(u => u.Username == userName);
                var team = context.Teams.FirstOrDefault(t => t.Name == teamName);
                
                if (user == null || team == null)
                {
                    throw new ArgumentException(Constants.ErrorMessages.TeamOrUserNotExist);
                }

                if (team.CreatorId == user.Id)
                {
                    var userTeam = new UserTeam
                    {
                        UserId = user.Id,
                        TeamId = team.Id
                    };

                    context.UserTeams.Add(userTeam);

                    context.SaveChanges();

                    return $"The creator of {teamName} was added directly.";
                }

                var currentUser = AuthenticationManager.GetCurrentUser();

                if (team.CreatorId != currentUser.Id || team.Members.Any(t=>t.UserId == user.Id || t.UserId==currentUser.Id))
                {
                    throw new InvalidOperationException(Constants.ErrorMessages.NotAllowed);
                }

                if (context.Invitations.Any(i => i.InvitedUserId == user.Id && i.TeamId == team.Id))
                {
                    throw new InvalidOperationException(Constants.ErrorMessages.InviteIsAlreadySent);
                }

                var invitation = new Invitation
                {
                    InvitedUserId = user.Id,
                    TeamId = team.Id
                };

                context.Invitations.Add(invitation);

                context.SaveChanges();
            }

            return string.Format(SuccessMessage, teamName, userName);
        }
    }
}
