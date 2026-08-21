using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Models;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<Infrastructure.Models.User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        private readonly IConfiguration _configuration;


        public AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseLazyLoadingProxies()
                              .UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
            }
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Cart> Carts { get; set; }


        public DbSet<Size> Sizes { get; set; }
        public DbSet<SizeType> SizeTypes { get; set; }
        public DbSet<ProductSize> ProductSizes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Унікальний індекс на назву категорії
            modelBuilder.Entity<Category>()
                    .HasIndex(c => c.Name)
                    .IsUnique();

            // Унікальні індекси для ApplicationUser
            modelBuilder.Entity<Infrastructure.Models.User>()
                .HasIndex(u => u.UserName)
                .IsUnique();

            modelBuilder.Entity<Infrastructure.Models.User>()
               .HasIndex(u => u.Email)
               .IsUnique();

            // Зв'язки для ProductCategory (багато до багатьох)
            modelBuilder.Entity<ProductCategory>()
                .HasKey(pc => new { pc.ProductId, pc.CategoryId });

            modelBuilder.Entity<ProductCategory>()
                .HasOne(pc => pc.Product)
                .WithMany(p => p.ProductCategories)
                .HasForeignKey(pc => pc.ProductId)
                .OnDelete(DeleteBehavior.Cascade); // Каскадне видалення

            modelBuilder.Entity<ProductCategory>()
                .HasOne(pc => pc.Category)
                .WithMany(c => c.ProductCategories)
                .HasForeignKey(pc => pc.CategoryId)
                .OnDelete(DeleteBehavior.Restrict); // Не видаляємо категорію при видаленні продукту


            modelBuilder.Entity<ProductSize>()
                    .HasKey(ps => ps.Id);

            // Унікальна комбінація ProductId і SizeId
            modelBuilder.Entity<ProductSize>()
                .HasIndex(ps => new { ps.ProductId, ps.SizeId })
                .IsUnique();

            modelBuilder.Entity<ProductSize>()
                .HasOne(ps => ps.Product)
                .WithMany(p => p.ProductSizes)
                .HasForeignKey(ps => ps.ProductId)
                .OnDelete(DeleteBehavior.Cascade); // Каскадне видалення

            modelBuilder.Entity<ProductSize>()
                .HasOne(ps => ps.Size)
                .WithMany(s => s.ProductSizes)
                .HasForeignKey(ps => ps.SizeId)
                .OnDelete(DeleteBehavior.Restrict); // Не видаляємо Size при видаленні ProductSize

            // Зв'язки для Review (один до багатьох)
            modelBuilder.Entity<Review>()
                .HasOne(r => r.Product)
                .WithMany(p => p.Reviews)
                .HasForeignKey(r => r.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // Зв'язки для Review (один до багатьох)
            modelBuilder.Entity<Review>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Налаштування зв'язків для Cart
            modelBuilder.Entity<Cart>()
                .HasMany(c => c.CartItems)
                .WithOne(ci => ci.Cart)
                .HasForeignKey(ci => ci.CartId);

            // Налаштування зв'язків для CartItem
            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Product)
                .WithMany() // Якщо Product має колекцію CartItems, можна вказати
                .HasForeignKey(ci => ci.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.ProductSize)
                .WithMany() // Якщо ProductSize має колекцію CartItems, можна вказати
                .HasForeignKey(ci => ci.ProductSizeId)
                .OnDelete(DeleteBehavior.Restrict); // Залишити Restrict

            // Додаємо унікальне обмеження на комбінацію ProductId, ProductSizeId і UserId
            modelBuilder.Entity<CartItem>()
                .HasIndex(ci => new { ci.ProductId, ci.ProductSizeId, ci.UserId })
                .IsUnique();


            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            // Налаштування ApplicationUser
            modelBuilder.ApplyConfiguration(new ApplicationUserEntityConfiguration());
        }

    }

    internal class ApplicationUserEntityConfiguration : IEntityTypeConfiguration<User>
    {
        void IEntityTypeConfiguration<User>.Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(u => u.FirstName)
                   .HasMaxLength(255)
                   .IsRequired(); // Optional: mark it as required

            builder.Property(u => u.LastName)
                   .HasMaxLength(255)
                   .IsRequired(); // Optional: mark it as required

            builder.Property(u => u.DateOfBitrh)
                   .IsRequired(); // Optional: mark it as required
        }
    }
}
