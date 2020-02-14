namespace Employees.Data.EntityConfig
{
    using Employees.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
                builder.HasKey(e => e.Id);

                builder.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(60);

                builder.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(60);

                builder.Property(e => e.Address)
                    .HasMaxLength(250);

                builder.HasOne(e => e.Manager)
                    .WithMany(e => e.ManagerEmployees)
                    .HasForeignKey(e => e.ManagerId);
          
        }
    }
}
