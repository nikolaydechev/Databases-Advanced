namespace TeamBuilder.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Team
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
        
        public string Acronym { get; set; }

        public int CreatorId { get; set; }
        public User Creator { get; set; }

        public ICollection<TeamEvent> TeamEvents { get; set; } = new List<TeamEvent>();

        public ICollection<UserTeam> Members { get; set; } = new List<UserTeam>();

        public ICollection<Invitation> Invitations { get; set; } = new List<Invitation>();
    }
}
