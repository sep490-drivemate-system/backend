using MessagingService.Application.Commons.DTOs.Chat;
using MessagingService.Domain.DTOs;
using MessagingService.Domain.Entities;
using SharedLibrary.SharedKernel.Enum;
using SharedLibrary.SharedKernel.ServiceResult;

namespace MessagingService.Application.Interfaces
{
    public interface IChatUseCase
    {
        Task<IEnumerable<ChatSession>> GetUserConversationsAsync(Guid userId);
        Task<IEnumerable<ChatSessionLastMessageDTO>> GetUserChatSessions(Guid userId, UserRole userRole);
        Task<ChatSession?> GetConversationByIdAsync(Guid conversationId);
        Task<IEnumerable<MessageResponseDTO>> GetConversationMessagesAsync(Guid conversationId, Guid userId, int pageNumber = 1, int pageSize = 10);
        Task<ChatSession> CreateConversationAsync(Guid instructorId, Guid noviceDriverId);
        Task<MessageResponseDTO> SaveMessageAsync(SendMessageDTO dto, Guid senderId);
        Task<bool> MarkMessagesAsReadAsync(Guid conversationId, Guid userId);
        Task<int> GetUnreadMessageCountAsync(Guid conversationId, Guid userId);
        Task<Dictionary<Guid, ChatPartnerInfoDTO>> GetPartnerInfoAsync(IEnumerable<Guid> partnerIds, UserRole currentUserRole);
    }
}

