using MessagingService.Domain.Entities;
using MessagingService.Domain.Interfaces;
using MessagingService.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace MessagingService.Infrastructure.Repositories
{
    public class ConversationRepository : GenericRepository<ChatSession>, IConversationRepository
    {
        public ConversationRepository(MessagingDbContext context) : base(context)
        {
        }

        public async Task<ChatSession?> GetConversationByIdAsync(Guid conversationId)
        {
            return await _dbSet
                .Include(c => c.Messages)
                .FirstOrDefaultAsync(c => c.Id == conversationId && !c.IsDeleted);
        }

        public async Task<ChatSession?> GetConversationByParticipantsAsync(Guid instructorId, Guid noviceDriverId)
        {
            return await _dbSet
                .Where(c => !c.IsDeleted)
                .Where(c => c.InstructorId == instructorId && c.NoviceDriverId == noviceDriverId)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<ChatSession>> GetUserConversationsAsync(Guid userId)
        {
            return await _dbSet
                .Where(c => !c.IsDeleted)
                .Where(c => c.InstructorId == userId || c.NoviceDriverId == userId)
                .OrderByDescending(c => c.LastModifiedAt)
                .ToListAsync();
        }

        public async Task<bool> IsUserParticipantAsync(Guid conversationId, Guid userId)
        {
            return await _dbSet
                .Where(c => c.Id == conversationId && !c.IsDeleted)
                .AnyAsync(c => c.InstructorId == userId || c.NoviceDriverId == userId);
        }
    }


}

