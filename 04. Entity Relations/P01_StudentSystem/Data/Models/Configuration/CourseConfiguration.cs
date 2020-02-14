namespace P01_StudentSystem.Data.Models.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.HasKey(e => e.CourseId);

            builder.Property(e => e.Name)
                    .IsRequired()
                    .IsUnicode()
                    .HasMaxLength(80);

            builder.Property(e => e.Description)
                    .IsRequired(false)
                    .IsUnicode();

            builder.Property(e => e.StartDate).HasColumnType("DATETIME2");
            builder.Property(e => e.EndDate).HasColumnType("DATETIME2");

            builder.Property(e => e.Price);
        }
    }
}
