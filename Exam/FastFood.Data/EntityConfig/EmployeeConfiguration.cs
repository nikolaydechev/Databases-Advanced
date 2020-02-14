namespace FastFood.Data.EntityConfig
{
    using FastFood.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Name)
                .HasMaxLength(30)
                .IsRequired();

            builder.Property(e => e.Age)
                .HasMaxLength(80)
                .IsRequired();

            builder.HasOne(e => e.Position)
                .WithMany(e => e.Employees)
                .HasForeignKey(e => e.PositionId);
        }
    }
}
