using BookingService.Domain.Interfaces;
using BookingService.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BookingService.Infrastructure.Repositories
{
    /* IMPORTANT NOTE
         
    Please do not touch this. DO NOT TOUCH THIS.
    It took me a whole night to complete this behemoth that I won't touch again and will refuse to work on if any changes are made.
    */
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly BookingDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(BookingDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        /// <summary>
        ///     Return the primary key boolean lambda function used by <see cref="GetByIdAsync{Tid}(Tid, Expression{Func{T, object}}[]?)"/> and
        ///     <see cref="GetByIdAsync{Tid}(Tid, string)"/>.
        /// </summary>
        /// <typeparam name="Tid">The type of the primary key</typeparam>
        /// <param name="id">The primary key value</param>
        /// <returns>An <see cref="Expression"/>.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        private Expression<Func<T, bool>> GetPrimaryKeyLambda<Tid>(Tid id)
        {
            // Simple explanation for this part:
            // Use reflection and EF Core metadata to dynamically build a primary key property predicate (lambda function)
            // in order to work with DbSet.FirstOrDefault(lambda) for unknown entity at runtime.

            // Read more about expression tree and reflection.
            // Reflection: https://learn.microsoft.com/en-us/dotnet/csharp/advanced-topics/reflection-and-attributes/
            // Expression tree: https://learn.microsoft.com/en-us/dotnet/csharp/advanced-topics/expression-trees/

            // Side note: Using reflection is EXPENSIVE ! Also, we are overengineering this one.

            Type T_type = typeof(T);

            // This validate that the entity is an actual registered model in the dbcontext (a "table entity")
            var T_model_type = _context.Model.FindEntityType(T_type);

            if (T_model_type == null)
            {
                throw new InvalidOperationException($"Entity of type {T_type.Name} does not exist in the DbContext models");
            }

            // If the model does not have any primary key, we cant do a "get by key query" isn't it ?
            // Since our project does not have any composite key, There should only be one key.
            var T_model_key = T_model_type.FindPrimaryKey();

            if (T_model_key == null)
            {
                throw new InvalidOperationException($"No key defined for entity type {T_type.Name}!");
            }

            // Build predicate for the primary key.
            var parameter = Expression.Parameter(T_type, "e");
            var parameter_key_property = Expression.Property(parameter, T_model_key.Properties[0].Name);
            var equal = Expression.Equal(parameter_key_property, Expression.Constant(id));

            return Expression.Lambda<Func<T, bool>>(equal, parameter);
        }

        public async Task<int> CommitChangesAsync()
        {
            return await _context.SaveChangesAsync();
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

        public virtual async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, string include_properties = "", bool disable_tracking = false)
        {
            IQueryable<T> return_result = _dbSet.AsQueryable();

            if (filter != null)
            {
                return_result = return_result.Where(filter);
            }

            foreach (var property in include_properties.Split(",", StringSplitOptions.RemoveEmptyEntries))
            {
                return_result.Include(property.Trim());
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
            // Best case scenario, no include property
            if (string.IsNullOrEmpty(include_properties))
            {
                return await _dbSet.FindAsync(id);
            }

            var lamda = GetPrimaryKeyLambda(id);

            // Building the include query
            var query = _dbSet.AsQueryable();

            foreach (var property in include_properties.Split(',', StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(property.Trim());
            }

            // Return the result using the built expression
            return query.FirstOrDefault(lamda);
        }

        public virtual async Task<T?> GetByIdAsync<Tid>(Tid id, params Expression<Func<T, object>>[]? includes)
        {
            // Best case scenario, no include property
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
            // Default back to first page if invalid page is given.
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
