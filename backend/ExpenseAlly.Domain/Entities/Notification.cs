using ExpenseAlly.Domain.Common;
using ExpenseAlly.Domain.Enums;
namespace ExpenseAlly.Domain.Entities;

public class Notification : BaseAuditableEntity
{
    public NotificationType Type { get; set; }
    public string Title { get; set; }
    public string Message { get; set; }
    public bool IsRead { get; set; } = false;
}
