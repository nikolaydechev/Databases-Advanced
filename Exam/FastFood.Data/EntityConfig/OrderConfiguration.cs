namespace FastFood.Data.EntityConfig
{
    using FastFood.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o => o.Id);

            builder.Property(o => o.Customer).IsRequired();

            builder.Property(o => o.DateTime).IsRequired();

            builder.Property(o => o.Type).IsRequired();

            builder.Ignore(o => o.TotalPrice);

            builder.HasOne(o => o.Employee)
                .WithMany(o => o.Orders)
                .HasForeignKey(o => o.EmployeeId);
        }
    }
}
