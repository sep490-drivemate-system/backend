using BookingService.Domain.Entities;
using BookingService.Domain.Enum;

namespace BookingService.Domain.Interfaces
{
    public interface IDrivingSessionRepository : IGenericRepository<DrivingSession>
    {
        Task<IEnumerable<DrivingSession>> GetAllByStatus(SessionStatus status);
    }
}
