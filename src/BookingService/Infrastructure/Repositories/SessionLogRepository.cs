using Microsoft.EntityFrameworkCore;
using BookingService.Domain.Entities;
using BookingService.Domain.Interfaces;
using BookingService.Infrastructure.Persistence.Context;

namespace BookingService.Infrastructure.Repositories
{
    public class SessionLogRepository : GenericRepository<SessionLog>, ISessionLogRepository
    {
        public SessionLogRepository(BookingDbContext context) : base(context) { }

        public async Task<List<SessionLog>> GetLogsBySessionIdAsync(Guid sessionId)
        {
            return await _context.RouteLogs
                .Where(sl => sl.SessionId == sessionId && !sl.IsDeleted)
                .OrderBy(sl => sl.CreatedAt)
                .ToListAsync();
        }

        public async Task AddRangeAsync(IEnumerable<SessionLog> sessionLogs)
        {
            await _context.RouteLogs.AddRangeAsync(sessionLogs);
        }
    }
}
