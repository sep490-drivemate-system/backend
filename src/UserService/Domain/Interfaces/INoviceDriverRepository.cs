using UserService.Domain.Entities;

namespace UserService.Domain.Interfaces
{
    public interface INoviceDriverRepository: IGenericRepository<NoviceDriver>
    {
        Task<NoviceDriver?> GetByIdWithUserAsync(Guid id);
    }
}
