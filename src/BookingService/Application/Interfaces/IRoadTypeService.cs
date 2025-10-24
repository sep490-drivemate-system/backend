using BookingService.Application.Commons.DTOs.RoadTypes;
using SharedLibrary.SharedKernel.ServiceResult;

namespace BookingService.Application.Interfaces
{
    public interface IRoadTypeService
    {
        Task<Result<List<RoadTypeDTO>>> GetAllRoadType();

        Task<Result<RoadTypeDTO>> GetRoadTypeById(Guid id);
    }
}
