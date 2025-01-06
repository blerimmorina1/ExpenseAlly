using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

public class NotificationHub : Hub
{
    private static readonly Dictionary<string, string> UserConnections = new();
    private readonly IHttpContextAccessor _httpContextAccessor;

    public NotificationHub(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public override Task OnConnectedAsync()
    {
        var userId = Context.UserIdentifier;

        if (!string.IsNullOrEmpty(userId))
        {
            UserConnections[userId] = Context.ConnectionId;
        }

        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception exception)
    {
        var userId = Context.UserIdentifier;

        
        if (!string.IsNullOrEmpty(userId))
        {
            UserConnections.Remove(userId); 
        }

        return base.OnDisconnectedAsync(exception);
    }

    public async Task SendNotification(string userId, object notification)
    {
        if (UserConnections.ContainsKey(userId))
        {
            await Clients.Client(UserConnections[userId]).SendAsync("ReceiveNotification", notification);
        }
    }
}
