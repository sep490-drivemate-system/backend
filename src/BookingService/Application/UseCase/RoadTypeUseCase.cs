using AutoMapper;
using BookingService.Application.Commons.Constants;
using BookingService.Application.Commons.DTOs.DrivingSkills;
using BookingService.Application.Commons.DTOs.RoadTypes;
using BookingService.Application.Interfaces;
using BookingService.Domain.Entities;
using SharedLibrary.SharedKernel.ServiceResult;
using System.Linq.Expressions;

namespace BookingService.Application.UseCase
{
    public class RoadTypeUseCase(IUnitOfWork unitOfWork,IMapper mapper) : IRoadTypeUseCase
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<Result<bool>> CreateRoadType(RoadTypeCreationDTO road_type)
        {
            try
            {
                // Checking for existing road types
                Expression<Func<RoadType, bool>> filter = x => x.Name.ToLower().Equals(road_type.Name.ToLower()) && !x.IsDeleted;
                var existingRoads = await _unitOfWork.RoadTypeRepository.GetAllAsync(filter);

                if (existingRoads.Count != 0)
                {
                    return Result<bool>.Failure(ServiceError.ExistedError($"{road_type.Name}"), Messages.Commons.UNHANDLED);
                }

                await _unitOfWork.RoadTypeRepository.CreateAsync(new RoadType{ Name = road_type.Name });
                await _unitOfWork.CommitChangesAsync();
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure(ServiceError.UnhandledException($"{ex.Message}"), Messages.Commons.UNHANDLED);
            }

            return Result<bool>.Success(true);
        }

        public async Task<Result<bool>> DeleteRoadType(Guid id)
        {
            var road = await _unitOfWork.RoadTypeRepository.GetByIdAsync(id);

            if (road == null || road.IsDeleted)
            {
                return Result<bool>.Failure(ServiceError.NotFoundError($"{id}"), Messages.Commons.NOTFOUND);
            }

            road.IsDeleted = true;
            _unitOfWork.RoadTypeRepository.Update(road);
            await _unitOfWork.CommitChangesAsync();
            return Result<bool>.Success(true, Messages.Commons.SUCCESS);
        }

        public async Task<Result<IEnumerable<RoadTypeDTO>>> GetAllRoadType()
        {
            var roads = await _unitOfWork.RoadTypeRepository.GetAllAsync(filter: x => !x.IsDeleted);
            return Result<IEnumerable<RoadTypeDTO>>.Success(roads.Select(x => new RoadTypeDTO { 
                Id = x.Id, Name = x.Name}),
                Messages.Commons.SUCCESS);
        }

        public async Task<Result<RoadTypeDTO>> GetRoadTypeById(Guid id)
        {
            var road = await _unitOfWork.RoadTypeRepository.GetByIdAsync(id);
            if (road != null && !road.IsDeleted)
            {
                return Result<RoadTypeDTO>.Success(new RoadTypeDTO
                {
                    Id = id,
                    Name = road.Name,
                });
            }
            return Result<RoadTypeDTO>.Failure(ServiceError.NotFoundError("Can not find road type"));
        }

        public async Task<Result<bool>> UpdateRoadType(Guid id, RoadTypeCreationDTO road_type)
        {
            var road = await _unitOfWork.RoadTypeRepository.GetByIdAsync(id);

            if (road == null || road.IsDeleted)
            {
                return Result<bool>.Failure(ServiceError.NotFoundError($"{id}"), Messages.Commons.NOTFOUND);
            }

            road.Name = road_type.Name;

            try
            {
                _unitOfWork.RoadTypeRepository.Update(road);
                await _unitOfWork.CommitChangesAsync();
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure(ServiceError.UnhandledException(ex.Message), Messages.Commons.UNHANDLED);
            }

            return Result<bool>.Success(true, Messages.Commons.SUCCESS);
        }
    }
}
