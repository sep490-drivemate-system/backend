using SharedLibrary.SharedKernel.Http.Interfaces;
using SharedLibrary.SharedKernel.Pagination;
using SharedLibrary.SharedKernel.ServiceResult;
using UserService.Application.Commons.DTOs.Cars;
using UserService.Application.Commons.DTOs.Instructors;
using UserService.Application.Commons.Mapping.ExtentionMapping;
using UserService.Application.Interfaces;

namespace UserService.Application.UseCases
{
    public class InstructorUseCase(IUnitOfWork unitOfWork,IFeedback feedback) : IInstructorUseCase
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IFeedback _feedback = feedback;

        public async Task<Result<InstructorDetailDTO>> GetInstructorDetail(Guid id)
        {
            var instructor_info = await _unitOfWork.InstructorRepository.GetByIdAsync(id);

            if (instructor_info == null || instructor_info.IsDelete)
            {
                return Result<InstructorDetailDTO>.Failure(ServiceError.NotFoundError("can not find the requested resource"), $"can not find instructor information for {id}");
            }

            InstructorDetailDTO parsed_instructor_info = new InstructorDetailDTO
            {
                Id = id,
                FullName = instructor_info.User.UserName, // Requires changing domain model!
                ExperienceYear = instructor_info.Experience,
                Avatar = instructor_info.User.Avatar,
                Bio = instructor_info.Bio,
                Gender = "", // Requires changing domain model!
                Birthdate = instructor_info.User.DateOfBirth,
                IssueDateOfLicense = DateTime.MinValue, // Requires changing domain model!
                RegistrationDate = instructor_info.CreatedAt,
                Feedbacks = new List<InstructorFeedbackDTO>(), // Requires calling to booking service!
                Packages = new List<InstructorPackageDTO>(), // Requires calling to booking service!
                UnitPrice = 0, // Requires changing domain model!
                BookingCount = 0, // Requires calling to booking service!
                AverageRating = 0, // Requires calling to booking service!
            };

            return Result<InstructorDetailDTO>.Success(parsed_instructor_info, "success");
        }

        public async Task<Result<PaginatedList<InstructorDTO>>> GetInstructors(InstructorListFilterDTO filter)
        {
            var instructors = await _unitOfWork.InstructorRepository.GetAllAsync();
            var feedbacks = await _feedback.GetStatiticFeedback(instructors.Select(i => i.Id).ToList());

            var mappedData = MappingFeedback.MapInstructorsWithFeedback(instructors, feedbacks);

            return Result<PaginatedList<InstructorDTO>>.Success(
                PaginatedList<InstructorDTO>.Create(mappedData.AsQueryable(), filter.PageNumber, filter.PageSize)
            );
        }

        public async Task<Result<List<InstructorScheduleDTO>>> GetInstructorSchedule(Guid instructor_id)
        {
            // Check if the instructor exist
            var target_instructor = await _unitOfWork.InstructorRepository.GetByIdAsync(instructor_id);

            if (target_instructor == null)
            {
                return Result<List<InstructorScheduleDTO>>
                    .Failure(ServiceError.NotFoundError(Commons.Constants.Messages.Common.NotFoundError));
            }


            var schedule = await _unitOfWork.ScheduleRepository.GetAllAsync();

            return Result<List<InstructorScheduleDTO>>
                .Success(schedule.Select(x => new InstructorScheduleDTO
                {
                    Id = x.Id,
                    Date = x.Date,
                }).ToList());
        }
    }
}
