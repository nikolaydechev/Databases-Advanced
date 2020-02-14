namespace ProductsShop.Data
{
    using Microsoft.EntityFrameworkCore;
    using ProductsShop.Models;

    public class ProductsShopContext : DbContext
    {
        public ProductsShopContext()
        { 
        }

        public ProductsShopContext(DbContextOptions options)
            :base(options)
        { 
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryProduct> CategoryProduct { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.FirstName)
                    .IsRequired(false);

                entity.Property(e => e.Age)
                    .IsRequired(false);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.BuyerId)
                    .IsRequired(false);

                entity.HasOne(e => e.Buyer)
                    .WithMany(e => e.BoughtProducts)
                    .HasForeignKey(e => e.BuyerId);

                entity.HasOne(e => e.Seller)
                    .WithMany(e => e.SoldProducts)
                    .HasForeignKey(e => e.SellerId);
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name)
                    .HasMaxLength(15);
            });

            modelBuilder.Entity<CategoryProduct>(entity =>
            {
                entity.HasKey(e => new {e.ProductId, e.CategoryId});

                entity.HasOne(e => e.Product)
                    .WithMany(e => e.ProductCategories)
                    .HasForeignKey(e => e.ProductId);

                entity.HasOne(e => e.Category)
                    .WithMany(e => e.CategoryProducts)
                    .HasForeignKey(e => e.CategoryId);
            });
        }
    }
}
