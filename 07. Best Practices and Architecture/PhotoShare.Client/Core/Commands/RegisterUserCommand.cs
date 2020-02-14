namespace PhotoShare.Client.Core.Commands
{
    using System;
    using System.Linq;
    using Models;
    using Data;
    using PhotoShare.Client.Core.Commands.Contracts;

    public class RegisterUserCommand : ICommand
    {
        // RegisterUser <username> <password> <repeat-password> <email>

        public string Execute(Session session, string[] data)
        {
            if (data.Length != 5)
            {
                throw new InvalidOperationException($"Command {data[0]} not valid!");
            }

            string username = data[1];

            if (session.IsLoggedIn())
            {
                throw new InvalidOperationException("Invalid credentials!");
            }

            string password = data[2];
            string repeatPassword = data[3];
            string email = data[4];

            using (var context = new PhotoShareContext())
            {
                if (context.Users.Any(u => u.Username == username))
                {
                    throw new InvalidOperationException($"Username {username} is already taken!");
                }

                if (password != repeatPassword)
                {
                    throw new ArgumentException("Passwords do not match!");
                }

                User user = new User
                {
                    Username = username,
                    Password = password,
                    Email = email,
                    IsDeleted = false,
                    RegisteredOn = DateTime.Now,
                    LastTimeLoggedIn = DateTime.Now,
                };

                context.Users.Add(user);
                context.SaveChanges();
            }

            return $"User {username} was registered successfully!";
        }
    }
}
