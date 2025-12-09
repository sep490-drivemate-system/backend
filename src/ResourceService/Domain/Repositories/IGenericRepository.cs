using System.Linq.Expressions;

namespace ResourceService.Domain.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> CreateAsync(T entity);
        Task<List<T>> GetAllAsync(string include_properties = "", bool disable_tracking = false);
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, string include_properties = "", bool disable_tracking = false);
        Task<T?> GetByIdAsync<Tid>(Tid id, params Expression<Func<T, object>>[]? includes);
        Task<T?> GetByIdAsync<Tid>(Tid id, string include_properties = "");
        void Remove<Tid>(Tid id);
        void Remove(T entity);       
        void Update(T entity);
        Task<List<T>> GetByPage(int page = 1, int page_size = 10);
    }
}
