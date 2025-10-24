using BookingService.Application.Commons.DTOs.RoadTypes;
using BookingService.Application.Interfaces;
using SharedLibrary.SharedKernel.ServiceResult;

namespace BookingService.Application.UseCase
{
    public class RoadTypeService(IUnitOfWork unitOfWork) : IRoadTypeService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<Result<List<RoadTypeDTO>>> GetAllRoadType()
        {
            var roads = await _unitOfWork.RoadTypeRepository.GetAllRoadType();

            return Result<List<RoadTypeDTO>>.Success(roads.Select(x => new RoadTypeDTO
            {
                Id = x.Id,
                Name = x.Name
            }).ToList());
        }

        public async Task<Result<RoadTypeDTO>> GetRoadTypeById(Guid id)
        {
            var road = await _unitOfWork.RoadTypeRepository.GetRoadTypeById(id);

            if (road != null)
            {
                return Result<RoadTypeDTO>.Success(new RoadTypeDTO
                {
                    Id = id,
                    Name = road.Name
                });
            }

            return Result<RoadTypeDTO>.Failure(ServiceError.NotFoundError("Can not find road type"));
        }
    }
}
