namespace TeamBuilder.App.Utilities
{
    using System.Linq;
    using TeamBuilder.Data;
    using TeamBuilder.Models;

    public static class CommandHelper
    {
        public static bool IsTeamExisting(string teamName)
        {
            using (var context = new TeamBuilderContext())
            {
                return context.Teams.Any(t => t.Name == teamName);
            }
        }

        public static bool IsUserExisting(string username)
        {
            using (var context = new TeamBuilderContext())
            {
                return context.Users.Any(t => t.Username == username && t.IsDeleted == false);
            }
        }

        public static bool IsInviteExisting(string teamName, User user)
        {
            using (var context = new TeamBuilderContext())
            {
                return context
                    .Invitations
                    .Any(i => i.Team.Name == teamName && i.InvitedUserId == user.Id && i.IsActive);
            }
        }

        public static bool IsMemberOfTeam(string teamName, string username)
        {
            using (var context = new TeamBuilderContext())
            {
                return context.Teams
                    .Single(t => t.Name == teamName)
                    .Members.Any(ut => ut.User.Username == username);
            }
        }

        public static bool IsEventExisting(string eventName)
        {
            using (var context = new TeamBuilderContext())
            {
                return context.Events.Any(e => e.Name == eventName);
            }
        }

        public static bool IsUserCreatorOfEvent(string eventName, User user)
        {
            using (var context = new TeamBuilderContext())
            {
                return context.Events
                           .Single(t => t.Name == eventName)
                           .Creator.Username == user.Username;
            }
        }

        public static bool IsUserCreatorOfTeam(string teamName, User user)
        {
            using (var context = new TeamBuilderContext())
            {
                return context.Teams
                           .Single(t => t.Name == teamName)
                           .Creator.Username == user.Username;
            }
        }
    }
}
