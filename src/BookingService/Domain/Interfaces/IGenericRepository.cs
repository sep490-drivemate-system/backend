using System.Linq.Expressions;

namespace BookingService.Domain.Interfaces
{
    /// <summary>
    ///     The basic interface for the generic repository that only return either a list of entity or only an entity.
    /// </summary>
    /// <typeparam name="T">The entity that has been defined and has an entity set in the ORM</typeparam>
    public interface IGenericRepository<T>
    {
        /// <summary>
        ///     Persists all changes made into storage (database).
        /// </summary>
        /// <returns>number of rows changes</returns>
        [Obsolete("This has been removed due to inconsistency. Please use UnitOfWork.CommitChangesAsync.", false)]
        Task<int> CommitChangesAsync();

        /// <summary>
        ///     Add a new entity to the current entity set. This does not persist changes to the database.
        /// </summary>
        /// <param name="entity">The entity to be added</param>
        /// <returns>The added entity</returns>
        Task<T> CreateAsync(T entity);

        /// <summary>
        ///     Load all records of entity <typeparamref name="T"/> into entity set and return the list. 
        /// </summary>
        /// <param name="disable_tracking">Does not keep track of the returned entities</param>
        /// <returns>All records in the database of type <typeparamref name="T"/> </returns>
        Task<List<T>> GetAllAsync(string include_properties = "", bool disable_tracking = false);


        /// <summary>
        ///     Load all records of entity <typeparamref name="T"/> into entity set and return the list of entities matching the given criteria.
        /// </summary>
        /// <param name="filter">The filter used to query for matching entities</param>
        /// <param name="orderBy">How queried entities are ordered.</param>
        /// <param name="disable_tracking">Does not keep track of the returned entities</param>
        /// <returns>The list of records that match the given <paramref name="filter"/> criteria</returns>
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, string include_properties = "", bool disable_tracking = false);

        /// <summary>
        ///     Get a record by it's primary key. 
        /// </summary>
        /// <typeparam name="Tid">The type of the entity primary key</typeparam>
        /// <param name="id">The entity primary key</param>
        /// <returns>An entity of type <typeparamref name="T"/> if found, else return null</returns>
        Task<T?> GetByIdAsync<Tid>(Tid id, params Expression<Func<T, object>>[]? includes);

        /// <summary>
        ///     Return a record using the entity primary key (or id).
        ///     Optionally return some extra data and relationship.
        /// </summary>
        /// <typeparam name="Tid">The type of the entity primary key</typeparam>
        /// <param name="id">The entity primary key</param>
        /// <param name="include_properties">Names of the included properties, seperated by comma (",")</param>
        /// <returns>An entity of type <typeparamref name="T"/> if found, else return null</returns>
        Task<T?> GetByIdAsync<Tid>(Tid id, string include_properties = "");

        /// <summary>
        ///     Remove an entity of type <typeparamref name="T"/> with it primary key (id) if found.
        /// </summary>
        /// <typeparam name="Tid">
        /// The type of the primary key.
        /// <para>
        ///     For component key, see <see href="https://stackoverflow.com/questions/47949228/entity-framework-core-find-and-composite-key"/>
        /// </para>
        /// </typeparam>
        /// <param name="id">The primary identifier of the entity in the entity set (or database)</param>
        void Remove<Tid>(Tid id);

        /// <summary>
        ///     Remove an entity of type T if found.
        /// </summary>
        /// <param name="entity">The entity record to remove</param>
        void Remove(T entity);

        /// <summary>
        ///     Update an entity of type <typeparamref name="T"/>.
        ///     <para>
        ///         Caution: DIRECT UPDATE WILL WORK FINE IF THE ENTITY IS NOT LOADED. BUT IF THE ENTITY IS LOADED, UPDATE THE LOADED ENTITY INSTEAD.
        ///     </para>
        ///    <para>
        ///         Do this by getting the entity loaded in the DbContext and directly modify it properties.
        ///    </para>
        /// </summary>
        /// <param name="entity">The entity to be update</param>
        void Update(T entity);

        /// <summary>
        ///     Return a simple list of entity.
        ///     This is used for basic pagination
        /// </summary>
        /// <param name="page">The default is 1. Any smaller number will default back to first page</param>
        /// <param name="page_size">The page size of the item</param>
        /// <returns></returns>
        Task<List<T>> GetByPage(int page = 1, int page_size = 10);
    }
}
