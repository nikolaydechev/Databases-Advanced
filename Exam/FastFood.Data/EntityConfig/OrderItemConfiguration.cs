namespace FastFood.Data.EntityConfig
{
    using FastFood.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.HasKey(oi => new {oi.OrderId, oi.ItemId});

            builder.HasOne(oi => oi.Order)
                .WithMany(oi => oi.OrderItems)
                .HasForeignKey(oi => oi.OrderId);

            builder.HasOne(oi => oi.Item)
                .WithMany(oi => oi.OrderItems)
                .HasForeignKey(oi => oi.ItemId);

            builder.Property(oi => oi.Quantity).IsRequired();
        }
    }
}
