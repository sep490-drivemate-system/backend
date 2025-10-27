using SharedLibrary.SharedKernel.Pagination;
using SharedLibrary.SharedKernel.ServiceResult;
using UserService.Application.Commons.DTOs.Instructors;

namespace UserService.Application.Interfaces
{
    public interface IInstructorUseCase
    {
        Task<Result<PaginatedList<InstructorDTO>>> GetInstructors(InstructorListFilterDTO filter);

        Task<Result<InstructorDetailDTO>> GetInstructorDetail(Guid id);

        Task<Result<List<InstructorScheduleDTO>>> GetInstructorSchedule(Guid instructor_id);
    }
}
