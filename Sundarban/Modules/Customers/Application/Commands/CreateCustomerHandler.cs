using MediatR;
using Sundarban.Modules.Customers.Domain;
using Sundarban.Modules.Customers.Infrastructure;

namespace Sundarban.Modules.Customers.Application.Commands;

public class CreateCustomerHandler : IRequestHandler<CreateCustomerCommand, Guid>
{
    private readonly CustomersDbContext _dbContext;
    
    public CreateCustomerHandler(CustomersDbContext dbContext) => _dbContext = dbContext;
    
    public async Task<Guid> Handle(CreateCustomerCommand command, CancellationToken cancellationToken)
    {
        var customer = Customer.Create(command.FirstName, command.LastName, command.Email);
        _dbContext.Customers.Add(customer);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return customer.Id;
    }
}