using Microsoft.EntityFrameworkCore;
using Sundarban.Modules.Orders.Domain;

namespace Sundarban.Modules.Orders.Infrastructure;

public class OrdersDbContext : DbContext
{
    public OrdersDbContext(DbContextOptions<OrdersDbContext> options) : base(options) { }
    
    public DbSet<Order> Orders => Set<Order>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("orders");
        modelBuilder.Entity<Order>(e =>
        {
            e.HasKey(o => o.Id);
            e.Property(o => o.Status).HasMaxLength(50);
            e.Property(o => o.Name).IsRequired().HasMaxLength(500);
            e.Property(o => o.Category).IsRequired().HasMaxLength(500);
            e.Property(o => o.Price).IsRequired();
        });
    }
}