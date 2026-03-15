using Microsoft.EntityFrameworkCore;
using Sundarban.Contracts;

namespace Sundarban.Modules.Customers.Infrastructure;

public class CustomerService : ICustomerService
{
    private readonly CustomersDbContext _dbContext;
    
    public CustomerService(CustomersDbContext dbContext) => _dbContext = dbContext;
    
    public async Task<bool> CustomerExistsAsync(Guid customerId, CancellationToken ct)
    {
        return await _dbContext.Customers.AnyAsync(c => c.Id == customerId, ct);
    }
}