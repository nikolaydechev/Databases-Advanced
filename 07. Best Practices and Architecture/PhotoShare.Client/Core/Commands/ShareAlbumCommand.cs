namespace PhotoShare.Client.Core.Commands
{
    using System;
    using System.Linq;
    using PhotoShare.Client.Core.Commands.Contracts;
    using PhotoShare.Data;
    using PhotoShare.Models;

    public class ShareAlbumCommand : ICommand
    {
        // ShareAlbum <albumId> <username> <permission>
        // For example:
        // ShareAlbum 4 dragon321 Owner
        // ShareAlbum 4 dragon11 Viewer

        public string Execute(Session session, string[] data)
        {
            if (!session.IsLoggedIn())
            {
                throw new InvalidOperationException("Invalid credentials!");
            }

            if (data.Length != 4)
            {
                throw new InvalidOperationException($"Command {data[0]} not valid!");
            }

            var albumId = int.Parse(data[1]);
            var userName = data[2];
            string albumName = string.Empty;

            Role permission;

            using (var context = new PhotoShareContext())
            {
                var currentAlbum = context.Albums.Find(albumId);

                if (currentAlbum == null)
                {
                    throw new ArgumentException($"Album with Id {albumId} not found!");
                }

                var user = context.Users.FirstOrDefault(u => u.Username == userName);

                if (user == null)
                {
                    throw new ArgumentException($"User {userName} not found!");
                }

                if (user.Username != session.User.Username)
                {
                    throw new InvalidOperationException("Invalid credentials!");
                }

                if (!Enum.TryParse(data[3], out permission))
                {
                    throw new ArgumentException($"Permission must be either “Owner” or “Viewer”!");
                }

                AlbumRole newAlbumRole = new AlbumRole()
                {
                    Album = currentAlbum,
                    User = user,
                    Role = permission
                };

                if (context.AlbumRoles.Any(ar => ar.Album == newAlbumRole.Album && ar.User == newAlbumRole.User))
                {
                    throw new ArgumentException($"Album {currentAlbum.Name} already shared to {user.Username} with role {permission}");
                }

                context.AlbumRoles.Add(newAlbumRole);

                context.SaveChanges();
            }

            return $"Username {userName} added to album {albumName} ({permission})";
        }
    }
}
