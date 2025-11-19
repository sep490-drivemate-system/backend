using Microsoft.AspNetCore.SignalR;
using SharedLibrary.Jwt;

namespace MessagingService.Infrastructure.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IJwtService _jwtService;

        public ChatHub(IJwtService jwtService)
        {
            _jwtService = jwtService;
        }

        public override async Task OnConnectedAsync()
        {
            // Extract user ID from token (if using JWT in query string)
            var userId = await GetUserIdFromContext();
            
            if (userId.HasValue)
            {
                // Add user to group based on their user ID
                await Groups.AddToGroupAsync(Context.ConnectionId, $"user_{userId.Value}");
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = await GetUserIdFromContext();
            
            if (userId.HasValue)
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"user_{userId.Value}");
            }

            await base.OnDisconnectedAsync(exception);
        }

        // Join a conversation room
        public async Task JoinConversation(string conversationId)
        {
            var userId = await GetUserIdFromContext();
            
            if (userId.HasValue)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, $"conversation_{conversationId}");
            }
        }

        // Leave a conversation room
        public async Task LeaveConversation(string conversationId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"conversation_{conversationId}");
        }

        private async Task<Guid?> GetUserIdFromContext()
        {
            try
            {
                // Try to get token from query string or headers
                var httpContext = Context.GetHttpContext();
                var token = httpContext?.Request.Query["access_token"].ToString() 
                         ?? httpContext?.Request.Headers["Authorization"].ToString()?.Replace("Bearer ", "");

                if (string.IsNullOrEmpty(token))
                {
                    return null;
                }

                var userId = await _jwtService.ExtractUserIdFromToken($"Bearer {token}");
                return userId;
            }
            catch
            {
                return null;
            }
        }
    }
}

