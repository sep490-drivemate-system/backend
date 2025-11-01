using AutoMapper;
using BookingService.Application.Commons.Constants;
using BookingService.Application.Commons.DTOs.DrivingSkills;
using BookingService.Application.Interfaces;
using BookingService.Domain.Entities;
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

        public async Task<Result<bool>> CreateNewDrivingSkill(string name)
        {
            try
            {
                await _unitOfWork.SkillRepository.CreateAsync(new DrivingSkill { Name = name, IllustrationUrl = "" });
                await _unitOfWork.CommitChangesAsync();

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}\nStack trace:\n{ex.StackTrace}\nHelp link: {ex.HelpLink}");
                return Result<bool>.Success(false, Messages.Commons.UNHANDLED);
            }
        }
    }
}
