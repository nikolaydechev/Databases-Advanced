namespace P01_StudentSystem.Data.Models.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class StudentCourseConfiguration : IEntityTypeConfiguration<StudentCourse>
    {
        public void Configure(EntityTypeBuilder<StudentCourse> builder)
        {
            builder.HasKey(e => new { e.StudentId, e.CourseId });

            builder.HasOne(e => e.Course)
                    .WithMany(e => e.StudentsEnrolled)
                    .HasForeignKey(e => e.CourseId);

            builder.HasOne(e => e.Student)
                    .WithMany(e => e.CourseEnrollments)
                    .HasForeignKey(e => e.StudentId);
        }
    }
}
