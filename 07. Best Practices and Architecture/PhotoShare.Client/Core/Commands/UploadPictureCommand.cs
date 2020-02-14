namespace PhotoShare.Client.Core.Commands
{
    using System;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using PhotoShare.Client.Core.Commands.Contracts;
    using PhotoShare.Data;
    using PhotoShare.Models;

    public class UploadPictureCommand : ICommand
    {
        // UploadPicture <albumName> <pictureTitle> <pictureFilePath>

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

            var albumName = data[1];
            var pictureTitle = data[2];
            var pictureFilePath = data[3];

            using (var context = new PhotoShareContext())
            {
                var album = context.Albums
                    .Include(a => a.AlbumRoles)
                    .ThenInclude(ar => ar.User)
                    .FirstOrDefault(a => a.Name == albumName);

                if (album == null)
                {
                    throw new ArgumentException($"Album {albumName} not found!");
                }

                bool isUserOwner = album.AlbumRoles
                    .Any(ar => ar.Role == Role.Owner && ar.User.Username == session.User.Username);

                if (!isUserOwner)
                {
                    throw new InvalidOperationException("Invalid credentials!");
                }

                var picture = new Picture() {Album = album, Title = pictureTitle, Path = pictureFilePath};
                
                context.Pictures.Add(picture);
                context.SaveChanges();
            }

            return $"Picture {pictureTitle} added to {albumName}!";
        }
    }
}
