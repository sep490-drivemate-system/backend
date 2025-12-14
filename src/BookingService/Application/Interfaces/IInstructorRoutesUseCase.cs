using BookingService.Application.Commons.DTOs.InstructorRoutes;
using SharedLibrary.SharedKernel.Pagination;
using SharedLibrary.SharedKernel.ServiceResult;

namespace BookingService.Application.Interfaces
{
    public interface IInstructorRoutesUseCase
    {
        Task<Result<PaginatedList<InstructorRouteDTO>>> GetRoutes(Guid instructorId);
        Task<Result<InstructorRouteDTO>> GetRoute(Guid id);
        Task<Result<bool>> CreateRoute(InstructorRouteDTO instructorRouteDTO,Guid instructorId);
        Task<Result<bool>> UpdateRoute(Guid id, InstructorRouteDTO instructorRouteDTO);
        Task<Result<bool>> DeleteRoute(Guid id);
    }
}


