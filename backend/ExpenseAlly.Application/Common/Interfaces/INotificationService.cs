using ExpenseAlly.Domain.Enums;

namespace ExpenseAlly.Application.Common.Interfaces;

public interface INotificationService
{
    Task SendNotificationAsync(NotificationType type, object entity, CancellationToken cancellationToken);
}
