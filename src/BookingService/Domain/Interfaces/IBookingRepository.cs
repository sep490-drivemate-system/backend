using BookingService.Domain.Entities;

namespace BookingService.Domain.Interfaces
{
    public interface IBookingRepository : IGenericRepository<Booking>
    {
        // Add specific booking methods here if needed
    }
}

