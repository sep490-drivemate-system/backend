using Microsoft.EntityFrameworkCore;
using ResourceService.Application.Commons;
using ResourceService.Domain.Repositories;
using ResourceService.Infrastructure.Persistences;
using ResourceService.Infrastructure.Repositories;

namespace ResourceService.Infrastructure.Commons
{
    public class UnitOfWork: IUnitOfWork
    {
        private readonly DbContext context;
        private readonly IDictionary<Type, object> _repositories = new Dictionary<Type, object>();

        private IResourceRepository _resourceRepository;

        public UnitOfWork(ResourceDbContext context)
        {
            this.context = context;
        }

        public IResourceRepository ResourceRepository => _resourceRepository ??= new ResourceRepository(context);

        public async Task<T> ReloadEntity<T>(T entity) where T : class
        {
            var entity_tracker = context.Entry(entity);

            if (entity_tracker.State == EntityState.Detached)
            {
                throw new Exception("Entity has not been tracked by the context");
            }
            
            // Reload and return the entity
            await entity_tracker.ReloadAsync();
            return entity_tracker.Entity as T;
        }

        public IGenericRepository<T> Repository<T>() where T: class
        {
            var type = typeof(T);

            if (!_repositories.ContainsKey(type))
            {
                var repoInstance = new GenericRepository<T>(context);
                _repositories[type] = repoInstance;
            }

            return (IGenericRepository<T>)_repositories[type];
        }

        public int SaveChanges()
        {
            throw new NotImplementedException();
        }

        public Task<int> SaveChangesAsync()
        {
            throw new NotImplementedException();
        }
    }
}
