using MessagingService.Application.DTOs.Chat;
using MessagingService.Domain.Interfaces;
using MessagingService.Infrastructure.UoW;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using SharedLibrary.SharedKernel.Enum;
using SharedLibrary.SharedKernel.Http;
using SharedLibrary.SharedKernel.Http.DTOs.User;

namespace MessagingService.Infrastructure.Hubs
{
    [AllowAnonymous]
    public class ChatHub : Hub
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly HttpService _httpService;
        private readonly IConfiguration _configuration;

        public ChatHub(IUnitOfWork unitOfWork, HttpService httpService, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _httpService = httpService;
            _configuration = configuration;
        }

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var userIdParam = httpContext?.Request.Query["userId"].ToString();

            if (!string.IsNullOrEmpty(userIdParam) && Guid.TryParse(userIdParam, out var userId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, $"user_{userId}");
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var httpContext = Context.GetHttpContext();
            var userIdParam = httpContext?.Request.Query["userId"].ToString();

            if (!string.IsNullOrEmpty(userIdParam) && Guid.TryParse(userIdParam, out var userId))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"user_{userId}");
            }

            await base.OnDisconnectedAsync(exception);
        }

        public async Task<List<ChatSessionsDTO>> GetChatSessions(Guid userId, UserRole userRole)
        {
            try
            {
                // Get all conversations for this user
                var conversations = await _unitOfWork.ConversationRepository
                    .GetUserConversationsAsync(userId);

                if (!conversations.Any())
                {
                    return new List<ChatSessionsDTO>();
                }

                // Get all other user IDs (the users this person is chatting with)
                var otherUserIds = conversations
                    .Select(c => userRole == UserRole.Instructor 
                        ? c.NoviceDriverId 
                        : c.InstructorId)
                    .Distinct()
                    .ToList();

                // Get user info from UserService
                var users = await GetUsersByIdsAsync(otherUserIds);
                var userDict = users?.ToDictionary(u => u.UserId, u => u) 
                    ?? new Dictionary<Guid, UserDetailDTO>();

                // Map to ChatSessionsDTO
                var sessions = new List<ChatSessionsDTO>();

                foreach (var conversation in conversations)
                {
                    var otherUserId = userRole == UserRole.Instructor 
                        ? conversation.NoviceDriverId 
                        : conversation.InstructorId;

                    userDict.TryGetValue(otherUserId, out var otherUser);

                    // Get last message
                    var lastMessage = conversation.Messages?
                        .Where(m => !m.IsDeleted)
                        .OrderByDescending(m => m.CreatedAt)
                        .FirstOrDefault();

                    var sessionDTO = new ChatSessionsDTO
                    {
                        Id = conversation.Id,
                        ToUserId = otherUserId,
                        LastMessage = lastMessage?.Content ?? "",
                        LastModifiedAt = conversation.LastModifiedAt,
                        ToUserAvatar = otherUser?.AvatarUrl ?? "",
                        ToUserFullName = otherUser?.FullName ?? ""
                    };

                    sessions.Add(sessionDTO);
                }

                // Sort by LastModifiedAt descending (most recent first)
                return sessions.OrderByDescending(s => s.LastModifiedAt).ToList();
            }
            catch (Exception)
            {
                return new List<ChatSessionsDTO>();
            }
        }

        private async Task<List<UserDetailDTO>?> GetUsersByIdsAsync(IEnumerable<Guid> userIds)
        {
            try
            {
                var userServiceUrl = _configuration["USERSERVICE:URL"] 
                    ?? _configuration.GetConnectionString("Userservice_connection")
                    ?? "http://localhost:5100";
                
                var url = $"{userServiceUrl}/api/users/ids";
                var result = await _httpService.PostAsync<List<Guid>, List<UserDetailDTO>>(url, userIds.ToList());
                return result;
            }
            catch
            {
                return null;
            }
        }
    }
}

