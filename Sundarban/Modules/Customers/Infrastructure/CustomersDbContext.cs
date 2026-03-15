using Microsoft.EntityFrameworkCore;
using Sundarban.Modules.Customers.Domain;

namespace Sundarban.Modules.Customers.Infrastructure;

public class CustomersDbContext : DbContext
{
    public CustomersDbContext(DbContextOptions<CustomersDbContext> options) : base(options) {}

    public DbSet<Customer> Customers => Set<Customer>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("customers");
        modelBuilder.Entity<Customer>(e =>
        {
            e.HasKey(c => c.Id);
            e.Property(c => c.FirstName).HasMaxLength(300).IsRequired();
            e.Property(c => c.LastName).HasMaxLength(300).IsRequired();
            e.Property(c => c.Email).HasMaxLength(300).IsRequired();
        });
    }
}