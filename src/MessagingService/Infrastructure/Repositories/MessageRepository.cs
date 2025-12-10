using MessagingService.Domain.Entities;
using MessagingService.Domain.Interfaces;
using MessagingService.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace MessagingService.Infrastructure.Repositories
{
    public class MessageRepository : GenericRepository<Message>, IMessageRepository
    {
        public MessageRepository(MessagingDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Message>> GetConversationMessagesAsync(Guid conversationId, int pageNumber = 1, int pageSize = 50)
        {
            return await _dbSet
                .Where(m => m.ChatSessionId == conversationId && !m.IsDeleted)
                .OrderByDescending(m => m.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .OrderBy(m => m.CreatedAt) // Reverse order for display
                .ToListAsync();
        }

        public async Task<int> GetUnreadMessageCountAsync(Guid conversationId, Guid userId)
        {
            return await _dbSet
                .CountAsync(m => m.ChatSessionId == conversationId && 
                               m.SenderId != userId && 
                               m.Status == Domain.Enum.MessageStatus.Sent &&
                               !m.IsDeleted);
        }

        public async Task<int> GetTotalUnreadMessageCountAsync(IEnumerable<Guid> conversationIds, Guid userId)
        {
            var conversationIdsList = conversationIds.ToList();
            if (!conversationIdsList.Any())
            {
                return 0;
            }

            return await _dbSet
                .CountAsync(m => conversationIdsList.Contains(m.ChatSessionId) && 
                               m.SenderId != userId && 
                               m.Status == Domain.Enum.MessageStatus.Sent &&
                               !m.IsDeleted);
        }

        public async Task MarkMessagesAsReadAsync(Guid conversationId, Guid userId)
        {
            // Update all messages in this conversation from other users to "Read" status
            var messages = await _dbSet
                .Where(m => m.ChatSessionId == conversationId && 
                           m.SenderId != userId && 
                           m.Status == Domain.Enum.MessageStatus.Sent &&
                           !m.IsDeleted)
                .ToListAsync();

            foreach (var message in messages)
            {
                message.Status = Domain.Enum.MessageStatus.Read;
                message.LastModifiedAt = DateTime.Now;
            }

            if (messages.Any())
            {
                _dbSet.UpdateRange(messages);
            }
        }
    }
}

