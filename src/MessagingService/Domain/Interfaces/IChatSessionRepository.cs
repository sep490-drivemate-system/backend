using MessagingService.Domain.DTOs;
using MessagingService.Domain.Entities;
using SharedLibrary.SharedKernel.Enum;

namespace MessagingService.Domain.Interfaces
{
    public interface IChatSessionRepository : IGenericRepository<ChatSession>
    {
        Task<ChatSession?> GetConversationByIdAsync(Guid conversationId);
        Task<ChatSession?> GetConversationByParticipantsAsync(Guid instructorId, Guid noviceDriverId);
        Task<IEnumerable<ChatSession>> GetUserConversationsAsync(Guid userId);
        Task<IEnumerable<ChatSessionLastMessageDTO>> GetUserChatSessionsAsync(Guid userId, UserRole userRole);
        Task<bool> IsUserParticipantAsync(Guid conversationId, Guid userId);
    }
}

