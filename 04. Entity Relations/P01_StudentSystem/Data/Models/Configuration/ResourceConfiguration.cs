namespace P01_StudentSystem.Data.Models.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ResourceConfiguration : IEntityTypeConfiguration<Resource>
    {
        public void Configure(EntityTypeBuilder<Resource> builder)
        {
            builder.HasKey(e => e.ResourceId);

            builder.Property(e => e.Name)
                    .IsRequired()
                    .IsUnicode()
                    .HasMaxLength(50);

            builder.Property(e => e.Url)
                    .IsUnicode(false);

            builder.HasOne(e => e.Course)
                    .WithMany(e => e.Resources)
                    .HasForeignKey(e => e.CourseId);
        }
    }
}
