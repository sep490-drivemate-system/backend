using AutoMapper;
using BookingService.Application.Commons.DTOs.DrivingSkills;
using BookingService.Application.Commons.DTOs.RoadTypes;
using BookingService.Application.Interfaces;
using SharedLibrary.SharedKernel.ServiceResult;

namespace BookingService.Application.UseCase
{
    public class RoadTypeUseCase(IUnitOfWork unitOfWork,IMapper mapper) : IRoadTypeUseCase
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        public async Task<Result<List<RoadTypeDTO>>> GetAllRoadType()
        {
            var roads = await _unitOfWork.RoadTypeRepository.GetAllRoadType();

            var roadDTOs = _mapper.Map<List<RoadTypeDTO>>(roads);
            return Result<List<RoadTypeDTO>>.Success(roadDTOs);
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
