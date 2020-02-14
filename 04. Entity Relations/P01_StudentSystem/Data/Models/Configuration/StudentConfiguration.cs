namespace P01_StudentSystem.Data.Models.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class StudentConfiguration :IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.HasKey(e => e.StudentId);

            builder.Property(e => e.Name)
                    .IsRequired()
                    .IsUnicode()
                    .HasMaxLength(100);

            builder.Property(e => e.PhoneNumber)
                    .IsRequired(false)
                    .IsUnicode(false)
                    .HasMaxLength(10);

            builder.Property(e => e.RegisteredOn)
                    .HasColumnType("DATETIME2");

            builder.Property(e => e.Birthday)
                    .IsRequired(false)
                    .HasColumnType("DATETIME2");
        }
    }
}
