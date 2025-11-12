using BookingService.Domain.Interfaces;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BookingService.Application.Interfaces
{
    public interface IUnitOfWork
    {
        /// <summary>
        ///     Return a generic implementation of entities repository, useful for doing simple queries.
        /// </summary>
        /// <typeparam name="IEntity">Type of entity</typeparam>
        /// <returns>A generic <see cref="IGenericRepository{T}"/> implementation for entity of type <typeparamref name="IEntity"/> </returns>
        IGenericRepository<IEntity> Repository<IEntity>() where IEntity : class;

        /// <summary>
        ///     Revert all changes made (before a commit).
        /// </summary>
        void RevertChanges();

        /// <summary>
        ///     Commit all current changes to dedicated storage (database)
        /// </summary>
        /// <returns>Number of rows has been affected</returns>
        /// <exception cref="DbUpdateException" />
        /// <exception cref="DbUpdateConcurrencyException" />
        /// <exception cref="OperationCanceledException" />
        Task<int> CommitChangesAsync();

        /// <summary>
        ///     Return the current tracking item in the dbcontext if exist
        /// </summary>
        /// <param name="entity">The entity to get tracking state.</param>
        /// <returns>The <see cref="EntityEntry"/> for the given object.</returns>
        EntityEntry GetTrackingEntry(object entity);

        // Main aggregated entities
        ICarRepository CarRepository { get; }
        IBookingRepository BookingRepository { get; }
        IDrivingSessionRepository DrivingSessionRepository { get; }
        IDrivingSkillRepository SkillRepository { get; }
        IRoadTypeRepository RoadTypeRepository { get; }
        IPackageRepository PackageRepository { get; }
        IFeedbackRepository FeedbackRepository { get; }
        ISessionRouteRepository SessionRouteRepository { get; }
        ISessionLogRepository SessionLogRepository { get; }
    }
}
