using MessagingService.Application.Commons.DTOs.Chat;
using MessagingService.Application.Interfaces;
using MessagingService.Domain.Interfaces;
using MessagingService.Infrastructure.UoW;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using SharedLibrary.SharedKernel.Enum;
using SharedLibrary.SharedKernel.Http;
using SharedLibrary.SharedKernel.Http.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace MessagingService.Infrastructure.Hubs
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ChatHub : Hub
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly HttpService _httpService;
        private readonly IConfiguration _configuration;
        private readonly IChatUseCase _chatUseCase;

        public ChatHub(IUnitOfWork unitOfWork, HttpService httpService, IConfiguration configuration, IChatUseCase chatUseCase)
        {
            _unitOfWork = unitOfWork;
            _httpService = httpService;
            _configuration = configuration;
            _chatUseCase = chatUseCase;
        }

        public override async Task OnConnectedAsync()
        {
            var userId = GetUserId();
            await Groups.AddToGroupAsync(Context.ConnectionId, BuildGroupName(userId));
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = GetUserId();
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, BuildGroupName(userId));
            await base.OnDisconnectedAsync(exception);
        }

        public async Task<List<ChatSessionsDTO>> GetChatSessions()
        {
            try
            {
                var authenticatedUserId = GetUserId();
                var userRole = GetUserRole();
                // Get all conversations for this user
                var conversations = await _unitOfWork.ConversationRepository
                    .GetUserConversationsAsync(authenticatedUserId);

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

        public async Task<int> GetNumberUnReadMessage(Guid userId)
        {
            var authenticatedUserId = GetUserId();

            var conversations = await _unitOfWork.ConversationRepository.GetUserConversationsAsync(userId);

            if (!conversations.Any())
            {
                return 0;
            }

            var unreadTasks = conversations
                .Select(conversation => _unitOfWork.MessageRepository.GetUnreadMessageCountAsync(conversation.Id, userId));

            var unreadCounts = await Task.WhenAll(unreadTasks);
            return unreadCounts.Sum();
        }

        public async Task<MessageResponseDTO> SendMessage(SendMessageDTO dto)
        {
            var authenticatedUserId = GetUserId();

            // Gửi tin nhắn thông qua UseCase
            var result = await _chatUseCase.SendMessageAsync(dto, authenticatedUserId);

            if (!result.IsSuccess || result.Data == null)
            {
                throw new HubException(result.Error?.Description ?? "Failed to send message.");
            }

            // Lấy thông tin conversation để xác định người nhận
            var conversation = await _unitOfWork.ConversationRepository
                .GetConversationByIdAsync(dto.ConversationId);

            if (conversation == null)
            {
                throw new HubException("Conversation not found.");
            }

            // Xác định người nhận (người còn lại trong conversation)
            var recipientId = conversation.InstructorId == authenticatedUserId
                ? conversation.NoviceDriverId
                : conversation.InstructorId;

            // Gửi tin nhắn trực tiếp đến người nhận (chat 1-1)
            // Group vẫn cần thiết để gửi đến tất cả thiết bị của người nhận
            // Ví dụ: người nhận mở chat trên cả điện thoại và máy tính
            await Clients.Group(BuildGroupName(recipientId))
                .SendAsync("ReceiveMessage", result.Data);

            // Gửi lại cho người gửi để confirm
            await Clients.Caller.SendAsync("MessageSent", result.Data);

            return result.Data;
        }

        public async Task<List<MessageResponseDTO>> GetMessages(Guid conversationId, int pageNumber = 1, int pageSize = 50)
        {
            var authenticatedUserId = GetUserId();

            // Kiểm tra user có phải là participant không
            var isParticipant = await _unitOfWork.ConversationRepository
                .IsUserParticipantAsync(conversationId, authenticatedUserId);

            if (!isParticipant)
            {
                throw new HubException("You are not a participant in this conversation.");
            }

            // Lấy tin nhắn thông qua UseCase
            var result = await _chatUseCase.GetConversationMessagesAsync(
                conversationId,
                authenticatedUserId,
                pageNumber,
                pageSize);

            if (!result.IsSuccess)
            {
                throw new HubException(result.Error?.Description ?? "Failed to get messages.");
            }

            return result.Data?.ToList() ?? new List<MessageResponseDTO>();
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

        private Guid GetUserId()
        {
            var idClaim = Context.User?.Claims.FirstOrDefault(c => c.Type == "id");
            return idClaim != null && Guid.TryParse(idClaim.Value, out var userId) ? userId : Guid.Empty;
        }
        private UserRole GetUserRole()
        {
            var roleClaim = Context.User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role || c.Type == "role");
            return roleClaim != null && Enum.TryParse<UserRole>(roleClaim.Value, out var role) ? role : UserRole.Demo;
        }
        public static string BuildGroupName(Guid userId) => $"user_{userId}";
    }
}

