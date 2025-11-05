using SharedLibrary.SharedKernel.Http.Interfaces;
using SharedLibrary.SharedKernel.Pagination;
using SharedLibrary.SharedKernel.ServiceResult;
using UserService.Application.Commons.DTOs.Instructors;
using UserService.Application.Commons.Mapping.ExtentionMapping;
using UserService.Application.Interfaces;
using SharedLibrary.SharedKernel.Http.DTOs.Package;
using UserService.Application.Commons.DTOs.Instructors.Registration;
using UserService.Domain.Enum;
using UserService.Application.Commons.Constants;
using UserService.Domain.Entities;
using SharedLibrary.SharedKernel.Enum;
using SharedLibrary.CloudinaryStorage;
using SharedLibrary.SharedKernel.Password;
using System.Linq.Expressions;
using System.Reflection;

namespace UserService.Application.UseCases
{
    public class InstructorUseCase(IUnitOfWork unitOfWork, ICloudinaryServiceProvider cloudinary, IPasswordHasherService passwordHasher, IFeedback feedback, IPackage package) : IInstructorUseCase
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ICloudinaryServiceProvider _cloudinary = cloudinary;
        private readonly IPasswordHasherService _passwordHasher = passwordHasher;
        private readonly IFeedback _feedback = feedback;
        private readonly IPackage _package = package;

        public async Task<Result<List<ApplicationDTO>>> GetAllInstructorApplicationsByStatus(ApplicationStatus status)
        {
            Expression<Func<InstructorApplication, bool>> filter_expression = x => x.Status == status;
            Func<IQueryable<InstructorApplication>, IOrderedQueryable<InstructorApplication>> order_expression = x => x.OrderBy(u => u.SubmitAt);
            string included_properties = "Instructors,Instructors.User,ApplicationTrackings";

            var application_list = await _unitOfWork.ApplicationRepository.GetAllAsync(filter: filter_expression, orderBy: order_expression, include_properties: included_properties);

            return Result<List<ApplicationDTO>>.Success(application_list.Select(x => new ApplicationDTO
            {
                ApplicationId = x.Id,
                InstructorId = x.InstructorId,
                Avatar = x.Instructors?.User?.Avatar ?? "",
                Fullname = x.Fullname,
                Email = x.EmailAddress,
                Phone = x.PhoneNumber,
                BirthDate = x.DateOfBirth,
                Gender = x.Gender.ToString(),
                SumbitDate = x.SubmitAt,
                DrivingLicenseFront = x.DrivingLicenseFront,
                DrivingLicenseBack = x.DrivingLicenseBack,
                TeachingLicenseFront = x.TeachingLicenseFront,
                TeachingLicenseBack = x.TeachingLicenseBack,
                HealthCheckup = x.HealthCheckup,
                PersonalProfile = x.BackgroundProfile,
                TrackingHistories = x.ApplicationTrackings?.Select(x => new ApplicationTrackingDTO
                {
                    Id = x.Id,
                    Note = x.Note,
                    Status = x.Status.ToString(),
                }).ToList()
            }).ToList());
        }

        public async Task<Result<ApplicationDTO>> GetApplicationById(Guid application_id)
        {
            string included_properties = "Instructors,Instructors.User,ApplicationTrackings";

            var application_detail = await _unitOfWork.ApplicationRepository.GetByIdAsync(application_id, include_properties: included_properties);

            if (application_detail == null)
            {
                return Result<ApplicationDTO>.Failure(ServiceError.NotFoundError($"{application_id}"), Messages.Common.NotFoundError);
            }

            return Result<ApplicationDTO>.Success(new ApplicationDTO
            {
                ApplicationId = application_detail.Id,
                InstructorId = application_detail.InstructorId,
                Avatar = application_detail.Instructors?.User?.Avatar ?? "",
                Fullname = application_detail.Fullname,
                Email = application_detail.EmailAddress,
                Phone = application_detail.PhoneNumber,
                BirthDate = application_detail.DateOfBirth,
                Gender = application_detail.Gender.ToString(),
                SumbitDate = application_detail.SubmitAt,
                DrivingLicenseFront = application_detail.DrivingLicenseFront,
                DrivingLicenseBack = application_detail.DrivingLicenseBack,
                TeachingLicenseFront = application_detail.TeachingLicenseFront,
                TeachingLicenseBack = application_detail.TeachingLicenseBack,
                HealthCheckup = application_detail.HealthCheckup,
                PersonalProfile = application_detail.BackgroundProfile,
                TrackingHistories = application_detail.ApplicationTrackings?.Select(x => new ApplicationTrackingDTO
                {
                    Id = x.Id,
                    Note = x.Note,
                    Status = x.Status.ToString(),
                }).ToList()
            });
        }

        public async Task<Result<ApplicationDTO>> GetInstructorApplication(Guid instructor_id)
        {
            Expression<Func<InstructorApplication, bool>> filter_expression = x => x.InstructorId == instructor_id;
            string included_properties = "Instructors,Instructors.User,ApplicationTrackings";

            var application_result = await _unitOfWork.ApplicationRepository.GetAllAsync(filter: filter_expression, include_properties: included_properties);

            if (application_result.Count < 1)
            {
                return Result<ApplicationDTO>.Failure(ServiceError.NotFoundError($"{instructor_id}"), Messages.Common.NotFoundError);
            }

            InstructorApplication application_detail = application_result[0];

            return Result<ApplicationDTO>.Success(new ApplicationDTO
            {
                ApplicationId = application_detail.Id,
                InstructorId = application_detail.InstructorId,
                Avatar = application_detail.Instructors?.User?.Avatar ?? "",
                Fullname = application_detail.Fullname,
                Email = application_detail.EmailAddress,
                Phone = application_detail.PhoneNumber,
                BirthDate = application_detail.DateOfBirth,
                Gender = application_detail.Gender.ToString(),
                SumbitDate = application_detail.SubmitAt,
                DrivingLicenseFront = application_detail.DrivingLicenseFront,
                DrivingLicenseBack = application_detail.DrivingLicenseBack,
                TeachingLicenseFront = application_detail.TeachingLicenseFront,
                TeachingLicenseBack = application_detail.TeachingLicenseBack,
                HealthCheckup = application_detail.HealthCheckup,
                PersonalProfile = application_detail.BackgroundProfile,
                TrackingHistories = application_detail.ApplicationTrackings?.Select(x => new ApplicationTrackingDTO
                {
                    Id = x.Id,
                    Note = x.Note,
                    Status = x.Status.ToString(),
                }).ToList()
            });
        }

        public async Task<Result<ApplicationTrackingDTO>> GetInstructorApplicationNewestTracking(Guid instructor_id)
        {
            Expression<Func<ApplicationTracking, bool>> filter_expression = x => x.InstructorApplication.InstructorId == instructor_id;
            Func<IQueryable<ApplicationTracking>, IOrderedQueryable<ApplicationTracking>> order_expression = x => x.OrderByDescending(u => u.CreatedAt);
            string included_properties = "InstructorApplication";

            var application_tracking_list = await _unitOfWork.Repository<ApplicationTracking>().GetAllAsync(filter_expression, order_expression, included_properties);

            if (application_tracking_list.Count == 0)
            {
                return Result<ApplicationTrackingDTO>.Failure(ServiceError.NotFoundError($"{instructor_id}"), Messages.Common.NotFoundError);
            }

            var latest_tracking = application_tracking_list[0];

            return Result<ApplicationTrackingDTO>.Success(new ApplicationTrackingDTO
            {
                Id = latest_tracking.Id,
                Note = latest_tracking.Note,
                Status = latest_tracking.Status.ToString(),
            });
        }

        public async Task<Result<InstructorDetailDTO>> GetInstructorDetail(Guid id)
        {
            var instructor_info = await _unitOfWork.InstructorRepository.GetByIdAsync(id);

            if (instructor_info == null || instructor_info.IsDeleted)
            {
                return Result<InstructorDetailDTO>.Failure(ServiceError.NotFoundError("can not find the requested resource"), $"can not find instructor information for {id}");
            }

            // Get feedback statistics for this instructor
            var feedbackResponse = await _feedback.GetStatiticFeedback(new List<Guid> { id });
            var instructorFeedback = feedbackResponse?.InstructorStatistics?.FirstOrDefault(f => f.InstructorId == id);

            // Get packages overview for this instructor
            var overViewPackages = await _package.GetOverViewPackages(id);

            InstructorDetailDTO parsed_instructor_info = new InstructorDetailDTO
            {
                Id = id,
                FullName = instructor_info.User.Username,
                ExperienceYear = instructor_info.Experience,
                Avatar = instructor_info.User.Avatar,
                Bio = instructor_info.Bio,
                Gender = instructor_info.User.Gender, // Requires changing domain model!
                Birthdate = instructor_info.User.DateOfBirth,
                //Feedbacks = new List<InstructorFeedbackDTO>(), // Requires calling to booking service!
                //Packages = overViewPackages.Select(p => new InstructorPackageDTO
                //{
                //    Name = p.Name,
                //    Description = p.Description,
                //    Price = p.MinPrice == p.MaxPrice ? p.MinPrice : p.MinPrice 
                //}).ToList(),
                BookingCount = instructorFeedback?.BookingCount ?? 0,
                AverageRating = instructorFeedback?.AverageRating ?? 0,
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
                    StartTime = x.StartTime,
                    EndTime = x.EndTime,
                }).ToList());
        }

        public async Task<Result<bool>> ModerateInstructorApplication(Guid application_id, string action, string inspector_note)
        {
            string included_properties = "Instructors,Instructors.User";
            var application = await _unitOfWork.ApplicationRepository.GetByIdAsync(application_id, included_properties);

            if (application == null)
            {
                return Result<bool>.Failure(ServiceError.NotFoundError($"{application_id}"), Messages.Common.NotFoundError);
            }

            if (application.Status != ApplicationStatus.Pending && application.Status != ApplicationStatus.ReApplying)
            {
                return Result<bool>.Failure(ServiceError.AlreadyProcessedError($"{application_id}"), Messages.Common.ActionNotSupported);
            }

            switch (action)
            {
                case "reject":
                    application.Status = ApplicationStatus.Rejected;
                    await _unitOfWork.Repository<ApplicationTracking>().CreateAsync(new ApplicationTracking
                    {
                        ApplicationId = application_id,
                        Note = inspector_note,
                        Status = ApplicationStatus.Rejected,
                    });
                    break;
                case "approve":
                    application.Status = ApplicationStatus.Approved;
                    application.Instructors.Status = InstructorStatus.Active;
                    await _unitOfWork.Repository<ApplicationTracking>().CreateAsync(new ApplicationTracking
                    {
                        ApplicationId = application_id,
                        Note = inspector_note,
                        Status = ApplicationStatus.Approved,
                    });
                    break;
                default:
                    return Result<bool>.Failure(ServiceError.BadRequestError($"{action}"), Messages.Common.ActionNotSupported);
            }

            _unitOfWork.ApplicationRepository.Update(application);
            await _unitOfWork.CommitChangesAsync();

            return Result<bool>.Success(true);
        }

        public async Task<Result<bool>> RegisterInstructor(RegistrationDTO instructor_registration)
        {
            PropertyInfo[] registration_fields_info = instructor_registration.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            // Check for required fields
            foreach (PropertyInfo field in registration_fields_info)
            {
                if (!field.CanRead)
                {
                    continue;
                }

                object? value = field.GetValue(instructor_registration);

                if (value == null)
                {
                    return Result<bool>.Failure(ServiceError.BadRequestError($"{field.Name}"), Messages.Common.RequiredField);
                }

                if (field.PropertyType == typeof(string))
                {
                    if (string.IsNullOrEmpty((string)value))
                    {
                        return Result<bool>.Failure(ServiceError.BadRequestError($"{field.Name}"), Messages.Common.RequiredField);
                    }
                }
            }

            // Check for existing accounts (or applications)
            Expression<Func<InstructorApplication, bool>> filter_expression = x => x.EmailAddress == instructor_registration.Email;
            var existing_applicants = await _unitOfWork.ApplicationRepository.GetAllAsync(filter: filter_expression);

            if (existing_applicants.Count > 0)
            {
                return Result<bool>.Failure(ServiceError.BadRequestError($"{instructor_registration.Email}"), Messages.InstructorApplication.EmailUsed);
            }

            // Checking for valid content types
            // Too lazy to check for other files, might complete this later :3
            if (!instructor_registration.Avatar?.ContentType.StartsWith("image") ?? true)
            {
                return Result<bool>.Failure(ServiceError.BadRequestError($"Avatar: {instructor_registration.Avatar.ContentType}"), Messages.Common.InvalidDocumentType);
            }

            if (!instructor_registration.DrivingLicenseFront?.ContentType.StartsWith("image") ?? true)
            {
                return Result<bool>.Failure(ServiceError.BadRequestError($"Driving license (front):{instructor_registration.DrivingLicenseFront.ContentType}"), Messages.Common.InvalidDocumentType);
            }

            if (!instructor_registration.DrivingLicenseBack?.ContentType.StartsWith("image") ?? true)
            {
                return Result<bool>.Failure(ServiceError.BadRequestError($"Driving license (back):{instructor_registration.DrivingLicenseFront.ContentType}"), Messages.Common.InvalidDocumentType);
            }

            // Checking for valid license tier.
            if (!Enum.TryParse(instructor_registration.TeachingTier, out DrivingLicenseTier instructor_license_tier))
            {
                return Result<bool>.Failure(ServiceError.BadRequestError($"{instructor_registration.TeachingTier}"), Messages.InstructorApplication.InvalidDrivingLicenseTier);
            }

            // Checking for valid gender choice.
            if (!Enum.TryParse(instructor_registration.Gender, out GenderType instructor_gender))
            {
                return Result<bool>.Failure(ServiceError.BadRequestError($"{instructor_registration.TeachingTier}"), Messages.InstructorApplication.InvalidGender);
            }

            // Save all user uploaded resource to Cloudinary through provider wrapper.
            string avatar_url = /*$"{instructor_registration.Email}_avatar";*/ _cloudinary.UploadImageFormFileResourceToCloudinary(instructor_registration.Avatar, $"{instructor_registration.Email}-avatar");
            string driving_license_front_url = /*$"{instructor_registration.Email}_driving_license_front";*/ _cloudinary.UploadImageFormFileResourceToCloudinary(instructor_registration.DrivingLicenseFront, $"{instructor_registration.Email}-driving-license-front");
            string driving_license_back_url = /*$"{instructor_registration.Email}_driving_license_back";*/_cloudinary.UploadImageFormFileResourceToCloudinary(instructor_registration.DrivingLicenseBack, $"{instructor_registration.Email}-driving-license-back");
            string teaching_license_front_url = /*$"{instructor_registration.Email}_teaching_license_front";*/_cloudinary.UploadImageFormFileResourceToCloudinary(instructor_registration.TeachingLicenseFront, $"{instructor_registration.Email}-teaching-license-front");
            string teaching_license_back_url = /*$"{instructor_registration.Email}_teaching_license_back";*/ _cloudinary.UploadImageFormFileResourceToCloudinary(instructor_registration.TeachingLicenseBack, $"{instructor_registration.Email}-teaching-license-back");
            string health_checkup_url = /*$"{instructor_registration.Email}_health_checkup";*/_cloudinary.UploadImageFormFileResourceToCloudinary(instructor_registration.HealthCheckup, $"{instructor_registration.Email}-health-checkup");
            string personal_porfolio_url = /*$"{instructor_registration.Email}_porfolio";*/_cloudinary.UploadImageFormFileResourceToCloudinary(instructor_registration.PersonalProfile, $"{instructor_registration.Email}-porfolio");

            Instructor instructor = new Instructor
            {
                Bio = "",
                Experience = 0,
                Status = InstructorStatus.Pending,
                User = new User
                {
                    Username = instructor_registration.Email,
                    Avatar = avatar_url,
                    PhoneNumber = instructor_registration.PhoneNumber,
                    Email = instructor_registration.Email,
                    Gender = instructor_gender,
                    HashedPassword = await _passwordHasher.HashPassword(instructor_registration.RawPassword), // Why do we have to make the hashing function asynchronous ???
                    AccountStatus = AccountStatus.Normal,
                    DateOfBirth = (DateOnly)instructor_registration.BirthDate,
                    MaxLicenseLevel = instructor_license_tier,
                    Role = UserRole.Instructor,
                },
                InstructorApplication = new InstructorApplication
                {
                    Fullname = instructor_registration.Fullname,
                    EmailAddress = instructor_registration.Email,
                    PhoneNumber = instructor_registration.PhoneNumber,
                    Gender = instructor_gender,
                    DateOfBirth = (DateOnly)instructor_registration.BirthDate,
                    SubmitAt = DateTime.Now,
                    Status = ApplicationStatus.Pending,
                    DrivingLicenseFront = driving_license_front_url,
                    DrivingLicenseBack = driving_license_back_url,
                    TeachingLicenseFront = teaching_license_front_url,
                    TeachingLicenseBack = teaching_license_back_url,
                    BackgroundProfile = personal_porfolio_url,
                    HealthCheckup = health_checkup_url,
                    DrivingLicenseTier = instructor_license_tier,
                }
            };

            await _unitOfWork.InstructorRepository.CreateAsync(instructor);
            await _unitOfWork.CommitChangesAsync();

            return Result<bool>.Success(true, Messages.Common.Success);
        }

        public Task<Result<bool>> UpdateInstructorApplication(Guid instructor_id, RegistrationDTO application_patch)
        {
            throw new NotImplementedException();
        }
    }
}
