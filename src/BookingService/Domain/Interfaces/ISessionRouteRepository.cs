using BookingService.Domain.Entities;

namespace BookingService.Domain.Interfaces
{
    public interface ISessionRouteRepository : IGenericRepository<SessionRoute>
    {
        Task<IEnumerable<SessionRoute>> GetRoutesBySessionIdAsync(Guid sessionId);
        Task CreateMultipleAsync(IEnumerable<SessionRoute> routes);
    }
}
