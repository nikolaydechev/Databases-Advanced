namespace TeamBuilder.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using TeamBuilder.Models;

    public class TeamConfiguration : IEntityTypeConfiguration<Team>
    {
        public void Configure(EntityTypeBuilder<Team> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Name)
                .HasMaxLength(25)
                .IsRequired();

            builder
                .HasIndex(t => t.Name)
                .IsUnique();

            builder.Property(t => t.Description)
                .HasMaxLength(32);

            builder.HasOne(t => t.Creator)
                .WithMany(t => t.UserTeams)
                .HasForeignKey(t => t.CreatorId);

            //builder.Property(t=>t.Acronym)
        }
    }
}
