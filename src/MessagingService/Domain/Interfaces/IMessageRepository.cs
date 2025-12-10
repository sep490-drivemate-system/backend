using MessagingService.Domain.Entities;

namespace MessagingService.Domain.Interfaces
{
    public interface IMessageRepository : IGenericRepository<Message>
    {
        Task<IEnumerable<Message>> GetConversationMessagesAsync(Guid conversationId, int pageNumber = 1, int pageSize = 50);
        Task<int> GetUnreadMessageCountAsync(Guid conversationId, Guid userId);
        Task<int> GetTotalUnreadMessageCountAsync(IEnumerable<Guid> conversationIds, Guid userId);
        Task MarkMessagesAsReadAsync(Guid conversationId, Guid userId);
    }
}

