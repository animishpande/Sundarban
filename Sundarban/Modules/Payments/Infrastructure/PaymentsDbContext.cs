using Microsoft.EntityFrameworkCore;
using Sundarban.Modules.Payments.Domain;

namespace Sundarban.Modules.Payments.Infrastructure;

public class PaymentsDbContext : DbContext
{
    public PaymentsDbContext(DbContextOptions<PaymentsDbContext> options) : base(options) {}
    
    public DbSet<Payment> Payments => Set<Payment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("Payments");
    }
}