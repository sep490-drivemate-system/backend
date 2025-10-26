using SharedLibrary.SharedKernel.Pagination;
using SharedLibrary.SharedKernel.ServiceResult;
using UserService.Application.Commons.DTOs.Cars;
using UserService.Application.Commons.DTOs.Instructors;
using UserService.Application.Interfaces;

namespace UserService.Application.UseCases
{
    public class InstructorUseCase(IUnitOfWork unitOfWork) : IInstructorUseCase
    {
        private IUnitOfWork _unitOfWork = unitOfWork;

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

        public async Task<Result<PaginatedList<InstructorDTO>>> GetInstructorPaginatedList(InstructorListFilterDTO filter)
        {
            var instructors = await _unitOfWork.InstructorRepository.GetAllAsync();

        var filtered_instructors = instructors;
        //.Where(x => (filter..Split(",").Contains(x.Manufacturer.Name) || string.IsNullOrEmpty(filter.Manufacturer)) &&
        //(filter.SeatCounts.Split(",").Select(z => int.Parse(z)).Contains(x.Seat) || string.IsNullOrEmpty(filter.SeatCounts)) &&
        //(filter.CarType.Split(",").Contains(x.CartType) || string.IsNullOrEmpty(filter.CarType)) &&
        //(filter.FuelType.Split(",").Contains(x.Fuel) || string.IsNullOrEmpty(filter.FuelType))).ToList();

        var parsed_instructors = filtered_instructors.Select(x => new InstructorDTO
        {
            Id = x.Id,
            Avatar = x.User.Avatar,
            FullName = x.User.UserName, // Requires domain model update!
            ExperienceYear = x.Experience,
            BookingCount = 0, // Requires calling to booking service!
            AverageRating = 0, // Requires calling to booking service!
            UnitPrice = 0, // Requires calling to booking service!,
        }).AsQueryable();

        PaginatedList<InstructorDTO> paginated_filtered_instructor = PaginatedList<InstructorDTO>.Create(parsed_instructors, filter.PageNumber, filter.PageSize);

            return Result<PaginatedList<InstructorDTO>>.Success(paginated_filtered_instructor, "success");
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
