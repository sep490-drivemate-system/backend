using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace MessagingService.Infrastructure.Hubs
{
    [AllowAnonymous]
    public class ChatHub : Hub
    {

        public override async Task OnConnectedAsync()
        {
            // Get user ID from query string if provided (optional)
            var httpContext = Context.GetHttpContext();
            var userIdParam = httpContext?.Request.Query["userId"].ToString();
            
            if (!string.IsNullOrEmpty(userIdParam) && Guid.TryParse(userIdParam, out var userId))
            {
                // Add user to group based on their user ID
                await Groups.AddToGroupAsync(Context.ConnectionId, $"user_{userId}");
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            // Get user ID from query string if provided (optional)
            var httpContext = Context.GetHttpContext();
            var userIdParam = httpContext?.Request.Query["userId"].ToString();
            
            if (!string.IsNullOrEmpty(userIdParam) && Guid.TryParse(userIdParam, out var userId))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"user_{userId}");
            }

            await base.OnDisconnectedAsync(exception);
        }

        // Join a conversation room
        public async Task JoinConversation(string conversationId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"conversation_{conversationId}");
        }

        // Leave a conversation room
        public async Task LeaveConversation(string conversationId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"conversation_{conversationId}");
        }
    }
}

