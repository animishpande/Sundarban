using MediatR;
using Sundarban.Modules.Orders.Domain;
using Sundarban.Modules.Orders.Infrastructure;

namespace Sundarban.Modules.Orders.Application.Queries;

public class GetOrderHandler : IRequestHandler<GetOrderQuery, List<Order>>
{
    private readonly OrdersDbContext _dbContext;
    
    public GetOrderHandler(OrdersDbContext dbContext) => _dbContext = dbContext;
    
    public Task<List<Order>> Handle(GetOrderQuery query, CancellationToken cancellationToken)
    {
        var orderList = _dbContext.Orders.Where(o => o.CustomerId == query.CustomerId).ToList();
        return Task.FromResult(orderList);
    }
}