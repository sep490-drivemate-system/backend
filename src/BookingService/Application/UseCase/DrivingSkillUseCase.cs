using AutoMapper;
using BookingService.Application.Commons.Constants;
using BookingService.Application.Commons.DTOs.DrivingSkills;
using BookingService.Application.Interfaces;
using BookingService.Domain.Entities;
using SharedLibrary.SharedKernel.ServiceResult;
using System.Linq.Expressions;

namespace BookingService.Application.UseCase
{
    public class DrivingSkillUseCase(IUnitOfWork unitOfWork,IMapper mapper) : IDrivingSkillUseCase
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<Result<List<DrivingSkillDTO>>> GetAllDrivingSkills(bool include_removed = false)
        {
            var skills = await _unitOfWork.SkillRepository.GetAllAsync(filter: x => !x.IsDeleted);
            var skillDTOs = _mapper.Map<List<DrivingSkillDTO>>(skills);
            return Result<List<DrivingSkillDTO>>.Success(skillDTOs);
        }

        public async Task<Result<DrivingSkillDTO>> GetDrivingSkillWithId(Guid id)
        {
            var skill = await _unitOfWork.SkillRepository.GetByIdAsync(id);

            if (skill != null && !skill.IsDeleted)
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
                // Check for existinfg name
                Expression<Func<DrivingSkill, bool>> filter = x => x.Name.ToLower() == name.ToLower() && !x.IsDeleted;
                var exstingSkills = await _unitOfWork.SkillRepository.GetAllAsync(filter);

                if (exstingSkills.Count != 0)
                {
                    return Result<bool>.Failure(ServiceError.ExistedError($"{name}"), Messages.Commons.UNHANDLED);
                }

                await _unitOfWork.SkillRepository.CreateAsync(new DrivingSkill { Name = name, IllustrationUrl = "" });
                await _unitOfWork.CommitChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}\nStack trace:\n{ex.StackTrace}\nHelp link: {ex.HelpLink}");
                return Result<bool>.Success(false, Messages.Commons.UNHANDLED);
            }

            return Result<bool>.Success(true);
        }

        public async Task<Result<bool>> UpdateDrivingSkill(Guid id, DrivingSkillCreationDTO driving_skill)
        {
            var skill = await _unitOfWork.SkillRepository.GetByIdAsync(id);

            if (skill == null || skill.IsDeleted)
            {
                return Result<bool>.Failure(ServiceError.NotFoundError($"{id}"), Messages.Commons.NOTFOUND);
            }

            skill.Name = driving_skill.SkillName;
            _unitOfWork.SkillRepository.Update(skill);
            await _unitOfWork.CommitChangesAsync();

            return Result<bool>.Success(true);
        }

        public async Task<Result<bool>> DeleteDrivingSkills(Guid id)
        {
            var skill = await _unitOfWork.SkillRepository.GetByIdAsync(id);

            if (skill == null || skill.IsDeleted)
            {
                return Result<bool>.Failure(ServiceError.NotFoundError($"{id}"), Messages.Commons.NOTFOUND);
            }

            skill.IsDeleted = true;
            _unitOfWork.SkillRepository.Update(skill);
            await _unitOfWork.CommitChangesAsync();

            return Result<bool>.Success(true);
        }
    }
}
