namespace TeamBuilder.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using TeamBuilder.Models;

    public class TeamEventConfiguration : IEntityTypeConfiguration<TeamEvent>
    {
        public void Configure(EntityTypeBuilder<TeamEvent> builder)
        {
            builder.HasKey(te => new {te.EventId, te.TeamId});

            builder.HasOne(te => te.Team)
                .WithMany(te => te.TeamEvents)
                .HasForeignKey(te => te.TeamId);
            
            builder.HasOne(te => te.Event)
                .WithMany(te => te.ParticipatingEventTeams)
                .HasForeignKey(te => te.EventId);
        }
    }
}
