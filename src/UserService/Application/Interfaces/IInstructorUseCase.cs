using SharedLibrary.SharedKernel.Pagination;
using SharedLibrary.SharedKernel.ServiceResult;
using UserService.Application.Commons.DTOs.Instructors;

namespace UserService.Application.Interfaces
{
    public interface IInstructorUseCase
    {
        Task<Result<PaginatedList<InstructorDTO>>> GetInstructorPaginatedList(InstructorFilterDTO filter);

        Task<Result<InstructorDetailDTO>> GetInstructorDetail(Guid id);
    }
}
