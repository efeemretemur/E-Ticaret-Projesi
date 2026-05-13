using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Nora.Shop.Core.Entities;

namespace Nora.Shop.DataAccess.Context
{
    public class NoraShopContext : IdentityDbContext<AppUser>
    {
        public NoraShopContext(DbContextOptions<NoraShopContext> options)
            : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Product>()
                .Property(product => product.Price)
                .HasColumnType("decimal(18,2)");

            builder.Entity<Product>()
                .Property(product => product.ImageUrl)
                .IsRequired(false);

            builder.Entity<Order>()
                .Property(order => order.TotalPrice)
                .HasColumnType("decimal(18,2)");

            builder.Entity<OrderItem>()
                .Property(orderItem => orderItem.UnitPrice)
                .HasColumnType("decimal(18,2)");
        }
    }
}
