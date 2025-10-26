using AutoMapper;
using BookingService.Application.Commons.DTOs.DrivingSkills;
using BookingService.Application.Interfaces;
using SharedLibrary.SharedKernel.ServiceResult;

namespace BookingService.Application.UseCase
{
    public class DrivingSkillUseCase(IUnitOfWork unitOfWork,IMapper mapper) : IDrivingSkillUseCase
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<Result<List<DrivingSkillDTO>>> GetAllDrivingSkills(bool include_removed = false)
        {
            var skills = await _unitOfWork.SkillRepository.GetAllDrivingSkill();
            var skillDTOs = _mapper.Map<List<DrivingSkillDTO>>(skills);
            return Result<List<DrivingSkillDTO>>.Success(skillDTOs);
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
