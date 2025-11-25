using AutoMapper;
using MessagingService.Application.Interfaces;
using MessagingService.Domain.Entities;
using MessagingService.Domain.Enum;
using MessagingService.Infrastructure.UoW;
using SharedLibrary.SharedKernel.ServiceResult;
using SharedLibrary.SharedKernel.Http;
using SharedLibrary.SharedKernel.Http.DTOs.User;
using Microsoft.Extensions.Configuration;
using MessagingService.Application.Commons.DTOs.Chat;

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

        public async Task<Result<ConversationResponseDTO>> GetOrCreateConversationAsync(Guid userId1, Guid userId2)
        {
            // Determine which user is instructor and which is novice driver
            // This logic might need to be adjusted based on your business rules
            // For now, we'll assume userId1 is instructor and userId2 is novice driver
            // You may want to check user roles from UserService
            
            var instructorId = userId1;
            var noviceDriverId = userId2;

            // Check if conversation already exists
            var existingConversation = await _unitOfWork.ConversationRepository
                .GetConversationByParticipantsAsync(instructorId, noviceDriverId);

            if (existingConversation != null)
            {
                return await MapConversationToDTO(existingConversation, userId1);
            }

            // Create new conversation
            var conversation = new ChatSession
            {
                InstructorId = instructorId,
                NoviceDriverId = noviceDriverId,
                LastModifiedAt = DateTime.UtcNow
            };

            await _unitOfWork.ConversationRepository.AddAsync(conversation);
            await _unitOfWork.CommitChangesAsync();

            return await MapConversationToDTO(conversation, userId1);
        }

        public async Task<Result<IEnumerable<ConversationResponseDTO>>> GetUserConversationsAsync(Guid userId)
        {
            var conversations = await _unitOfWork.ConversationRepository
                .GetUserConversationsAsync(userId);

            var conversationDTOs = new List<ConversationResponseDTO>();

            foreach (var conversation in conversations)
            {
                var dto = await MapConversationToDTO(conversation, userId);
                //if (dto.IsSuccess)
                //{
                //    conversationDTOs.Add(dto.Value);
                //}
            }

            return Result<IEnumerable<ConversationResponseDTO>>.Success(conversationDTOs);
        }

        public async Task<Result<IEnumerable<MessageResponseDTO>>> GetConversationMessagesAsync(
            Guid conversationId, Guid userId, int pageNumber = 1, int pageSize = 50)
        {
            // Verify user is participant
            var isParticipant = await _unitOfWork.ConversationRepository
                .IsUserParticipantAsync(conversationId, userId);

            if (!isParticipant)
            {
                return Result<IEnumerable<MessageResponseDTO>>.Failure(
                    ServiceError.ForbiddenError("You are not a participant in this conversation"));
            }

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

            return Result<IEnumerable<MessageResponseDTO>>.Success(messageDTOs);
        }

        public async Task<Result<MessageResponseDTO>> SendMessageAsync(SendMessageDTO dto, Guid senderId)
        {
            // Verify user is participant
            var isParticipant = await _unitOfWork.ConversationRepository
                .IsUserParticipantAsync(dto.ConversationId, senderId);

            if (!isParticipant)
            {
                return Result<MessageResponseDTO>.Failure(
                    ServiceError.ForbiddenError("You are not a participant in this conversation"));
            }

            var message = new Message
            {
                ChatSessionId = dto.ConversationId,
                SenderId = senderId,
                Content = dto.Content,
                Status = MessageStatus.Sent,
                LastModifiedAt = DateTime.UtcNow
            };

            await _unitOfWork.MessageRepository.AddAsync(message);

            // Update conversation last modified
            var conversation = await _unitOfWork.ConversationRepository
                .GetConversationByIdAsync(dto.ConversationId);

            if (conversation != null)
            {
                conversation.LastModifiedAt = DateTime.UtcNow;
                await _unitOfWork.ConversationRepository.UpdateAsync(conversation);
            }

            await _unitOfWork.CommitChangesAsync();

            // Get user info for response
            var users = await GetUsersByIdsAsync(new[] { senderId });
            var senderInfo = users?.FirstOrDefault();

            var messageDTO = new MessageResponseDTO
            {
                Id = message.Id,
                Content = message.Content,
                SenderId = message.SenderId,
            //    SenderName = senderInfo?.Fullname,
                SenderAvatar = senderInfo?.AvatarUrl,
                CreatedAt = message.CreatedAt
            };

            return Result<MessageResponseDTO>.Success(messageDTO);
        }

        public async Task<Result<bool>> MarkMessagesAsReadAsync(Guid conversationId, Guid userId)
        {
            var isParticipant = await _unitOfWork.ConversationRepository
                .IsUserParticipantAsync(conversationId, userId);

            if (!isParticipant)
            {
                return Result<bool>.Failure(
                    ServiceError.ForbiddenError("You are not a participant in this conversation"));
            }

            await _unitOfWork.MessageRepository.MarkMessagesAsReadAsync(conversationId, userId);
            await _unitOfWork.CommitChangesAsync();

            return Result<bool>.Success(true);
        }

        public async Task<Result<int>> GetUnreadMessageCountAsync(Guid conversationId, Guid userId)
        {
            var isParticipant = await _unitOfWork.ConversationRepository
                .IsUserParticipantAsync(conversationId, userId);

            if (!isParticipant)
            {
                return Result<int>.Failure(
                    ServiceError.ForbiddenError("You are not a participant in this conversation"));
            }

            var count = await _unitOfWork.MessageRepository
                .GetUnreadMessageCountAsync(conversationId, userId);

            return Result<int>.Success(count);
        }

        private async Task<Result<ConversationResponseDTO>> MapConversationToDTO(ChatSession conversation, Guid currentUserId)
        {
            var dto = new ConversationResponseDTO
            {
                Id = conversation.Id,
                IsGroupChat = false // Always false for ChatSession
            };

            // Get unread count
            var unreadCount = await _unitOfWork.MessageRepository
                .GetUnreadMessageCountAsync(conversation.Id, currentUserId);
            dto.UnreadCount = unreadCount;

            // Get the other participant info
            var otherUserId = conversation.InstructorId == currentUserId 
                ? conversation.NoviceDriverId 
                : conversation.InstructorId;

            var users = await GetUsersByIdsAsync(new[] { otherUserId });
            if (users != null && users.Any())
            {
                var otherUser = users.First();
                dto.Participants = new List<ParticipantDTO>
                {
                    new ParticipantDTO
                    {
                        UserId = otherUser.UserId,
                      //  UserName = otherUser.Fullname,
                        Avatar = otherUser.AvatarUrl
                    }
                };
            }

            // Get last message if exists
            var lastMessage = conversation.Messages?
                .Where(m => !m.IsDeleted)
                .OrderByDescending(m => m.CreatedAt)
                .FirstOrDefault();

            if (lastMessage != null)
            {
                dto.LastMessage = lastMessage.Content;
                dto.LastMessageAt = lastMessage.CreatedAt;
            }

            return Result<ConversationResponseDTO>.Success(dto);
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
