using MessagingService.Application.Commons.DTOs.Chat;
using MessagingService.Application.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SharedLibrary.SharedKernel.Enum;
using SharedLibrary.SharedKernel.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using Twilio.TwiML.Voice;

namespace MessagingService.Infrastructure.Hubs
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ChatHub(IChatUseCase chatUseCase,IUserClaimsAccessor userClaimsAccessor) : Hub
    {
        private readonly IChatUseCase _chatUseCase = chatUseCase;
        private readonly IUserClaimsAccessor _userClaimsAccessor = userClaimsAccessor;

        public override async System.Threading.Tasks.Task OnConnectedAsync()
        {
            var userId = _userClaimsAccessor.GetUserId(Context.User);

            if (userId == Guid.Empty)
            {
                Context.Abort();
                return;
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, BuildGroupName(userId));
            await base.OnConnectedAsync();
        }

        public override async System.Threading.Tasks.Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = _userClaimsAccessor.GetUserId(Context.User);

            if (userId != Guid.Empty)
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, BuildGroupName(userId));
            }

            await base.OnDisconnectedAsync(exception);
        }

        public async Task<List<ChatSessionsDTO>> GetChatSessions()
        {
            var userid = _userClaimsAccessor.GetUserId(Context.User);
            var userRole = _userClaimsAccessor.GetUserRole(Context.User);

            var conversationSummaries = await _chatUseCase.GetUserChatSessions(userid,userRole);

            if (!conversationSummaries.Any())
            {
                return new List<ChatSessionsDTO>();
            }

            var partnerIds = conversationSummaries
                .Select(c => c.PartnerId)
                .Distinct()
                .ToList();

            var partnerInfos = await _chatUseCase.GetPartnerInfoAsync(partnerIds, userRole);

            var sessions = new List<ChatSessionsDTO>();

            foreach (var conversation in conversationSummaries)
            {
                partnerInfos.TryGetValue(conversation.PartnerId, out var partnerInfo);

                var sessionDTO = new ChatSessionsDTO
                {
                    Id = conversation.Id,
                    ToUserId = conversation.PartnerId,
                    LastMessage = conversation.LastMessage,
                    LastModifiedAt = conversation.LastMessageAt ?? conversation.LastModifiedAt,
                    Status = conversation.Status,
                    ToUserAvatar = partnerInfo?.AvatarUrl,
                    ToUserFullName = partnerInfo?.FullName
                };
                sessions.Add(sessionDTO);
            }

            return sessions.OrderByDescending(s => s.LastModifiedAt).ToList();
        }

        public async Task<int> GetNumberUnReadMessage()
        {
            var userId = _userClaimsAccessor.GetUserId(Context.User);

            var conversations = await _chatUseCase.GetUserConversationsAsync(userId);

            if (!conversations.Any())
            {
                return 0;
            }

            var conversationIds = conversations.Select(c => c.Id);
            return await _chatUseCase.GetTotalUnreadMessageCountAsync(conversationIds, userId);
        }

        public async System.Threading.Tasks.Task SendMessage(SendMessageDTO dto)
        {
            var userId = _userClaimsAccessor.GetUserId(Context.User);
            var userRole = _userClaimsAccessor.GetUserRole(Context.User);

            Domain.Entities.ChatSession? conversation = null;
            if (dto.ConversationId == null || dto.ConversationId == Guid.Empty)
            {
                var instructorId = userRole == UserRole.Instructor ? userId : dto.RecipientId;
                var noviceDriverId = userRole == UserRole.Instructor ? dto.RecipientId : userId;

                conversation = await _chatUseCase.CreateConversationAsync(instructorId, noviceDriverId);
                dto.ConversationId = conversation.Id;
            }
            else
            {
                conversation = await _chatUseCase.GetConversationByIdAsync(dto.ConversationId.Value);
                if (conversation == null)
                {
                    return; 
                }
            }
            var result = await _chatUseCase.SaveMessageAsync(dto, userId);

            var recipientId = conversation.InstructorId == userId
                ? conversation.NoviceDriverId
                : conversation.InstructorId;

            await Clients.Group(BuildGroupName(recipientId))
                .SendAsync("ReceiveMessage", result);

            await Clients.Caller.SendAsync("MessageSent", result);
        }

        public async Task<List<MessageResponseDTO>> GetMessages(Guid conversationId, int pageNumber = 1, int pageSize = 10)
        {
            var userId = _userClaimsAccessor.GetUserId(Context.User);

            var result = await _chatUseCase.GetConversationMessagesAsync(
                conversationId,
                userId,
                pageNumber,
                pageSize);


            return result.ToList();
        }

        public async Task<bool> MarkMessagesAsRead(Guid conversationId)
        {
            var userId = _userClaimsAccessor.GetUserId(Context.User);

            var result = await _chatUseCase.MarkMessagesAsReadAsync(conversationId, userId);

            return result;
        }

        public static string BuildGroupName(Guid userId) => $"user_{userId}";
    }
}

