using BookingService.Application.DTOs.DrivingSkills;
using SharedLibrary.SharedKernel.ServiceResult;

namespace BookingService.Application.Interfaces
{
    public interface IDrivingSkillService
    {
        Task<Result<List<DrivingSkillDTO>>> GetAllDrivingSkills(bool include_removed = false);

        Task<Result<DrivingSkillDTO>> GetDrivingSkillWithId(Guid id);
    }
}
