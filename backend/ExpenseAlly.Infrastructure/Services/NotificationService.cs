using ExpenseAlly.Application.Common.Factories;
using ExpenseAlly.Domain.Entities;
using ExpenseAlly.Domain.Enums;
using Microsoft.AspNetCore.SignalR;

public class NotificationService : INotificationService
{
    private readonly NotificationStrategyFactory _factory;
    private readonly IApplicationDbContext _context;
    private readonly IHubContext<NotificationHub> _notificationHubContext;

    public NotificationService(IApplicationDbContext context, IHubContext<NotificationHub> notificationHubContext)
    {
        _factory = new NotificationStrategyFactory();
        _context = context;
        _notificationHubContext = notificationHubContext;
    }

    public async Task SendNotificationAsync(NotificationType type, object entity, CancellationToken cancellationToken)
    {
        var strategy = _factory.GetStrategy(type);
        var notification = strategy.GenerateNotification(entity);

        if (notification != null)
        {
            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync(cancellationToken);

            await NotifyUser(notification.CreatedBy.ToString(), notification);
        }
    }

    private async Task NotifyUser(string userId, Notification notification)
    {
        await _notificationHubContext.Clients.User(userId)
            .SendAsync("ReceiveNotification", new
            {
                notification.Id,
                notification.Title,
                notification.Message,
                notification.CreatedOn
            });
    }
}
