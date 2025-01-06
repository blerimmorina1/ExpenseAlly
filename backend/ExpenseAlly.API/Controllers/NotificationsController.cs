using ExpenseAlly.Application.Common.Models;
using ExpenseAlly.Application.Features.Notifications.Commands;
using ExpenseAlly.Application.Features.Notifications.Dtos;
using ExpenseAlly.Application.Features.Notifications.Queries;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseAlly.API.Controllers;

public class NotificationsController : ApiControllerBase
{
    [HttpGet("getNotifications")]
    public async Task<NotificationsDto> GetNotifications(bool allNotifications = false)
    {
        return await _mediator.Send(new GetNotificationsQuery(allNotifications));
    }

    [HttpPost("markAsRead")]
    public async Task<ResponseDto> MarkNotificationAsRead(MarkNotificationAsReadCommand request)
    {
        return await _mediator.Send(request);
    }
}
