using MessagingService.Domain.DTOs;
using MessagingService.Domain.Entities;
using MessagingService.Domain.Enum;
using MessagingService.Domain.Interfaces;
using MessagingService.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.SharedKernel.Enum;

namespace MessagingService.Infrastructure.Repositories
{
    public class ChatSessionRepository : GenericRepository<ChatSession>, IChatSessionRepository
    {
        public ChatSessionRepository(MessagingDbContext context) : base(context)
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
                .Include(c => c.Messages)
                .Where(c => !c.IsDeleted)
                .Where(c => c.InstructorId == userId || c.NoviceDriverId == userId)
                .OrderByDescending(c => c.LastModifiedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<ChatSessionLastMessageDTO>> GetUserChatSessionsAsync(Guid userId, UserRole userRole)
        {
            return await _dbSet
                .Where(c => !c.IsDeleted)
                .Where(c => c.InstructorId == userId || c.NoviceDriverId == userId)
                .OrderByDescending(c => c.LastModifiedAt)
                .Select(c => new ChatSessionLastMessageDTO
                {
                    Id = c.Id,
                    PartnerId = userRole == UserRole.Instructor ? c.NoviceDriverId : c.InstructorId,
                    LastMessage = c.Messages
                        .Where(m => !m.IsDeleted)
                        .OrderByDescending(m => m.CreatedAt)
                        .Select(m => m.Content)
                        .FirstOrDefault() ?? string.Empty,
                    Status = c.Messages
                        .Where(m => !m.IsDeleted)
                        .OrderByDescending(m => m.CreatedAt) 
                        .Select(m => m.Status)
                        .FirstOrDefault(),
                    LastMessageAt = c.Messages
                        .Where(m => !m.IsDeleted)
                        .OrderByDescending(m => m.CreatedAt)
                        .Select(m => (DateTime?)m.CreatedAt)
                        .FirstOrDefault(),
                    LastModifiedAt = c.LastModifiedAt
                })
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

