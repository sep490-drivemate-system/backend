using BookingService.Application.Commons.DTOs.InstructorRoutes;
using BookingService.Application.Interfaces;
using BookingService.Domain.Entities;
using BookingService.Domain.Interfaces;
using SharedLibrary.SharedKernel.Pagination;
using SharedLibrary.SharedKernel.ServiceResult;

namespace BookingService.Application.UseCase
{
    public class InstructorRoutesUseCase : IInstructorRoutesUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private const int DefaultPageSize = 10;
        private const int DefaultPageNumber = 1;

        public InstructorRoutesUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<PaginatedList<InstructorRouteDTO>>> GetRoutes(Guid instructorId)
        {
            var routes = await _unitOfWork.InstructorRoutesRepository.GetByInstructorAsync(instructorId);

            if (!routes.Any())
            {
                return Result<PaginatedList<InstructorRouteDTO>>.Success(
                    new PaginatedList<InstructorRouteDTO>());
            }

            var routeDTOs = routes.Select(r => new InstructorRouteDTO
            {
                Id = r.Id,
                InstructorId = r.InstructorId,
                RouteName = r.RouteName,
                Polyline = r.Polyline,
                StartingLatitude = r.StartingLatitude,
                StartingLongtitude = r.StartingLongtitude,
                EndingLatitude = r.EndingLatitude,
                EndingLongtitude = r.EndingLongtitude,
                DisplayStartLocationName = r.DisplayStartLocationName,
                DisplayEndLocationName = r.DisplayEndLocationName,
                LastModifiedAt = r.LastModifiedAt,
                CreatedAt = r.CreatedAt
            }).ToList();

            var paginatedList = PaginatedList<InstructorRouteDTO>.Create(
                routeDTOs,
                DefaultPageNumber,
                DefaultPageSize);

            return Result<PaginatedList<InstructorRouteDTO>>.Success(paginatedList);
        }

        public async Task<Result<InstructorRouteDTO>> GetRoute(Guid id)
        {
            var route = await _unitOfWork.InstructorRoutesRepository.GetByIdAsync(id);

            if (route == null || route.IsDeleted)
            {
                return Result<InstructorRouteDTO>.Failure(
                    ServiceError.NotFoundError($"{id}"),
                    "Route not found");
            }

            var dto = new InstructorRouteDTO
            {
                Id = route.Id,
                InstructorId = route.InstructorId,
                RouteName = route.RouteName,
                Polyline = route.Polyline,
                StartingLatitude = route.StartingLatitude,
                StartingLongtitude = route.StartingLongtitude,
                EndingLatitude = route.EndingLatitude,
                EndingLongtitude = route.EndingLongtitude,
                DisplayStartLocationName = route.DisplayStartLocationName,
                DisplayEndLocationName = route.DisplayEndLocationName,
                LastModifiedAt = route.LastModifiedAt,
                CreatedAt = route.CreatedAt
            };

            return Result<InstructorRouteDTO>.Success(dto);
        }

        public async Task<Result<bool>> CreateRoute(InstructorRouteDTO instructorRouteDTO, Guid instructorId)
        {
            // Validation
            if (string.IsNullOrWhiteSpace(instructorRouteDTO.RouteName))
            {
                return Result<bool>.Failure(
                    ServiceError.BadRequestError("RouteName is required"),
                    "Invalid input");
            }

            if (string.IsNullOrWhiteSpace(instructorRouteDTO.Polyline))
            {
                return Result<bool>.Failure(
                    ServiceError.BadRequestError("Polyline is required"),
                    "Invalid input");
            }

            if (string.IsNullOrWhiteSpace(instructorRouteDTO.DisplayStartLocationName) ||
                string.IsNullOrWhiteSpace(instructorRouteDTO.DisplayEndLocationName))
            {
                return Result<bool>.Failure(
                    ServiceError.BadRequestError("Location names are required"),
                    "Invalid input");
            }

            var route = new InstructorRoutes
            {
                Id = Guid.NewGuid(),
                InstructorId = instructorId,
                RouteName = instructorRouteDTO.RouteName,
                Polyline = instructorRouteDTO.Polyline,
                StartingLatitude = instructorRouteDTO.StartingLatitude,
                StartingLongtitude = instructorRouteDTO.StartingLongtitude,
                EndingLatitude = instructorRouteDTO.EndingLatitude,
                EndingLongtitude = instructorRouteDTO.EndingLongtitude,
                DisplayStartLocationName = instructorRouteDTO.DisplayStartLocationName,
                DisplayEndLocationName = instructorRouteDTO.DisplayEndLocationName,
                CreatedAt = DateTime.UtcNow,
                LastModifiedAt = DateTime.UtcNow,
                IsDeleted = false
            };

            await _unitOfWork.InstructorRoutesRepository.CreateAsync(route);
            await _unitOfWork.CommitChangesAsync();

            return Result<bool>.Success(true);
        }

        public async Task<Result<bool>> UpdateRoute(Guid id, InstructorRouteDTO instructorRouteDTO)
        {
            var route = await _unitOfWork.InstructorRoutesRepository.GetByIdAsync(id);

            if (route == null || route.IsDeleted)
            {
                return Result<bool>.Failure(
                    ServiceError.NotFoundError($"{id}"),
                    "Route not found");
            }

            // Update all fields from DTO (partial update)
            if (!string.IsNullOrWhiteSpace(instructorRouteDTO.RouteName))
            {
                route.RouteName = instructorRouteDTO.RouteName;
            }

            if (!string.IsNullOrWhiteSpace(instructorRouteDTO.Polyline))
            {
                route.Polyline = instructorRouteDTO.Polyline;
            }

            if (!string.IsNullOrWhiteSpace(instructorRouteDTO.DisplayStartLocationName))
            {
                route.DisplayStartLocationName = instructorRouteDTO.DisplayStartLocationName;
            }

            if (!string.IsNullOrWhiteSpace(instructorRouteDTO.DisplayEndLocationName))
            {
                route.DisplayEndLocationName = instructorRouteDTO.DisplayEndLocationName;
            }

            // Update coordinates (always update as coordinates can be 0 which is valid)
            route.StartingLatitude = instructorRouteDTO.StartingLatitude;
            route.StartingLongtitude = instructorRouteDTO.StartingLongtitude;
            route.EndingLatitude = instructorRouteDTO.EndingLatitude;
            route.EndingLongtitude = instructorRouteDTO.EndingLongtitude;

            route.LastModifiedAt = DateTime.UtcNow;

            _unitOfWork.InstructorRoutesRepository.Update(route);
            await _unitOfWork.CommitChangesAsync();

            return Result<bool>.Success(true);
        }

        public async Task<Result<bool>> DeleteRoute(Guid id)
        {
            var route = await _unitOfWork.InstructorRoutesRepository.GetByIdAsync(id);

            if (route == null || route.IsDeleted)
            {
                return Result<bool>.Failure(
                    ServiceError.NotFoundError($"{id}"),
                    "Route not found");
            }

            route.IsDeleted = true;
            route.LastModifiedAt = DateTime.UtcNow;

            _unitOfWork.InstructorRoutesRepository.Update(route);
            await _unitOfWork.CommitChangesAsync();

            return Result<bool>.Success(true);
        }
    }
}


