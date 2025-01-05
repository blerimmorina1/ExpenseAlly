using ExpenseAlly.Application.Common.Interfaces;
using ExpenseAlly.Application.Common.Models;
using Microsoft.Extensions.Logging;

namespace ExpenseAlly.Application.Features.Notifications.Commands;

public class MarkNotificationAsReadCommand : IRequest<ResponseDto>
{
    public Guid Id { get; set; }
    public bool Read { get; set; }

}

public class MarkNotificationAsReadCommandHandler : IRequestHandler<MarkNotificationAsReadCommand, ResponseDto>
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<MarkNotificationAsReadCommandHandler> _logger;

    public MarkNotificationAsReadCommandHandler(IApplicationDbContext context, ILogger<MarkNotificationAsReadCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<ResponseDto> Handle(MarkNotificationAsReadCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var notification = await _context.Notifications.FirstOrDefaultAsync(x => x.Id == request.Id);

            if (notification == null)
            {
                return new ResponseDto
                {
                    Success = false,
                    Errors = new List<ErrorDto>
                    {
                        new ErrorDto { Code = "NotFound", Message = "Notification with the provided ID doesn't exist." }
                    }
                };

            }

            notification.IsRead = request.Read;
            _context.Notifications.Update(notification);

            await _context.SaveChangesAsync(cancellationToken);
            return new ResponseDto
            {
                Success = true
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while editing the notification.");
            throw;
        }
    }
}
