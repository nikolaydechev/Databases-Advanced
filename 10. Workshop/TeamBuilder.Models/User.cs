namespace TeamBuilder.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using TeamBuilder.Models.Enums;

    public class User
    {
        public int Id { get; set; }
        
        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
        
        public string Password { get; set; }

        public Gender Gender { get; set; }

        public int Age { get; set; }

        public bool IsDeleted { get; set; }

        public ICollection<Event> CreatedEvents { get; set; } = new List<Event>();
        public ICollection<Team> UserTeams { get; set; } = new List<Team>();
        public ICollection<UserTeam> CreatedUserTeams { get; set; } = new List<UserTeam>();
        public ICollection<Invitation> ReceivedInvitations { get; set; } = new List<Invitation>();
    }
}
