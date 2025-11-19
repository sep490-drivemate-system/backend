using MessagingService.Domain.Entities;

namespace MessagingService.Domain.Interfaces
{
    public interface IConversationRepository : IGenericRepository<ChatSession>
    {
        Task<ChatSession?> GetConversationByIdAsync(Guid conversationId);
        Task<ChatSession?> GetConversationByParticipantsAsync(Guid instructorId, Guid noviceDriverId);
        Task<IEnumerable<ChatSession>> GetUserConversationsAsync(Guid userId);
        Task<bool> IsUserParticipantAsync(Guid conversationId, Guid userId);
    }
}

