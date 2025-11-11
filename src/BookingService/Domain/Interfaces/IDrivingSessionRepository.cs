using BookingService.Domain.Entities;
using BookingService.Domain.Enum;

namespace BookingService.Domain.Interfaces
{
    public interface IDrivingSessionRepository : IGenericRepository<DrivingSession>
    {
        Task<IEnumerable<DrivingSession>> GetAllByStatus(SessionStatus status);
        Task<IEnumerable<DrivingSession>> GetSessionsByInstructorIdAsync(Guid instructorId, SessionStatus? status = null);
    }
}
