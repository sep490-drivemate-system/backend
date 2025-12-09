using ResourceService.Domain.Repositories;

namespace ResourceService.Application.Commons
{
    public interface IUnitOfWork
    {
        IGenericRepository<T> Repository<T>() where T : class;
        int SaveChanges();
        Task<int> SaveChangesAsync();
        Task<T> ReloadEntity<T>(T entity) where T: class;

        IResourceRepository ResourceRepository { get; }
    }
}
