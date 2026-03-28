using MediatR;
using Microsoft.EntityFrameworkCore;
using Sundarban.Contracts;
using Sundarban.Exceptions;
using Sundarban.Modules.Orders.Enums;
using Sundarban.Modules.Orders.Infrastructure;

namespace Sundarban.Modules.Orders.Application.EventHandlers;

public class OrderPaidEventHandler : INotificationHandler<OrderPaidEvent>
{
    private readonly OrdersDbContext _dbContext;

    public OrderPaidEventHandler(OrdersDbContext dbContext) => _dbContext = dbContext;
    
    public async Task Handle(OrderPaidEvent notification, CancellationToken cancellationToken)
    {
        var order = await _dbContext.Orders.FirstOrDefaultAsync(o => o.Id == notification.OrderId, cancellationToken);
        if (order is null)
            throw new NotFoundException("Order", notification.OrderId);
        order.UpdateStatus(OrderStatus.Paid);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}