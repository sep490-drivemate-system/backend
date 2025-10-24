using BookingService.Application.Commons.DTOs.DrivingSkills;
using BookingService.Application.Interfaces;
using SharedLibrary.SharedKernel.ServiceResult;

namespace BookingService.Application.UseCase
{
    public class DrivingSkillService(IUnitOfWork unitOfWork) : IDrivingSkillService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result<List<DrivingSkillDTO>>> GetAllDrivingSkills(bool include_removed = false)
        {
            var roads = await _unitOfWork.SkillRepository.GetAllDrivingSkill();

            return Result<List<DrivingSkillDTO>>.Success(roads.Select(x => new DrivingSkillDTO
            {
                Id = x.Id,
                Name = x.Name
            }).ToList());
        }

        public async Task<Result<DrivingSkillDTO>> GetDrivingSkillWithId(Guid id)
        {
            var skill = await _unitOfWork.SkillRepository.GetDrivingSkillById(id);

            if (skill != null)
            {
                return Result<DrivingSkillDTO>.Success(new DrivingSkillDTO
                {
                    Id = id,
                    Name = skill.Name
                });
            }

            return Result<DrivingSkillDTO>.Failure(ServiceError.NotFoundError("Can not find driving skill"));
        }
    }
}
