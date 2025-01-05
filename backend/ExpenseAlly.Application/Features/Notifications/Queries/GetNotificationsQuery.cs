using ExpenseAlly.Application.Common.Interfaces;
using ExpenseAlly.Application.Features.Notifications.Dtos;

namespace ExpenseAlly.Application.Features.Notifications.Queries
{
    public class GetNotificationsQuery : IRequest<NotificationsDto>
    {
        public bool AllNotifications { get; set; }

        public GetNotificationsQuery(bool allNotifications)
        {
            AllNotifications = allNotifications;
        }
    }

    public class GetNotificationsQueryHandler : IRequestHandler<GetNotificationsQuery, NotificationsDto>
    {
        private readonly IApplicationDbContext _context;

        public GetNotificationsQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<NotificationsDto> Handle(GetNotificationsQuery request, CancellationToken cancellationToken)
        {
            var notifications = await _context.Notifications.Where(n => request.AllNotifications? true : n.IsRead == false).OrderByDescending(n => n.CreatedOn).ToListAsync(cancellationToken);


            var notificationsDto = new NotificationsDto
            {
                Success = true,
                Notifications = notifications.Select(n => new NotificationDto
                {
                    Id = n.Id,
                    Title = n.Title,
                    Message = n.Message,
                    CreatedOn = n.CreatedOn,
                    IsRead = n.IsRead
                }).ToList()
            }; 

            return notificationsDto;
        }
    }
}
