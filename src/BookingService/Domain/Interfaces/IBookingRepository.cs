using BookingService.Domain.Entities;

namespace BookingService.Domain.Interfaces
{
    public interface IBookingRepository
    {
        Task<IEnumerable<Booking>> GetAllAsync();
        Task<Booking?> GetByIdAsync(Guid id);
        Task<IEnumerable<Booking>> GetByUserIdAsync(Guid userId);
        Task<IEnumerable<Booking>> GetByInstructorIdAsync(Guid instructorId);
        Task<Booking> CreateAsync(Booking booking);
        Task<Booking> UpdateAsync(Booking booking);
        Task DeleteAsync(Guid id);
    }
}
