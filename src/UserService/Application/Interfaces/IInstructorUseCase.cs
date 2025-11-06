using SharedLibrary.SharedKernel.Pagination;
using SharedLibrary.SharedKernel.ServiceResult;
using UserService.Application.Commons.DTOs.Instructors;
using UserService.Application.Commons.DTOs.Instructors.Registration;
using UserService.Domain.Enum;

namespace UserService.Application.Interfaces
{
    public interface IInstructorUseCase
    {
        Task<Result<PaginatedList<InstructorDTO>>> GetInstructors(InstructorListFilterDTO filter);

        Task<Result<InstructorDTO>> GetInstructorDetail(Guid id);

        Task<Result<List<InstructorScheduleDTO>>> GetInstructorSchedule(Guid instructor_id);

        Task<Result<bool>> RegisterInstructor(RegistrationDTO instructor_registration);

        Task<Result<ApplicationDTO>> GetInstructorApplication(Guid instructor_id);

        Task<Result<List<ApplicationDTO>>> GetAllInstructorApplicationsByStatus(ApplicationStatus status);

        Task<Result<ApplicationDTO>> GetApplicationById(Guid application_id);

        Task<Result<ApplicationTrackingDTO>> GetInstructorApplicationNewestTracking(Guid instructor_id);

        Task<Result<bool>> UpdateInstructorApplication(Guid instructor_id, RegistrationDTO application_patch);

        Task<Result<bool>> ModerateInstructorApplication(Guid application_id, string action, string inspector_note);
    }
}
