using MediatR;
using Sundarban.Contracts;
using Sundarban.Modules.Orders.Domain;
using Sundarban.Modules.Orders.Infrastructure;

namespace Sundarban.Modules.Orders.Application.Commands;

public class PlaceOrderHandler : IRequestHandler<PlaceOrderCommand, Guid>
{
    private readonly OrdersDbContext _dbContext;
    private readonly ICustomerService _customerService;

    public PlaceOrderHandler(OrdersDbContext dbContext, ICustomerService customerService)
    {
        _dbContext = dbContext;
        _customerService = customerService;
    }
    
    public async Task<Guid> Handle(PlaceOrderCommand command, CancellationToken cancellationToken)
    {
        var customerExists = await _customerService.CustomerExistsAsync(command.CustomerId, cancellationToken);
        if (!customerExists)
            throw new InvalidOperationException($"Customer {command.CustomerId} does not exist");
        var order = Order.Create(command.CustomerId, command.Name, command.Category, command.Price);
        _dbContext.Orders.Add(order);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return order.Id;
    }
}