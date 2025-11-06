using BookingService.Application.Commons.DTOs.RoadTypes;
using SharedLibrary.SharedKernel.ServiceResult;

namespace BookingService.Application.Interfaces
{
    public interface IRoadTypeUseCase
    {
        Task<Result<IEnumerable<RoadTypeDTO>>> GetAllRoadType();
        Task<Result<RoadTypeDTO>> GetRoadTypeById(Guid id);
        Task<Result<bool>> CreateRoadType(RoadTypeCreationDTO road_type);
        Task<Result<bool>> UpdateRoadType(Guid id, RoadTypeCreationDTO road_type);
        Task<Result<bool>> DeleteRoadType(Guid id);
    }
}
