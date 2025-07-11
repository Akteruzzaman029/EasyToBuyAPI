using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
namespace Persistence.Repository
{
    [Authorize] // <-- add this
    public class NotificationHub : Hub
    {
        // In-memory storage for connected users (Thread-safe)
        private static readonly ConcurrentDictionary<string, string> ConnectedUsers = new ConcurrentDictionary<string, string>();


        public override async Task OnConnectedAsync()
        {
            var userId = Context.UserIdentifier; // ASP.NET Core Identity User ID
            if (!string.IsNullOrEmpty(userId))
            {
                ConnectedUsers.TryAdd(Context.ConnectionId, userId);
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            ConnectedUsers.TryRemove(Context.ConnectionId, out _);

            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendNotification(string userId, string message)
        {
            await Clients.User(userId).SendAsync("ReceiveNotification", message);
        }
      
    }
}
