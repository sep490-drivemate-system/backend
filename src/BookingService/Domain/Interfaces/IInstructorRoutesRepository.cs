using BookingService.Domain.Entities;

namespace BookingService.Domain.Interfaces
{
    public interface IInstructorRoutesRepository : IGenericRepository<InstructorRoutes>
    {
        Task<IEnumerable<InstructorRoutes>> GetByInstructorAsync(Guid instructorId);
    }
}
