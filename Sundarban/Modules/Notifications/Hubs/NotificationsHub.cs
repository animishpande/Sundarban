using Microsoft.AspNetCore.SignalR;

namespace Sundarban.Modules.Notifications.Hubs;

public class NotificationsHub : Hub
{
    public async Task JoinCustomerGroup(string customerId)
    {
        // Clients connect to the notifications group based on their customer ID
        // Only customers with valid customer ID will receive the notification
        await Groups.AddToGroupAsync(Context.ConnectionId, customerId);
    }
}