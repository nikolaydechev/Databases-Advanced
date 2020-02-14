namespace P01_StudentSystem.Data.Models.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class HomeworkConfiguration : IEntityTypeConfiguration<Homework>
    {
        public void Configure(EntityTypeBuilder<Homework> builder)
        {
            builder.HasKey(e => e.HomeworkId);

            builder.Property(e => e.Content)
                    .IsUnicode(false);

            builder.Property(e => e.SubmissionTime)
                    .HasColumnType("DATETIME2");

            builder.HasOne(e => e.Student)
                    .WithMany(e => e.HomeworkSubmissions)
                    .HasForeignKey(e => e.StudentId);

            builder.HasOne(e => e.Course)
                    .WithMany(e => e.HomeworkSubmissions)
                    .HasForeignKey(e => e.CourseId);
        }
    }
}
