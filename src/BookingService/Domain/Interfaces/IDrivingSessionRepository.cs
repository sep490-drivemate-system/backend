using BookingService.Domain.Entities;

namespace BookingService.Domain.Interfaces
{
    public interface IDrivingSessionRepository
    {
        Task<IEnumerable<DrivingSession>> GetAllAsync();
        Task<DrivingSession?> GetByIdAsync(Guid id);
        Task<IEnumerable<DrivingSession>> GetByBookingIdAsync(Guid bookingId);
        Task<DrivingSession> CreateAsync(DrivingSession session);
        Task<DrivingSession> UpdateAsync(DrivingSession session);
        Task DeleteAsync(Guid id);
    }
}
