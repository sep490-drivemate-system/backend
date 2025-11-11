using Microsoft.EntityFrameworkCore;
using BookingService.Domain.Entities;
using BookingService.Domain.Interfaces;
using BookingService.Infrastructure.Persistence.Context;

namespace BookingService.Infrastructure.Repositories
{
    public class SessionRouteRepository : GenericRepository<SessionRoute>, ISessionRouteRepository
    {
        public SessionRouteRepository(BookingDbContext context) : base(context) { }

        public async Task<IEnumerable<SessionRoute>> GetRoutesBySessionIdAsync(Guid sessionId)
        {
            return await _context.SessionRoutes
                .Where(sr => sr.SessionId == sessionId && !sr.IsDeleted)
                .OrderBy(sr => sr.CreatedAt)
                .ToListAsync();
        }

        public async Task CreateMultipleAsync(IEnumerable<SessionRoute> routes)
        {
            await _context.SessionRoutes.AddRangeAsync(routes);
        }
    }
}
