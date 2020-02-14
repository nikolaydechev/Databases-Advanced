namespace PhotoShare.Client.Core.Commands
{
    using System;
    using System.Linq;
    using Models;
    using Data;
    using PhotoShare.Client.Core.Commands.Contracts;
    using Utilities;

    public class AddTagCommand : ICommand
    {
        // AddTag <tag>

        public string Execute(Session session, string[] data)
        {
            if (!session.IsLoggedIn())
            {
                throw new InvalidOperationException($"Invalid credentials!");
            }

            if (data.Length != 2)
            {
                throw new InvalidOperationException($"Command {data[0]} not valid!");
            }

            string tag = data[1].ValidateOrTransform();

            using (var context = new PhotoShareContext())
            {
                if (context.Tags.Any(t => t.Name == tag))
                {
                    throw new ArgumentException($"Tag {tag} exists!");
                }

                context.Tags.Add(new Tag
                {
                    Name = tag
                });

                context.SaveChanges();
            }

            return $"Tag {tag} was added successfully!";
        }
    }
}
