namespace FastFood.Data.EntityConfig
{
    using FastFood.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ItemConfiguration : IEntityTypeConfiguration<Item>
    {
        public void Configure(EntityTypeBuilder<Item> builder)
        {
            builder.HasKey(i => i.Id);

            builder.Property(i => i.Name)
                .HasMaxLength(30)
                .IsRequired();

            builder.HasAlternateKey(i => i.Name);

            builder.HasOne(i => i.Category)
                .WithMany(i => i.Items)
                .HasForeignKey(i => i.CategoryId);

            builder.Property(i => i.Price)
                .IsRequired();
        }
    }
}
