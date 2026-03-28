using Sundarban.Modules.Notifications.Hubs;

namespace Sundarban.Modules.Notifications.Presentation;

public static class NotificationsEndpoints
{
    public static void MapNotificationsEndpoints(this WebApplication app)
    {
        app.MapHub<NotificationsHub>("/hubs/notifications");
    }
}