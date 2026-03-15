using MediatR;
using Sundarban.Modules.Orders.Domain;
using Sundarban.Modules.Orders.Infrastructure;

namespace Sundarban.Modules.Orders.Application.Queries;

public class GetOrderHandler : IRequestHandler<GetOrderQuery, List<Order>>
{
    private readonly OrdersDbContext _dbContext;
    
    public GetOrderHandler(OrdersDbContext dbContext) => _dbContext = dbContext;
    
    public Task<List<Order>> Handle(GetOrderQuery request, CancellationToken cancellationToken)
    {
        var orderList = _dbContext.Orders.ToList();
        return Task.FromResult(orderList);
    }
}