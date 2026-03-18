using MediatR;
using Sundarban.Modules.Customers.Domain;
using Sundarban.Modules.Customers.Infrastructure;

namespace Sundarban.Modules.Customers.Application.Queries;

public class GetCustomerHandler : IRequestHandler<GetCustomerQuery, List<Customer>>
{
    private readonly CustomersDbContext _dbContext;
    
    public GetCustomerHandler(CustomersDbContext dbContext) => _dbContext = dbContext;
    
    public Task<List<Customer>> Handle(GetCustomerQuery query, CancellationToken cancellationToken)
    {
        var customersList = _dbContext.Customers.ToList();
        return Task.FromResult(customersList);
    }
}