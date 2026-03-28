using Microsoft.EntityFrameworkCore;
using Sundarban.Contracts;
using Sundarban.Modules.Orders.Enums;

namespace Sundarban.Modules.Orders.Infrastructure;

public class OrderService : IOrderService
{
    private readonly OrdersDbContext _ordersDbContext;
    
    public OrderService(OrdersDbContext ordersDbContext) => _ordersDbContext = ordersDbContext;
    
    public Task<bool> OrderExistsAsync(Guid orderId, CancellationToken cancellationToken)
    {
        return _ordersDbContext.Orders.AnyAsync(o => o.Id == orderId, cancellationToken);
    }

    public async Task<decimal> GetOrderAmountAsync(Guid orderId, CancellationToken cancellationToken)
    {
        var order = await _ordersDbContext.Orders.FirstOrDefaultAsync(o => o.Id == orderId, cancellationToken);
        return (order is not null) ? order.Price : 0m;
    }

    public async Task<Guid> GetOrderCustomerIdAsync(Guid orderId, CancellationToken cancellationToken)
    {
        var order = await _ordersDbContext.Orders.FirstOrDefaultAsync(o => o.Id == orderId, cancellationToken);
        return (order is not null) ? order.CustomerId : Guid.Empty;
    }
}