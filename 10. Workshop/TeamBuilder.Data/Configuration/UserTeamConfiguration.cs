namespace TeamBuilder.Data.Configuration
{
    using System.Runtime.InteropServices.ComTypes;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using TeamBuilder.Models;
    public class UserTeamConfiguration : IEntityTypeConfiguration<UserTeam>
    {
        public void Configure(EntityTypeBuilder<UserTeam> builder)
        {
            builder.HasKey(ut => new {ut.UserId, ut.TeamId});
            
            builder.HasOne(ut => ut.User)
                .WithMany(ut => ut.CreatedUserTeams)
                .HasForeignKey(ut => ut.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(ut => ut.Team)
                .WithMany(ut => ut.Members)
                .HasForeignKey(ut => ut.TeamId);
        }
    }
}
