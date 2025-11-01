using BookingService.Application.Commons.DTOs.DrivingSkills;
using SharedLibrary.SharedKernel.ServiceResult;

namespace BookingService.Application.Interfaces
{
    public interface IDrivingSkillUseCase
    {
        Task<Result<List<DrivingSkillDTO>>> GetAllDrivingSkills(bool include_removed = false);

        Task<Result<DrivingSkillDTO>> GetDrivingSkillWithId(Guid id);

        Task<Result<bool>> CreateNewDrivingSkill(string name);
    }
}
