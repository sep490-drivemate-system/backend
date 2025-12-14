using Microsoft.EntityFrameworkCore;
using ResourceService.Domain.Entities;
using ResourceService.Domain.Enums;
using ResourceService.Domain.Repositories;
using ResourceService.Infrastructure.Commons;
using ResourceService.Infrastructure.Persistences;

namespace ResourceService.Infrastructure.Repositories
{
    public class QARepository : GenericRepository<QaQuestion>, IQARepository
    {
        public QARepository(ResourceDbContext context) : base(context) { }

        public async Task<QaQuestion?> GetWithDetailsAsync(Guid questionId)
        {
            return await _dbSet.AsNoTracking()
                .FirstOrDefaultAsync(q => q.Id == questionId && !q.IsDeleted);
        }

        public async Task<bool> UpdateStatusAsync(Guid questionId, QaStatus status, string? reason, Guid? reviewerId)
        {
            var question = await _dbSet.FirstOrDefaultAsync(q => q.Id == questionId);
            if (question == null) return false;
            question.LastModifiedAt = DateTime.UtcNow;
            return true;
        }
    }
}

