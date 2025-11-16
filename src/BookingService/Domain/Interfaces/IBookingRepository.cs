using BookingService.Domain.Entities;
using BookingService.Domain.Enum;

namespace BookingService.Domain.Interfaces
{
    public interface IBookingRepository : IGenericRepository<Booking>
    {
        Task<List<Booking>> GetBookingsByDriverIdAsync(Guid driverId, BookingStatus? status = null);
    }
}

