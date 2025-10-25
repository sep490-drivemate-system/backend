using BookingService.Domain.Interfaces;

namespace BookingService.Application.Interfaces
{
    public interface IUnitOfWork
    {
        ITimeRangeRepository TimeRangeRepository { get; }
        IBookingRepository BookingRepository { get; }
        IDrivingSessionRepository DrivingSessionRepository { get; }
        IDrivingSkillRepository SkillRepository { get; }
        IRoadTypeRepository RoadTypeRepository { get; }
        IPackageRepository PackageRepository { get; }
        IFeedbackRepository FeedbackRepository { get; }

        Task<int> CommitChanges();

        int ReverChanges();
    }
}
