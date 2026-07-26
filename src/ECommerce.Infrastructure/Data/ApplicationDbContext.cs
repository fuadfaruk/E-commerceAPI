using ECommerce.Application.Common.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Data
{
    public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
        : IdentityDbContext<ApplicationUser>(options), IApplicationDbContext
    {
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Order> Orders => Set<Order>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Customer>(entity =>
            {
                entity.Property(x => x.FirstName).HasMaxLength(80).IsRequired();
                entity.Property(x => x.LastName).HasMaxLength(80).IsRequired();
                entity.Property(x => x.Email).HasMaxLength(160).IsRequired();
                entity.Property(x => x.UserIdentityId).HasMaxLength(450).IsRequired();
                entity.HasIndex(x => x.UserIdentityId).IsUnique();
            });

            builder.Entity<Product>(entity =>
            {
                entity.Property(x => x.Name).HasMaxLength(120).IsRequired();
                entity.Property(x => x.Description).HasMaxLength(1000);
                entity.OwnsOne(x => x.Price, money =>
                {
                    money.Property(x => x.Amount).HasColumnType("decimal(18,2)");
                    money.Property(x => x.Currency).HasMaxLength(3);
                });
            });

            builder.Entity<Order>(entity =>
            {
                entity.Property(x => x.Status).HasConversion<string>().HasMaxLength(30);
                entity.OwnsOne(x => x.ShippingAddress);
                entity.OwnsMany(x => x.Items, item =>
                {
                    item.WithOwner().HasForeignKey("OrderId");
                    item.Property<Guid>("OrderId");
                    item.HasKey(x => x.Id);
                    item.Property(x => x.ProductId).HasMaxLength(120).IsRequired();
                    item.OwnsOne(x => x.UnitPrice, money =>
                    {
                        money.Property(x => x.Amount).HasColumnType("decimal(18,2)");
                        money.Property(x => x.Currency).HasMaxLength(3);
                    });
                });
                entity.Navigation(x => x.Items).UsePropertyAccessMode(PropertyAccessMode.Field);
            });

            SeedProducts(builder);
        }
        private static void SeedProducts(ModelBuilder builder)
        {
            var keyboardId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var mouseId = Guid.Parse("22222222-2222-2222-2222-222222222222");

            builder.Entity<Product>().HasData(
                new { id = keyboardId, name = "Mechanical Keyboard", Description = "Tactile switches and white backlight.", StockQuantity = 25 },
                new { id = mouseId, name = "Wireless Mouse", Description = "Ergonomic mouse with USB-C charging", StockQuantity = 40 });

            builder.Entity<Product>().OwnsOne(x => x.Price).HasData(
                new { ProductId = keyboardId, Amount = 89.99m, Currency = "USD" },
                new { ProductId = mouseId, Amount = 39.99m, Currency = "USD" });
        }
    }

}
