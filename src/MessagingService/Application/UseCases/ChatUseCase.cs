using AutoMapper;
using MessagingService.Application.Commons.DTOs.Chat;
using MessagingService.Application.Interfaces;
using MessagingService.Domain.DTOs;
using MessagingService.Domain.Entities;
using MessagingService.Domain.Enum;
using MessagingService.Infrastructure.UoW;
using Microsoft.Extensions.Configuration;
using SharedLibrary.SharedKernel.Enum;
using SharedLibrary.SharedKernel.Http;
using SharedLibrary.SharedKernel.Http.DTOs.User;
using SharedLibrary.SharedKernel.ServiceResult;
using Twilio.TwiML.Voice;

namespace MessagingService.Application.UseCases
{
    public class ChatUseCase : IChatUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly HttpService _httpService;
        private readonly IConfiguration _configuration;

        public ChatUseCase(IUnitOfWork unitOfWork, IMapper mapper, HttpService httpService, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpService = httpService;
            _configuration = configuration;
        }

        public async Task<ChatSession> CreateConversationAsync(Guid instructorId, Guid noviceDriverId)
        {
            var createSessionDto = new CreateChatSessionDTO
            {
                InstructorId = instructorId,
                NoviceDriverId = noviceDriverId,
                LastModifiedAt = DateTime.Now
            };

            var conversation = _mapper.Map<ChatSession>(createSessionDto);

            await _unitOfWork.ChatSessionRepository.AddAsync(conversation);
            await _unitOfWork.CommitChangesAsync();

            return conversation;
        }

        public async Task<IEnumerable<ChatSession>> GetUserConversationsAsync(Guid userId)
        {
            var conversations = await _unitOfWork.ChatSessionRepository
                .GetUserConversationsAsync(userId);

            var conversationList = conversations.ToList();

            return conversationList;
        }

        public async Task<IEnumerable<MessageResponseDTO>> GetConversationMessagesAsync(
            Guid conversationId, Guid userId, int pageNumber = 1, int pageSize = 50)
        {
            var isParticipant = await _unitOfWork.ChatSessionRepository
                .IsUserParticipantAsync(conversationId, userId);

            var messages = await _unitOfWork.MessageRepository
                .GetConversationMessagesAsync(conversationId, pageNumber, pageSize);

            var messageDTOs = new List<MessageResponseDTO>();

            // Get user info for all senders
            var senderIds = messages.Select(m => m.SenderId).Distinct().ToList();
            var users = await GetUsersByIdsAsync(senderIds);
            var userDict = users?.ToDictionary(u => u.UserId, u => u) ?? new Dictionary<Guid, UserDetailDTO>();

            foreach (var message in messages)
            {
                userDict.TryGetValue(message.SenderId, out var senderInfo);

                var dto = new MessageResponseDTO
                {
                    Id = message.Id,
                    Content = message.Content,
                    SenderId = message.SenderId,
                  //  SenderName = senderInfo?.Fullname,
                    SenderAvatar = senderInfo?.AvatarUrl,
                    CreatedAt = message.CreatedAt
                };

                messageDTOs.Add(dto);
            }

            return messageDTOs;
        }

        public async Task<MessageResponseDTO> SaveMessageAsync(SendMessageDTO dto, Guid senderId)
        {
            if (dto.ConversationId == null || dto.ConversationId == Guid.Empty)
            {
                throw new ArgumentException("ConversationId is required", nameof(dto));
            }

            var message = _mapper.Map<Message>(dto);
            message.SenderId = senderId;
            message.LastModifiedAt = DateTime.Now;

            await _unitOfWork.MessageRepository.AddAsync(message);
            
            var conversation = await _unitOfWork.ChatSessionRepository
                .GetConversationByIdAsync(dto.ConversationId.Value);

            if (conversation != null)
            {
                conversation.LastModifiedAt = message.LastModifiedAt;
                await _unitOfWork.ChatSessionRepository.UpdateAsync(conversation);
            }

            await _unitOfWork.CommitChangesAsync();

            var users = await GetUsersByIdsAsync(new[] { senderId });
            var senderInfo = users?.FirstOrDefault();

            var response = new MessageResponseDTO
            {
                Id = message.Id,
                Content = message.Content,
                SenderId = message.SenderId,
                SenderAvatar = senderInfo?.AvatarUrl,
                CreatedAt = message.CreatedAt
            };

            return response;
        }

        public async Task<bool> MarkMessagesAsReadAsync(Guid conversationId, Guid userId)
        {
            var isParticipant = await _unitOfWork.ChatSessionRepository
                .IsUserParticipantAsync(conversationId, userId);


            await _unitOfWork.MessageRepository.MarkMessagesAsReadAsync(conversationId, userId);
            await _unitOfWork.CommitChangesAsync();

            return true;
        }

        public async Task<int> GetUnreadMessageCountAsync(Guid conversationId, Guid userId) => 
            await _unitOfWork.MessageRepository.GetUnreadMessageCountAsync(conversationId, userId);

        public async Task<int> GetTotalUnreadMessageCountAsync(IEnumerable<Guid> conversationIds, Guid userId) =>
            await _unitOfWork.MessageRepository.GetTotalUnreadMessageCountAsync(conversationIds, userId);

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

        public async Task<ChatSession?> GetConversationByIdAsync(Guid conversationId)
        {
            return await _unitOfWork.ChatSessionRepository.GetConversationByIdAsync(conversationId);
        }

        public async Task<IEnumerable<ChatSessionLastMessageDTO>> GetUserChatSessions(Guid userId, UserRole userRole)
        {
            return await _unitOfWork.ChatSessionRepository.GetUserChatSessionsAsync(userId,userRole);
        }

        public async Task<Dictionary<Guid, ChatPartnerInfoDTO>> GetPartnerInfoAsync(
            IEnumerable<Guid> partnerIds,
            UserRole currentUserRole)
        {
            var ids = partnerIds
                .Where(id => id != Guid.Empty)
                .Distinct()
                .ToList();

            if (!ids.Any())
            {
                return new Dictionary<Guid, ChatPartnerInfoDTO>();
            }

            var userServiceUrl = _configuration["USERSERVICE:URL"]
                ?? _configuration.GetConnectionString("Userservice_connection")
                ?? "http://localhost:5100";

            var requestingNoviceInfo = currentUserRole == UserRole.Instructor;
            var endpoint = requestingNoviceInfo
                ? "batch-novice-driver-info"
                : "batch-instructor-info";

            var url = $"{userServiceUrl}/api/users/{endpoint}";

            try
            {
                if (requestingNoviceInfo)
                {
                    var noviceResult =
                        await _httpService.PostAsync<List<Guid>, Dictionary<Guid, NoviceDriverBasicInfoDTO>>(url, ids)
                        ?? new Dictionary<Guid, NoviceDriverBasicInfoDTO>();

                    return noviceResult.ToDictionary(
                        kvp => kvp.Key,
                        kvp => new ChatPartnerInfoDTO
                        {
                            FullName = kvp.Value.Fullname,
                            AvatarUrl = kvp.Value.AvatarUrl
                        });
                }

                var instructorResult =
                    await _httpService.PostAsync<List<Guid>, Dictionary<Guid, InstructorBasicInfoDTO>>(url, ids)
                    ?? new Dictionary<Guid, InstructorBasicInfoDTO>();

                return instructorResult.ToDictionary(
                    kvp => kvp.Key,
                    kvp => new ChatPartnerInfoDTO
                    {
                        FullName = kvp.Value.Fullname,
                        AvatarUrl = kvp.Value.AvatarUrl
                    });
            }
            catch
            {
                return new Dictionary<Guid, ChatPartnerInfoDTO>();
            }
        }
    }
}
