namespace PhotoShare.Client.Core.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using PhotoShare.Client.Core.Commands.Contracts;
    using PhotoShare.Client.Utilities;
    using PhotoShare.Data;
    using PhotoShare.Models;

    public class CreateAlbumCommand : ICommand
    {
        // CreateAlbum <username> <albumTitle> <BgColor> <tag1> <tag2>...<tagN>

        public string Execute(Session session, string[] arguments)
        {
            if (!session.IsLoggedIn())
            {
                throw new InvalidOperationException($"Invalid credentials!");
            }

            if (arguments.Length <= 4)
            {
                throw new InvalidOperationException($"Command {arguments[0]} not valid!");
            }

            var userName = arguments[1];
            var albumTitle = arguments[2];

            Color bgColor;
            if (!Enum.TryParse(arguments[3], out bgColor))
            {
                throw new ArgumentException($"Color {arguments[2]} not found!");
            }

            var tags = arguments.Skip(3).Select(t => t.ValidateOrTransform()).ToArray();

            using (var context = new PhotoShareContext())
            {
                if (!tags.All(t => context.Tags.Any(ct => ct.Name == t)))
                {
                    throw new ArgumentException($"Invalid tags!");
                }

                var album = context.Albums.FirstOrDefault(a => a.Name == albumTitle);

                if (album == null)
                {
                    throw new ArgumentException($"Album {albumTitle} exists!");
                }

                var user = context.Users.FirstOrDefault(u => u.Username == userName);
                if (user == null)
                {
                    throw new ArgumentException($"User {userName} not found!");
                }

                if (user.Username != session.User.Username)
                {
                    throw new InvalidOperationException($"Invalid credentials!");
                }

                var newAlbum = new Album
                {
                    Name = albumTitle,
                    BackgroundColor = bgColor,
                    AlbumRoles = new List<AlbumRole>()
                    {
                        new AlbumRole()
                        {
                            User = user,
                            Role = Role.Owner
                        }
                    },
                    AlbumTags = tags.Select(t => new AlbumTag()
                    {
                        Tag = context.Tags.FirstOrDefault(ct => ct.Name == t)
                    })
                    .ToArray()
                };

                context.Albums.Add(newAlbum);

                context.SaveChanges();
            }

            return $"Album {albumTitle} successfully created!";
        }
    }
}
