namespace PhotoShare.Client.Core.Commands
{
    using System;
    using System.Linq;

    using Data;
    using PhotoShare.Client.Core.Commands.Contracts;

    public class DeleteUserCommand : ICommand
    {
        // DeleteUser <username>

        public string Execute(Session session, string[] data)
        {
            if (!session.IsLoggedIn())
            {
                throw new InvalidOperationException("Invalid credentials!");
            }

            string userName = data[1];

            using (var context = new PhotoShareContext())
            {
                var user = context.Users.FirstOrDefault(u => u.Username == userName);
                if (user == null)
                {
                    throw new ArgumentException($"User {userName} not found!");
                }

                if (user.Username != session.User.Username)
                {
                    throw new InvalidOperationException("Invalid credentials!");
                }

                if (user.IsDeleted != null && user.IsDeleted.Value)
                {
                    throw new InvalidOperationException($"User {userName} is already deleted!");
                }

                user.IsDeleted = true;

                context.SaveChanges();
            }

            return $"User {userName} was deleted successfully!";
        }
    }
}
