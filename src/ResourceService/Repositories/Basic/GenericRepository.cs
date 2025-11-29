using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1;
using ResourceService.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ResourceService.Repositories.Basic
{
    public class GenericRepository<T>: IGenericRepository<T> where T : class
    {
        protected ResourceDbContext _context;

        public GenericRepository()
        {
            _context ??= new ResourceDbContext();
        }

        public GenericRepository(ResourceDbContext context)
        {
            _context = context;
        }

        public List<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }
        public async Task<List<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public virtual async Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, string include_properties = "", bool disable_tracking = true)
        {
            IQueryable<T> query = _context.Set<T>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            var includeProps = include_properties.Split(",", StringSplitOptions.RemoveEmptyEntries);
            foreach (var property in includeProps)
            {
                query = query.Include(property.Trim());
            }

            if (includeProps.Length > 1)
            {
                query = query.AsSplitQuery();
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (disable_tracking)
            {
                query = query.AsNoTracking();
            }

            return await query.ToListAsync();
        }
        
        public void Create(T entity)
        {
            _context.Add(entity);
            _context.SaveChanges();
        }

        public async Task<int> CreateAsync(T entity)
        {
            _context.Add(entity);
            return await _context.SaveChangesAsync();
        }

        public void Update(T entity)
        {
            _context.ChangeTracker.Clear();
            var tracker = _context.Attach(entity);
            tracker.State = EntityState.Modified;
            _context.SaveChanges();
        }

        public async Task<int> UpdateAsync(T entity)
        {
            _context.ChangeTracker.Clear();
            var tracker = _context.Attach(entity);
            tracker.State = EntityState.Modified;
            return await _context.SaveChangesAsync();
        }
        public async void Remove<Tid>(Tid id)
        {
            var entity = await _context.Set<T>().FindAsync(id);

            if (entity != null)
            {
                _context.Remove(entity);
            }
        }

        public void Remove(T entity)
        {
            _context.Remove(entity);
            _context.SaveChanges();
        }

        public async Task<bool> RemoveAsync(T entity)
        {
            _context.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public T GetById(int id)
        {
            return _context.Set<T>().Find(id);
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public T GetById(string code)
        {
            return _context.Set<T>().Find(code);
        }

        public async Task<T> GetByIdAsync(string code)
        {
            return await _context.Set<T>().FindAsync(code);
        }

        public T GetById(Guid code)
        {
            return _context.Set<T>().Find(code);
        }

        public async Task<T> GetByIdAsync(Guid code)
        {
            return await _context.Set<T>().FindAsync(code);
        }

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

        public async Task<int> CommitChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public virtual async Task<List<T>> GetAllAsync(string include_properties = "", bool disable_tracking = false)
        {
            var query = _context.Set<T>().AsQueryable();

            var includeProps = include_properties.Split(",", StringSplitOptions.RemoveEmptyEntries);
            foreach (var property in includeProps)
            {
                query.Include(property.Trim());
            }

            if (includeProps.Length > 1)
            {
                query = query.AsSplitQuery();
            }

            return disable_tracking ? await query.AsNoTracking().ToListAsync() : await query.ToListAsync();
        }

        public virtual async Task<T?> GetByIdAsync<Tid>(Tid id, string include_properties = "")
        {
            // Best case scenario, no include property
            if (string.IsNullOrEmpty(include_properties))
            {
                return await _context.Set<T>().FindAsync(id);
            }

            var lamda = GetPrimaryKeyLambda(id);

            // Building the include query
            var query = _context.Set<T>().AsQueryable();

            var includeProps = include_properties.Split(',', StringSplitOptions.RemoveEmptyEntries);
            foreach (var property in includeProps)
            {
                query = query.Include(property.Trim());
            }

            if (includeProps.Length > 1)
            {
                query = query.AsSplitQuery();
            }

            // Return the result using the built expression
            return query.FirstOrDefault(lamda);
        }

        public virtual async Task<T?> GetByIdAsync<Tid>(Tid id, params Expression<Func<T, object>>[]? includes)
        {
            // Best case scenario, no include property
            if (includes == null)
            {
                return await _context.Set<T>().FindAsync(id);
            }

            var lamda = GetPrimaryKeyLambda(id);

            var query = _context.Set<T>().AsQueryable();

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

            return await _context.Set<T>().Skip((page - 1) * page_size).Take(page_size).ToListAsync();
        }

        #region Separating asigned entity and save operators        

        public void PrepareCreate(T entity)
        {
            _context.Add(entity);
        }

        public void PrepareUpdate(T entity)
        {
            var tracker = _context.Attach(entity);
            tracker.State = EntityState.Modified;
        }

        public void PrepareRemove(T entity)
        {
            _context.Remove(entity);
        }

        public int Save()
        {
            return _context.SaveChanges();
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        #endregion Separating asign entity and save operators
    }

}
