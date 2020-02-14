namespace PhotoShare.Client.Core.Commands
{
    using System;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using PhotoShare.Client.Core.Commands.Contracts;
    using PhotoShare.Data;
    using PhotoShare.Models;

    public class AddTagToCommand : ICommand
    {
        // AddTagTo <albumName> <tag>

        public string Execute(Session session, string[] arguments)
        {
            if (!session.IsLoggedIn())
            {
                throw new InvalidOperationException("Invalid credentials!");
            }

            if (arguments.Length != 3)
            {
                throw new InvalidOperationException($"Command {arguments[0]} not valid!");
            }

            var albumName = arguments[1];
            var tag = arguments[2];

            using (var context = new PhotoShareContext())
            {
                Album currentAlbum = context.Albums
                    .Include(a => a.AlbumTags)
                    .Include(a => a.AlbumRoles)
                    .ThenInclude(ar => ar.User)
                    .FirstOrDefault(a => a.Name == albumName);

                var currentTag = context.Tags.FirstOrDefault(t => t.Name == tag);

                if (currentAlbum == null || currentTag == null)
                {
                    throw new ArgumentException($"Either tag or album do not exist!");
                }

                bool isUserOwner = currentAlbum.AlbumRoles
                    .Any(ar => ar.Role == Role.Owner && ar.User.Username == session.User.Username);

                if (!isUserOwner)
                {
                    throw new InvalidOperationException("Invalid credentials!");
                }

                currentAlbum.AlbumTags.Add(new AlbumTag() {Album = currentAlbum, Tag = currentTag});

                context.SaveChanges();
            }

            return $"Tag {tag} added to {albumName}!";
        }
    }
}
