using MediatR;
using Microsoft.EntityFrameworkCore;
using Sundarban.Exceptions;
using Sundarban.Modules.Customers.Domain;
using Sundarban.Modules.Customers.Infrastructure;

namespace Sundarban.Modules.Customers.Application.Queries.GetCustomerById;

public class GetCustomerByIdHandler : IRequestHandler<GetCustomerByIdQuery, Customer>
{
    private readonly CustomersDbContext _dbContext;
    
    public GetCustomerByIdHandler(CustomersDbContext dbContext) => _dbContext = dbContext;
    
    public async Task<Customer> Handle(GetCustomerByIdQuery query, CancellationToken cancellationToken)
    {
        var customer = await _dbContext.Customers.FirstOrDefaultAsync(c => c.Id == query.CustomerId, cancellationToken);
        if (customer is null)
            throw new NotFoundException("Customer", query.CustomerId);
        return customer;
    }
}