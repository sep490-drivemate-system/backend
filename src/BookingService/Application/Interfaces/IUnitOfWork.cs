using BookingService.Domain.Interfaces;

namespace BookingService.Application.Interfaces
{
    public interface IUnitOfWork
    {
        IDrivingSkillRepository SkillRepository { get; }

        IRoadTypeRepository RoadTypeRepository { get; }

        Task<int> CommitChanges();

        int ReverChanges();
    }
}
