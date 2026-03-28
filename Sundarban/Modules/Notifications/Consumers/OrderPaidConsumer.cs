using MassTransit;
using Microsoft.AspNetCore.SignalR;
using Sundarban.Contracts;
using Sundarban.Modules.Notifications.Hubs;
using Sundarban.Shared.Services;

namespace Sundarban.Modules.Notifications.Consumers;

public class OrderPaidConsumer : IConsumer<OrderPaidEvent>
{
    private readonly IHubContext<NotificationsHub> _hubContext;
    private readonly ICacheService _cacheService;

    public OrderPaidConsumer(IHubContext<NotificationsHub> hubContext, ICacheService cacheService)
    {
        _hubContext = hubContext;
        _cacheService = cacheService;
    }
    
    public async Task Consume(ConsumeContext<OrderPaidEvent> context)
    {
        var message = context.Message;
        
        // Push to the customer group's SignalR Group
        await _hubContext.Clients
            .Groups(message.CustomerId.ToString())
            .SendAsync("OrderPaid", new
            {
                message.OrderId,
                message.CustomerId,
                Notification = $"Your order {message.OrderId} has been paid successfully.",
                Timestamp = DateTimeOffset.UtcNow
            });
        await _cacheService.RemoveAsync($"order:{context.Message.OrderId}");
    }
}