using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;

namespace WebApp.Data
{
    public class AppDBContext : IdentityDbContext<AppUser>
    {
        public AppDBContext(DbContextOptions<AppDBContext> options)
    : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

        }
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<OrderProduct> OrderProducts { get; set; } = null!;

        public DbSet<Cart> Carts { get; set; } = null!;
        public DbSet<CartItem> CartItems { get; set; } = null!;
    }
}
