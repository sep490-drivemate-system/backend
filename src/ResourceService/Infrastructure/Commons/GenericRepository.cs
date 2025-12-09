using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1;
using ResourceService.Domain.Repositories;
using System.Linq.Expressions;

namespace ResourceService.Infrastructure.Commons
{
    public class GenericRepository<T>(DbContext context): IGenericRepository<T> where T : class
    {
        protected readonly DbContext _context = context;
        protected readonly DbSet<T> _dbSet = context.Set<T>();

        private Expression<Func<T, bool>> GetPrimaryKeyLambda<Tid>(Tid id)
        {
            Type T_type = typeof(T);
            var T_model_type = _context.Model.FindEntityType(T_type);

            if (T_model_type == null)
            {
                throw new InvalidOperationException($"Entity of type {T_type.Name} does not exist in the DbContext models");
            }

            var T_model_key = T_model_type.FindPrimaryKey();

            if (T_model_key == null)
            {
                throw new InvalidOperationException($"No key defined for entity type {T_type.Name}!");
            }

            var parameter = Expression.Parameter(T_type, "e");
            var parameter_key_property = Expression.Property(parameter, T_model_key.Properties[0].Name);
            var equal = Expression.Equal(parameter_key_property, Expression.Constant(id));

            return Expression.Lambda<Func<T, bool>>(equal, parameter);
        }

        public virtual async Task<T> CreateAsync(T entity)
        {
            var tracking = await _dbSet.AddAsync(entity);

            return tracking.Entity;
        }

        public virtual async Task<List<T>> GetAllAsync(string include_properties = "", bool disable_tracking = false)
        {
            var query = _dbSet.AsQueryable();

            foreach (var property in include_properties.Split(",", StringSplitOptions.RemoveEmptyEntries))
            {
                query.Include(property.Trim());
            }

            return disable_tracking ? await query.AsNoTracking().ToListAsync() : await query.ToListAsync();
        }

        public virtual async Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, string include_properties = "", bool disable_tracking = false)
        {
            IQueryable<T> return_result = _dbSet.AsQueryable();

            if (filter != null)
            {
                return_result = return_result.Where(filter);
            }

            foreach (var property in include_properties.Split(",", StringSplitOptions.RemoveEmptyEntries))
            {
                return_result = return_result.Include(property.Trim());
            }

            if (orderBy != null)
            {
                return_result = orderBy(return_result);
            }

            if (disable_tracking)
            {
                return_result = return_result.AsNoTracking();
            }

            return await return_result.ToListAsync();
        }

        public virtual async Task<T?> GetByIdAsync<Tid>(Tid id, string include_properties = "")
        {
            if (string.IsNullOrEmpty(include_properties))
            {
                return await _dbSet.FindAsync(id);
            }

            var lamda = GetPrimaryKeyLambda(id);
            var query = _dbSet.AsQueryable();

            foreach (var property in include_properties.Split(',', StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(property.Trim());
            }

            return query.FirstOrDefault(lamda);
        }

        public virtual async Task<T?> GetByIdAsync<Tid>(Tid id, params Expression<Func<T, object>>[]? includes)
        {
            if (includes == null)
            {
                return await _dbSet.FindAsync(id);
            }

            var lamda = GetPrimaryKeyLambda(id);

            var query = _dbSet.AsQueryable();

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return query.FirstOrDefault(lamda);
        }

        public async Task<List<T>> GetByPage(int page = 1, int page_size = 10)
        {
            if (page < 1)
            {
                page = 1;
            }

            return await _dbSet.Skip((page - 1) * page_size).Take(page_size).ToListAsync();
        }

        public async void Remove<Tid>(Tid id)
        {
            var entity = await _dbSet.FindAsync(id);

            if (entity != null)
            {
                _dbSet.Remove(entity);
            }
        }

        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }

        public async void Update(T entity)
        {
            _dbSet.Update(entity);
        }
    }
}
