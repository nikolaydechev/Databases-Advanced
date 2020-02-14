namespace PhotoShare.Client.Core.Commands
{
    using System;
    using System.Linq;
    using Models;
    using Data;
    using PhotoShare.Client.Core.Commands.Contracts;

    public class AddTownCommand : ICommand
    {
        // AddTown <townName> <countryName>

        public string Execute(Session session, string[] data)
        {
            if (!session.IsLoggedIn())
            {
                throw new InvalidOperationException($"Invalid credentials!");
            }

            string townName = data[1];
            string country = data[2];

            using (var context = new PhotoShareContext())
            {
                if (context.Towns.Any(t => t.Name == townName))
                {
                    throw new ArgumentException($"Town {townName} was already added!");
                }

                Town town = new Town
                {
                    Name = townName,
                    Country = country
                };

                context.Towns.Add(town);
                context.SaveChanges();
            }

            return $"Town {townName} was added successfully!";
        }
    }
}
