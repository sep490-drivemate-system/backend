using BookingService.Domain.Entities;

namespace BookingService.Domain.Interfaces
{
    public interface ISessionLogRepository : IGenericRepository<SessionLog>
    {
        Task<List<SessionLog>> GetLogsBySessionIdAsync(Guid sessionId);
        Task AddRangeAsync(IEnumerable<SessionLog> sessionLogs);
    }
}
