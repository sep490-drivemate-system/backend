using ResourceService.Domain.Entities;
using ResourceService.Domain.Enums;

namespace ResourceService.Domain.Repositories
{
    public interface IQARepository : IGenericRepository<QaQuestion>
    {
        Task<QaQuestion?> GetWithDetailsAsync(Guid questionId);
        Task<bool> UpdateStatusAsync(Guid questionId, QaStatus status, string? reason, Guid? reviewerId);
    }
}

