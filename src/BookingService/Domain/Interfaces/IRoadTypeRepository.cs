using BookingService.Domain.Entities;

namespace BookingService.Domain.Interfaces
{
    public interface IRoadTypeRepository
    {
        Task<List<RoadType>> GetAllRoadType();

        Task<RoadType?> GetRoadTypeById(Guid id);

        Task<RoadType> CreateRoadType(RoadType info);

        Task<RoadType> RemoveRoadType(Guid id);

        Task<RoadType> UpdateRoadType(RoadType info);
    }
}
