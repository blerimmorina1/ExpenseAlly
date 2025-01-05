using ExpenseAlly.Domain.Entities;

namespace ExpenseAlly.Application.Common.Interfaces;

public interface INotificationStrategy
{
    Notification GenerateNotification(object entity);
}
