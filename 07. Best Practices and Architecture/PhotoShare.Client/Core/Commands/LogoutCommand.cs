namespace PhotoShare.Client.Core.Commands
{
    using PhotoShare.Client.Core.Commands.Contracts;

    public class LogoutCommand : ICommand 
    {
        public string Execute(Session session, params string[] arguments)
        {
            if (!session.IsLoggedIn())
            {
                return $"You should log in first in order to logout.";
            }

            var logOutOutput = $"User {session.User.Username} successfully logged out!";

            session.Logout();

            return logOutOutput;
        }
    }
}
