using BookingService.Domain.Entities;
using BookingService.Domain.Interfaces;
using BookingService.Infrastructure.Persistence.Context;

namespace BookingService.Infrastructure.Repositories
{
    public class ManufacturerRepository(BookingDbContext context): GenericRepository<Manufacturer>(context), IManufacturerRepository
    {
    }
}
