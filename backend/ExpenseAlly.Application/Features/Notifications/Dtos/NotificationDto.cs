namespace ExpenseAlly.Application.Features.Notifications.Dtos;

public class NotificationDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Message { get; set; }
    public DateTime CreatedOn { get; set; }
    public bool IsRead { get; set; }
}
