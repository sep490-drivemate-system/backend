using Microsoft.EntityFrameworkCore;
using BookingService.Domain.Entities;
using BookingService.Domain.Interfaces;
using BookingService.Infrastructure.Persistence.Context;

namespace BookingService.Infrastructure.Repositories
{
    public class BookingRepository : GenericRepository<Booking>, IBookingRepository
    {
        public BookingRepository(BookingDbContext context) : base(context) { }


      
    }
}
