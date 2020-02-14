namespace TeamBuilder.App.Core.Commands
{
    using System;
    using System.Linq;
    using TeamBuilder.App.Core.Commands.Contracts;
    using TeamBuilder.App.Utilities;
    using TeamBuilder.Data;

    public class DeclineInviteCommand : ICommand
    {
        public string Execute(string[] inputArgs)
        {
            //•	DeclineInvite <teamName>

            var teamName = inputArgs[0];

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

                context.SaveChanges();

                return $"Invite from {teamName} declined.";
            }
        }
    }
}
