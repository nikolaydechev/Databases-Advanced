namespace PhotoShare.Client.Core.Commands
{
    using System;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using PhotoShare.Client.Core.Commands.Contracts;
    using PhotoShare.Data;

    public class ModifyUserCommand : ICommand
    {
        // ModifyUser <username> <property> <new value>
        // For example:
        // ModifyUser <username> Password <NewPassword>
        // ModifyUser <username> BornTown <newBornTownName>
        // ModifyUser <username> CurrentTown <newCurrentTownName>
        // !!! Cannot change username

        public string Execute(Session session, string[] data)
        {
            if (!session.IsLoggedIn())
            {
                throw new InvalidOperationException($"Invalid credentials!");
            }

            var userName = data[1];
            var property = data[2];
            var newValue = data[3];

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

                var town = context.Towns.FirstOrDefault(t => t.Name == newValue);

                switch (property)
                {
                    case "Password":
                        if (!newValue.Any(Char.IsLower) || !newValue.Any(Char.IsDigit))
                        {
                            throw new ArgumentException("Invalid Password");
                        }
                        user.Password = newValue;
                        break;

                    case "BornTown":
                        if (town == null)
                        {
                            throw new ArgumentException($"Value {newValue} not valid." +
                                                        Environment.NewLine +
                                                        $"Town {newValue} not found!");
                        }

                        user.BornTown = town;
                        break;

                    case "CurrentTown":
                        if (town == null)
                        {
                            throw new ArgumentException($"Value {newValue} not valid." +
                                                        Environment.NewLine +
                                                        $"Town {newValue} not found!");
                        }

                        user.CurrentTown = town;
                        break;

                    default:
                        throw new ArgumentException($"Property {property} not supported!");
                }

                context.SaveChanges();
            }

            return $"User {userName} {property} is {newValue}.";
        }
    }
}

