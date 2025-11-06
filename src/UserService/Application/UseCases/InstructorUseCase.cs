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
using Twilio.TwiML.Messaging;

namespace UserService.Application.UseCases
{
    public class InstructorUseCase(IUnitOfWork unitOfWork, ICloudinaryServiceProvider cloudinary, IPasswordHasherService passwordHasher, IHttpClientFactory http_client_factory) : IInstructorUseCase
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ICloudinaryServiceProvider _cloudinary = cloudinary;
        private readonly IPasswordHasherService _passwordHasher = passwordHasher;

        #region Instructor Details
        public async Task<Result<InstructorDTO>> GetInstructorDetail(Guid id)
        {
            string included_properties = "User";

            var instructor_info = await _unitOfWork.InstructorRepository.GetByIdAsync(id, included_properties);

            if (instructor_info == null || instructor_info.IsDeleted)
            {
                return Result<InstructorDTO>.Failure(ServiceError.NotFoundError($"{id}"), Messages.Common.NotFoundError);
            }

            /*
            // Using HttpClient to get Instructor feedbacks statistic.
            HttpClient booking_client = _httpClientFactory.CreateClient("BookingServiceClient");

            booking_client.GetAsync("");

            // Using HttpClient to get Instructor packages.

            // Get feedback statistics for this instructor using httpClient
            //var feedbackResponse = await _feedback.GetStatiticFeedback(new List<Guid> { id });
            //var instructorFeedback = feedbackResponse?.InstructorStatistics?.FirstOrDefault(f => f.InstructorId == id);

            // Get packages overview for this instructor
            //var overViewPackages = await _package.GetOverViewPackages(id);

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
            */

            return Result<InstructorDTO>.Success(new InstructorDTO
            {
                Id = instructor_info.Id,
                Bio = instructor_info.Bio,
                FullName = instructor_info.User?.Fullname ?? "",
                Gender = instructor_info.User.Gender == GenderType.Male ? "Nam" : "Nữ",
                Birthdate = instructor_info.User.DateOfBirth,
                ExperienceYear = instructor_info.Experience,
                Avatar = instructor_info.User?.Avatar ?? "",
                AverageRating = 0,
                BookingCount = 0
            }, Messages.Common.Success);
        }

        public async Task<Result<PaginatedList<InstructorDTO>>> GetInstructors(InstructorListFilterDTO filter)
        {
            Expression<Func<Instructor, bool>> filter_expression = x => (filter.DrivingLicenseTier == null || filter.DrivingLicenseTier <= x.User.MaxLicenseLevel)
            && (filter.Experience == null || filter.Experience == 0 || filter.Experience <= x.Experience)
            && (string.IsNullOrEmpty(filter.SearchKey) || x.User.Fullname.Contains(filter.SearchKey)) // What exactly are we trying to achieve here ???
            && !x.IsDeleted;
            string included_properties = "User";

            var instructors = await _unitOfWork.InstructorRepository.GetAllAsync(filter: filter_expression, orderBy: null, include_properties: included_properties);

            return Result<PaginatedList<InstructorDTO>>.Success(
                PaginatedList<InstructorDTO>.Create(instructors.Select(x => new InstructorDTO
                {
                    Id = x.Id,
                    Bio = x.Bio,
                    FullName = x.User.Fullname ?? "",
                    Birthdate = x.User.DateOfBirth,
                    ExperienceYear = x.Experience,
                    Gender = x.User.Gender.ToString(),
                    Avatar = x.User?.Avatar ?? "",
                    AverageRating = 0,
                    BookingCount = 0,
                }), filter.PageNumber, filter.PageSize)
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
        #endregion

        #region Instructor Applications
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
            if (!Enum.TryParse(instructor_registration.TeachingTier, true, out DrivingLicenseTier instructor_license_tier))
            {
                return Result<bool>.Failure(ServiceError.BadRequestError($"{instructor_registration.TeachingTier}"), Messages.InstructorApplication.InvalidDrivingLicenseTier);
            }

            // Checking for valid gender choice.
            if (!Enum.TryParse(instructor_registration.Gender, true, out GenderType instructor_gender))
            {
                return Result<bool>.Failure(ServiceError.BadRequestError($"{instructor_registration.Gender}"), Messages.InstructorApplication.InvalidGender);
            }

            // Save all user uploaded resource to Cloudinary through provider wrapper.
            string avatar_url = _cloudinary.UploadImageFormFileResourceToCloudinary(instructor_registration.Avatar, $"{instructor_registration.Email}-avatar");
            string driving_license_front_url = _cloudinary.UploadImageFormFileResourceToCloudinary(instructor_registration.DrivingLicenseFront, $"{instructor_registration.Email}-driving-license-front");
            string driving_license_back_url = _cloudinary.UploadImageFormFileResourceToCloudinary(instructor_registration.DrivingLicenseBack, $"{instructor_registration.Email}-driving-license-back");
            string teaching_license_front_url = _cloudinary.UploadImageFormFileResourceToCloudinary(instructor_registration.TeachingLicenseFront, $"{instructor_registration.Email}-teaching-license-front");
            string teaching_license_back_url = _cloudinary.UploadImageFormFileResourceToCloudinary(instructor_registration.TeachingLicenseBack, $"{instructor_registration.Email}-teaching-license-back");
            string health_checkup_url = _cloudinary.UploadImageFormFileResourceToCloudinary(instructor_registration.HealthCheckup, $"{instructor_registration.Email}-health-checkup");
            string personal_porfolio_url = _cloudinary.UploadImageFormFileResourceToCloudinary(instructor_registration.PersonalProfile, $"{instructor_registration.Email}-porfolio");

            Instructor instructor = new Instructor
            {
                Bio = "",
                Experience = 0,
                Status = InstructorStatus.Pending,
                User = new User
                {
                    Username = instructor_registration.Email,
                    Fullname = instructor_registration.Fullname,
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

            try
            {
                await _unitOfWork.InstructorRepository.CreateAsync(instructor);
                await _unitOfWork.CommitChangesAsync();
            }
            catch (Exception ex)
            {
                // If fails to save instructor application details, remove uploaded resources.
                //_cloudinary.DeleteResourceFromCloudinary();

                return Result<bool>.Failure(ServiceError.UnhandledException($"{ex.Message}"), Messages.Common.UnknownError);
            }

            return Result<bool>.Success(true, Messages.Common.Success);
        }

        public async Task<Result<bool>> UpdateInstructorApplication(Guid instructor_id, RegistrationDTO application_patch)
        {
            string included_properties = "Instructors,Instructors.User";

            var applications = await _unitOfWork.ApplicationRepository.GetAllAsync(x => x.InstructorId == instructor_id, include_properties: included_properties);

            InstructorApplication? instructor_application = applications.FirstOrDefault();

            if (instructor_application == null)
            {
                return Result<bool>.Failure(ServiceError.NotFoundError($"{instructor_id}"), Messages.Common.NotFoundError);
            }

            if (instructor_application.Status != ApplicationStatus.Rejected)
            {
                return Result<bool>.Failure(ServiceError.InvalidStateError($"{instructor_id}"), Messages.Common.ActionNotSupported);
            }

            instructor_application.Fullname = application_patch.Fullname ?? instructor_application.Fullname;

            if (application_patch.Email != null)
            {
                instructor_application.EmailAddress = application_patch.Email;
                instructor_application.Instructors.User.Username = application_patch.Email;
                instructor_application.Instructors.User.Email = application_patch.Email;
            }

            if (application_patch.PhoneNumber != null)
            {
                instructor_application.PhoneNumber = application_patch.PhoneNumber;
                instructor_application.Instructors.User.PhoneNumber = application_patch.PhoneNumber;
            }

            if (application_patch.BirthDate != null)
            {
                instructor_application.DateOfBirth = (DateOnly)application_patch.BirthDate;
                instructor_application.Instructors.User.DateOfBirth = (DateOnly)application_patch.BirthDate;
            }

            if (application_patch.Gender != null)
            {
                if (!Enum.TryParse<GenderType>(application_patch.Gender, true, out var gender))
                {
                    return Result<bool>.Failure(ServiceError.BadRequestError($"{application_patch.Gender}"), Messages.InstructorApplication.InvalidGender);
                }

                instructor_application.Gender = gender;
                instructor_application.Instructors.User.Gender = gender;
            }

            if (application_patch.TeachingTier != null)
            {
                if (!Enum.TryParse<DrivingLicenseTier>(application_patch.TeachingTier, true, out var tier))
                {
                    return Result<bool>.Failure(ServiceError.BadRequestError($"{application_patch.TeachingTier}"), Messages.InstructorApplication.InvalidDrivingLicenseTier);
                }

                instructor_application.DrivingLicenseTier = tier;
                instructor_application.Instructors.User.MaxLicenseLevel = tier;
            }

            if (application_patch.DrivingLicenseFront != null)
            {
                instructor_application.DrivingLicenseFront = _cloudinary.UploadImageFormFileResourceToCloudinary(application_patch.DrivingLicenseFront, $"{instructor_application.EmailAddress}-driving-license-front");
            }

            if (application_patch.DrivingLicenseBack != null)
            {
                instructor_application.DrivingLicenseBack = _cloudinary.UploadImageFormFileResourceToCloudinary(application_patch.DrivingLicenseBack, $"{instructor_application.EmailAddress}-driving-license-back");
            }

            if (application_patch.TeachingLicenseFront != null)
            {
                instructor_application.HealthCheckup = _cloudinary.UploadImageFormFileResourceToCloudinary(application_patch.DrivingLicenseFront, $"{instructor_application.EmailAddress}-teaching-license-front");
            }

            if (application_patch.TeachingLicenseBack != null)
            {
                instructor_application.HealthCheckup = _cloudinary.UploadImageFormFileResourceToCloudinary(application_patch.TeachingLicenseBack, $"{instructor_application.EmailAddress}-teaching-license-back");
            }

            if (application_patch.HealthCheckup != null)
            {
                instructor_application.HealthCheckup = _cloudinary.UploadImageFormFileResourceToCloudinary(application_patch.HealthCheckup, $"{instructor_application.EmailAddress}-health-checkup");
            }

            if (application_patch.PersonalProfile != null)
            {
                instructor_application.BackgroundProfile = _cloudinary.UploadImageFormFileResourceToCloudinary(application_patch.PersonalProfile, $"{instructor_application.EmailAddress}-portfolio");
            }

            instructor_application.SubmitAt = DateTime.Now;
            instructor_application.Status = ApplicationStatus.ReApplying;

            try
            {
                _unitOfWork.ApplicationRepository.Update(instructor_application);
                await _unitOfWork.CommitChangesAsync();
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure(ServiceError.UnhandledException($"{ex.Message}"), Messages.Common.UnknownError);
            }

            return Result<bool>.Success(true, Messages.Common.Success);
        }
        #endregion
    }
    
}
