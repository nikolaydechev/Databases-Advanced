namespace PhotoShare.Client.Core.Commands
{
    using System;
    using PhotoShare.Data;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using PhotoShare.Client.Core.Commands.Contracts;
    using PhotoShare.Models;

    public class AcceptFriendCommand : ICommand
    {
        // AcceptFriend <username1> <username2>

        public string Execute(Session session, string[] data)
        {
            if (!session.IsLoggedIn())
            {
                throw new InvalidOperationException("Invalid credentials!");
            }

            if (data.Length != 3)
            {
                throw new InvalidOperationException($"Command {data[0]} not valid!");
            }

            var userName1 = data[1];
            var userName2 = data[2];

            using (var context = new PhotoShareContext())
            {
                var user1 = context.Users
                    .Include(u => u.FriendsAdded)
                        .ThenInclude(fa => fa.Friend)
                    .SingleOrDefault(u => u.Username == userName1);

                var user2 = context.Users
                    .Include(u => u.FriendsAdded)
                        .ThenInclude(fa => fa.Friend)
                    .FirstOrDefault(u => u.Username == userName2);

                if (user1 == null)
                {
                    throw new ArgumentException($"{userName1} not found!");
                }
                if (user2 == null)
                {
                    throw new ArgumentException($"{userName2} not found!");
                }
                if (user1.FriendsAdded.Any(f => f.Friend.Username == userName2))
                {
                    throw new InvalidOperationException($"{userName2} is already a friend to {userName1}");
                }
                if (user2.FriendsAdded.Any(f => f.Friend.Username == userName1))
                {
                    throw new InvalidOperationException($"{userName1} is already a friend to {userName2}");
                }
                if (user1.AddedAsFriendBy.All(f => f.Friend.Username != userName2))
                {
                    throw new InvalidOperationException($"{userName2} has not added {userName1} as a friend");
                }

                var friendShip = new Friendship() { User = user1, Friend = user2 };

                user1.FriendsAdded.Add(friendShip);

                context.SaveChanges();
            }

            return $"{userName1} accepted {userName2} as a friend";
        }
    }
}
