using ExpenseAlly.Application.Common.Models;

namespace ExpenseAlly.Application.Features.Notifications.Dtos;

public class NotificationsDto : ResponseDto
{
    public List<NotificationDto> Notifications { get; set; }
}
