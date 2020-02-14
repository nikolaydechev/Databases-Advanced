namespace PhotoShare.Client.Core.Commands
{
    using System;
    using System.Linq;
    using PhotoShare.Client.Core.Commands.Contracts;
    using PhotoShare.Data;

    public class LoginCommand : ICommand
    {
        public string Execute(Session session, params string[] arguments)
        {
            if (session.IsLoggedIn())
            {
                throw new ArgumentException($"You should logout first!");
            }

            string userName = arguments[1];
            string password = arguments[2];

            using (var context = new PhotoShareContext())
            {
                var user = context.Users.FirstOrDefault(u => u.Username == userName && u.Password == password);

                if (user == null)
                {
                    throw new ArgumentException($"Invalid username or password!");
                }

                session.Login(user);

                return $"User {user.Username} successfully logged in!";
            }
        }
    }
}
