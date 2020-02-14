namespace P01_StudentSystem.Data
{
    using Microsoft.EntityFrameworkCore;
    using P01_StudentSystem.Data.Models;
    using P01_StudentSystem.Data.Models.Configuration;

    public class StudentSystemContext : DbContext
    {
        public StudentSystemContext()
        {
        }

        public StudentSystemContext(DbContextOptions options)
            :base(options)
        {
        }

        public DbSet<Student> Students { get; set; }

        public DbSet<Course> Courses { get; set; }

        public DbSet<Homework> HomeworkSubmissions { get; set; }

        public DbSet<Resource> Resources { get; set; }

        public DbSet<StudentCourse> StudentCourses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            ////Student
            builder.ApplyConfiguration(new StudentConfiguration());
            //Course
            builder.ApplyConfiguration(new CourseConfiguration());
            //StudentCourse
            builder.ApplyConfiguration(new StudentCourseConfiguration());
            //Resource
            builder.ApplyConfiguration(new ResourceConfiguration());
            //Homework
            builder.ApplyConfiguration(new HomeworkConfiguration());
        }
    }
}
