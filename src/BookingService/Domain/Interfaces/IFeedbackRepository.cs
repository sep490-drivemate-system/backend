using BookingService.Domain.Entities;

namespace BookingService.Domain.Interfaces
{
    public interface IFeedbackRepository : IGenericRepository<Feedback>
    {
        Task<IEnumerable<Feedback>> GetAllFeedbacksAsync();
        Task<Feedback?> GetFeedbackByIdAsync(Guid id);
        Task<IEnumerable<Feedback>> GetByBookingIdAsync(Guid bookingId);
        Task<IEnumerable<Feedback>> GetByUserIdAsync(Guid userId);
        Task<Feedback> CreateFeedbackAsync(Feedback feedback);
        Task<Feedback> UpdateFeedbackAsync(Feedback feedback);
        Task DeleteFeedbackAsync(Guid id);
    }
}
