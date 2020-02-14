namespace PhotoShare.Client.Core.Commands
{
    using System;
    using System.Linq;
    using System.Text;
    using Microsoft.EntityFrameworkCore;
    using PhotoShare.Client.Core.Commands.Contracts;
    using PhotoShare.Data;

    public class ListFriendsCommand : ICommand
    {
        // PrintFriendsList <username>

        public string Execute(Session session, string[] data)
        {
            if (data.Length != 2)
            {
                throw new InvalidOperationException($"Command {data[0]} not valid!");
            }

            var userName = data[1];
            var sb = new StringBuilder();

            using (var context = new PhotoShareContext())
            {
                var user = context.Users
                    .Include(u => u.FriendsAdded)
                    .ThenInclude(fa => fa.Friend)
                    .FirstOrDefault(u => u.Username == userName);

                if (user == null)
                {
                    throw new ArgumentException($"User {userName} not found!");
                }

                if (!user.FriendsAdded.Any())
                {
                    return $"No friends for this user. :(";
                }

                sb.AppendLine($"Friends:");
                foreach (var friend in user.FriendsAdded.OrderBy(f => f.Friend.Username))
                {
                    sb.AppendLine($"-{friend.Friend.Username}");
                }
            }

            return sb.ToString().Trim();
        }
    }
}
