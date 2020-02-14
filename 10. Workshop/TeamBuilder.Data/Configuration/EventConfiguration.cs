namespace TeamBuilder.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using TeamBuilder.Models;

    public class EventConfiguration : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Name)
                .HasMaxLength(25)
                .IsRequired()
                .IsUnicode();

            builder.Property(e => e.Description)
                .HasMaxLength(250)
                .IsUnicode()
                .IsRequired();

            builder.HasOne(e => e.Creator)
                .WithMany(e => e.CreatedEvents)
                .HasForeignKey(e => e.CreatorId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
