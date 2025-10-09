using FoodDelivery.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace FoodDelivery.DataAccess;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // Existing tables
    public DbSet<User> Users { get; set; }

    // ✅ Add this line:
    public DbSet<MenuItem> MenuItems { get; set; }

    // ✅ Also add this for the Cart feature (for next migration)
    public DbSet<CartItem> CartItems { get; set; }

    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Rating> Ratings { get; set; }


}
