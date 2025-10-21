using BookingService.Domain.Entities;

namespace BookingService.Domain.Interfaces
{
    public interface IDrivingSkillRepository
    {
        Task<List<DrivingSkill>> GetAllDrivingSkill();

        Task<DrivingSkill?> GetDrivingSkillById(Guid id);

        Task<DrivingSkill> CreateDrivingSkill(DrivingSkill info);

        Task<DrivingSkill> RemoveDrivingSkill(Guid id);

        Task<DrivingSkill> UpdateDrivingSkill(DrivingSkill info);
    }
}
