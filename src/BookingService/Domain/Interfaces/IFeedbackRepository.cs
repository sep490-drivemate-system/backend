using BookingService.Domain.Entities;

namespace BookingService.Domain.Interfaces
{
    public interface IFeedbackRepository
    {
        Task<IEnumerable<Feedback>> GetAllAsync();
        Task<Feedback?> GetByIdAsync(Guid id);
        Task<IEnumerable<Feedback>> GetByBookingIdAsync(Guid bookingId);
        Task<IEnumerable<Feedback>> GetByUserIdAsync(Guid userId);
        Task<Feedback> CreateAsync(Feedback feedback);
        Task<Feedback> UpdateAsync(Feedback feedback);
        Task DeleteAsync(Guid id);
    }
}
