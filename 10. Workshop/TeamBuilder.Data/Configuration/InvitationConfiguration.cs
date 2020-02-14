namespace TeamBuilder.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using TeamBuilder.Models;

    public class InvitationConfiguration : IEntityTypeConfiguration<Invitation>
    {
        public void Configure(EntityTypeBuilder<Invitation> builder)
        {
            builder.HasKey(i => i.Id);

            builder.HasOne(i => i.InvitedUser)
                .WithMany(i => i.ReceivedInvitations)
                .HasForeignKey(i => i.InvitedUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(i => i.Team)
                .WithMany(i => i.Invitations)
                .HasForeignKey(i => i.TeamId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
