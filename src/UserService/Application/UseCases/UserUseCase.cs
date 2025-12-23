using AutoMapper;
using Resend;
using SharedLibrary.CloudinaryStorage;
using SharedLibrary.Email;
using SharedLibrary.SharedKernel.Enum;
using SharedLibrary.SharedKernel.Http.DTOs.User;
using SharedLibrary.SharedKernel.Pagination;
using SharedLibrary.SharedKernel.Password;
using SharedLibrary.SharedKernel.ServiceResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using UserService.Application.Commons.Constants;
using UserService.Application.Commons.DTOs.Auth;
using UserService.Application.Commons.DTOs.Users;
using UserService.Application.Interfaces;
using UserService.Domain.Entities;
using UserService.Domain.Enum;

namespace UserService.Application.UseCases
{
    public class UserUseCase(IUnitOfWork unitOfWork, IPasswordHasherService passwordHasher, ICloudinaryServiceProvider cloudinary, IMapper mapper, IEmailService emailService): IUserUseCase
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IPasswordHasherService _passwordHasherService = passwordHasher;
        private readonly ICloudinaryServiceProvider _cloudinaryService = cloudinary;
        private readonly IMapper _mapper = mapper;
        private readonly IEmailService _emailService = emailService;

        public async Task<Result<bool>> CreateDefaultUserAccount(UserCreationDTO user_information)
        {
            if ((await _unitOfWork.UserRepository.GetAllAsync(filter: x => x.Email.ToLower() == user_information.Email.ToLower())).Any())
            {
                return Result<bool>.Failure(ServiceError.ExistedError($"{user_information.Email}"), Messages.Auth.EmailAlreadyExists);
            }

            if ((await _unitOfWork.UserRepository.GetAllAsync(filter: x => x.PhoneNumber == user_information.PhoneNumber)).Any())
            {
                return Result<bool>.Failure(ServiceError.ExistedError($"{user_information.PhoneNumber}"), Messages.Auth.PhoneAlreadyExists);
            }

            // Create a default user account with provided information
            User new_user = new User
            {
                HashedPassword = await _passwordHasherService.HashPassword(user_information.Password), // why is hashing the password requires it to be asynchronous ?
                Avatar = "", // default as blank avatar
                Fullname = user_information.Fullname,
                Email = user_information.Email,
                PhoneNumber = user_information.PhoneNumber,
                DateOfBirth = user_information.DateOfBirth,
                Gender = user_information.Gender,
                Role = user_information.Role,
                AccountStatus = AccountStatus.Normal,
                MaxLicenseLevel = DrivingLicenseTier.C,
            };

            // create default novice driver or instructor record based on role.
            switch (user_information.Role)
            {
                case SharedLibrary.SharedKernel.Enum.UserRole.NoviceDriver:

                    new_user.NoviceDriver = new NoviceDriver
                    {
                        DrivingLicense = "", // default as blank driving license.
                        DrivingLicenseExpirationDate = DateOnly.MaxValue,
                    };
                    break;
                case SharedLibrary.SharedKernel.Enum.UserRole.Instructor:
                    new_user.Instructor = new Instructor
                    {
                        Bio = "", // default
                        Experience = 1, // default 
                        Status = InstructorStatus.Active,
                    };
                    break;
                default:
                    break;
            }

            // Send email or something here

            try
            {
                await _unitOfWork.UserRepository.CreateAsync(new_user);
                await _unitOfWork.CommitChangesAsync();
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure(ServiceError.UnhandledException($"{ex.Message}"), Messages.Common.UnknownError);
            }

            return Result<bool>.Success(true, Messages.Common.Success);
        }

        public async Task<Result<PaginatedList<UserDetailDTO>>> GetAllUser(UserFilterDTO filter)
        {
            Expression<Func<User, bool>> filterExpression = x => !x.IsDeleted
             && (String.IsNullOrEmpty(filter.searchKey) || 
                (x.PhoneNumber.ToLower().Contains(filter.searchKey.ToLower()) 
                    || x.Fullname.ToLower().Contains(filter.searchKey)
                    || x.Email.ToLower().Contains(filter.searchKey)))
             && (filter.Status == null || x.AccountStatus == filter.Status)
             && (filter.Role == null || x.Role == filter.Role);
            
            var filterdList = await _unitOfWork.UserRepository.GetAllAsync(
                filter: filterExpression,
                orderBy: q => q.OrderByDescending(x => x.CreatedAt),
                include_properties: "Instructor,NoviceDriver");

            PaginatedList<UserDetailDTO> mappedList = PaginatedList<UserDetailDTO>
                .Create(filterdList.Select(x => new UserDetailDTO
                {
                    UserId = x.Id,
                    FullName = x.Fullname,
                    AvatarUrl = x.Avatar,
                    AccountStatus = (int)x.AccountStatus,
                    Email = x.Email,
                    BirthDate = x.DateOfBirth,
                    Phone = x.PhoneNumber,
                    LicenseTier = x.MaxLicenseLevel ?? DrivingLicenseTier.B,
                    Role = x.Role,
                    NoviceDriver = x.NoviceDriver == null ? null : new NoviceDriverDetailDTO
                    {
                        NoviceDriverId = x.NoviceDriver.Id,
                        DrivingLicense = x.NoviceDriver.DrivingLicense,
                        DrivingLicenseExpirationDate = x.NoviceDriver.DrivingLicenseExpirationDate,
                    },
                    Instructor = x.Instructor == null ? null : new InstructorDetailDTO
                    {
                        InstructorId = x.Id,
                        Bio = x.Instructor.Bio,
                        ExperienceYear = x.Instructor.Experience,
                    }
                }).ToList(), filter.Page, filter.PageSize);

            return Result<PaginatedList<UserDetailDTO>>.Success(mappedList);
        }

        public async Task<Result<bool>> BanUser(Guid userId, BanUserDTO request)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);

            if (user == null || user.IsDeleted)
            {
                return Result<bool>.Failure(ServiceError.NotFoundError($"{userId}"), Messages.User.UserNotFound);
            }

            if (!string.Equals(user.Email, user.Email, StringComparison.OrdinalIgnoreCase))
            {
                return Result<bool>.Failure(ServiceError.BadRequestError($"{user.Email}"), Messages.Common.UnknownError);
            }

            user.AccountStatus = AccountStatus.Banned;
            user.LastModifiedAt = DateTime.Now;
            _unitOfWork.UserRepository.Update(user);
            await _unitOfWork.CommitChangesAsync();

            var placeholders = new Dictionary<string, string>
            {
                { "username", user.Fullname ?? user.Email },
                { "reason", string.IsNullOrWhiteSpace(request.Reason) ? "Tài khoản bị khóa bởi quản trị viên." : request.Reason }
            };

            var emailSent = await _emailService.SendingEmail(
                user.Email,
                placeholders,
                "Tài khoản của bạn đã bị khóa",
                EmailType.BanUser);

            if (!emailSent)
            {
                return Result<bool>.Failure(new ServiceError(ServiceError.Unhandled, Messages.Common.EmailError));
            }

            return Result<bool>.Success(true, Messages.Common.Success);
        }

        public async Task<Result<UserDetailDTO>> GetUser(Guid userId)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId, include_properties: "Instructor,NoviceDriver");

            if (user == null)
            {
                return Result<UserDetailDTO>.Failure(ServiceError.NotFoundError($"{userId}"), Messages.Common.NotFoundError);
            }

            var mappedUser = _mapper.Map<UserDetailDTO>(user);

            return Result<UserDetailDTO>.Success(mappedUser);
        }

        public async Task<Result<bool>> UpdatePersonalProfile(Guid id, UserProfileUpdateDTO user_profile)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id); 
        
            if (user == null)
            {
                return Result<bool>.Failure(ServiceError.NotFoundError($"id {id}"), Messages.Common.NotFoundError);
            }

            if (user_profile.ProfileAvatar != null) { user.Avatar = _cloudinaryService.UploadImageFormFileResourceToCloudinaryWithExactName(user_profile.ProfileAvatar ,user.Avatar?.Split("/").Last() ?? $"{Guid.NewGuid()}-avatar"); }
            if (user_profile.Fullname != null) user.Fullname = user_profile.Fullname;
            if (user_profile.PhoneNumber != null) user.PhoneNumber = user_profile.PhoneNumber;
            if (user_profile.Email != null) user.Email = user_profile.Email;
            if (user_profile.Password != null) user.HashedPassword = await _passwordHasherService.HashPassword(user_profile.Password);


            // Handling emergency contact update
            var emergencyContact = await _unitOfWork.Repository<EmergencyContact>().GetAllAsync(x => x.UserId == id);
            var closedEmergencyContact = emergencyContact.OrderByDescending(x => x.LastModifiedAt).FirstOrDefault();

            if (closedEmergencyContact == null)
            {
                // Create default
                closedEmergencyContact = new EmergencyContact
                {
                    SavedName = "",
                    ContactNumber = "",
                    UserId = id,
                };

                await _unitOfWork.Repository<EmergencyContact>().CreateAsync(closedEmergencyContact);
            }

            // Update if phone or number is updated
            if (user_profile.EmergencyContactName != null) closedEmergencyContact.SavedName = user_profile.EmergencyContactName;
            if (user_profile.EmergencyContactPhone != null) closedEmergencyContact.ContactNumber = user_profile.EmergencyContactPhone;

            await _unitOfWork.CommitChangesAsync();
           
            return Result<bool>.Success(true);
        }


        #region Saved Address
        public async Task<Result<IEnumerable<EmergencyContactDTO>>> GetUserEmergencyContacts(Guid user_id)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(user_id, include_properties: "EmergencyContacts");

            if (user == null)
            {
                return Result<IEnumerable<EmergencyContactDTO>>.Failure(ServiceError.NotFoundError($"{user_id}"), Messages.Common.NotFoundError);
            }

            return Result<IEnumerable<EmergencyContactDTO>>.Success(user.EmergencyContacts.OrderByDescending(x => x.LastModifiedAt).Select(x => new EmergencyContactDTO
            {
                Id = x.Id,
                Name = x.SavedName,
                Phone = x.ContactNumber
            }).Take(1));
        }

        public async Task<Result<bool>> CreateUserSavedAddress(Guid user_id, UserAddressDTO user_address)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(user_id, include_properties: "SavedLocations");

            if (user == null)
            {
                return Result<bool>.Failure(ServiceError.NotFoundError($"{user_id}"), Messages.User.UserNotFound);
            }

            await _unitOfWork.Repository<SavedLocation>().CreateAsync(new SavedLocation
            {
                UserId = user_id,
                DisplayName = user_address.AddressString,
                LocationLatitude = user_address.Latitude,
                LocationLongtitude = user_address.Longitude
            });
            await _unitOfWork.CommitChangesAsync();

            return Result<bool>.Success(true);
        }
        #endregion

        #region Emergency Contacts
        public async Task<Result<IEnumerable<UserAddressDTO>>> GetUserSavedAddress(Guid user_id)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(user_id, include_properties: "SavedLocations");

            if (user == null)
            {
                return Result<IEnumerable<UserAddressDTO>>.Failure(ServiceError.NotFoundError($"{user_id}"), Messages.Common.NotFoundError);
            }

            var addresses = _mapper.Map<IEnumerable<UserAddressDTO>>(user.SavedLocations);

            return Result<IEnumerable<UserAddressDTO>>.Success(addresses);
        }

        public async Task<Result<bool>> CreateUserEmergencyContacts(Guid user_id, EmergencyContactDTO emergency_contact)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(user_id, include_properties: "EmergencyContacts");

            if (user == null)
            {
                return Result<bool>.Failure(ServiceError.NotFoundError($"{user_id}"), Messages.User.UserNotFound);
            }

            await _unitOfWork.Repository<EmergencyContact>().CreateAsync(new EmergencyContact
            {
                UserId = user_id,
                SavedName = emergency_contact.Name,
                ContactNumber = emergency_contact.Phone,
            });
            await _unitOfWork.CommitChangesAsync();

            return Result<bool>.Success(true);
        }


        public async Task<Result<bool>> UpdateUserEmergencyContact(Guid contact_id, EmergencyContactDTO emergency_contact)
        {
            var emergency_detail = await _unitOfWork.Repository<EmergencyContact>().GetByIdAsync(contact_id);

            if (emergency_detail == null || emergency_detail.IsDeleted)
            {
                return Result<bool>.Failure(ServiceError.NotFoundError($"contact not found for id {contact_id}"), Messages.Common.NotFoundError);
            }

            emergency_detail.SavedName = emergency_contact.Name;
            emergency_detail.ContactNumber = emergency_contact.Phone;

            await _unitOfWork.CommitChangesAsync();

            return Result<bool>.Success(true);
        }
        #endregion

        #region GET methods for other services
        public async Task<Result<IEnumerable<UserDetailDTO>>> GetUserWithInstructorId(IEnumerable<Guid> instructor_ids)
        {
            Expression<Func<User, bool>> filter_expression = x => x.Role == SharedLibrary.SharedKernel.Enum.UserRole.Instructor && instructor_ids.Contains(x.Instructor.Id);
            string included_properties = "Instructor,NoviceDriver";
            var query_result = await _unitOfWork.UserRepository.GetAllAsync(filter: filter_expression, include_properties:  included_properties);

            return Result<IEnumerable<UserDetailDTO>>.Success(query_result.Select(x => new UserDetailDTO
            {
                UserId = x.Id,
                FullName = x.Fullname,
                AvatarUrl = x.Avatar,
                BirthDate = x.DateOfBirth,
                Email = x.Email,
                Phone = x.PhoneNumber,
                Role = x.Role,
                Instructor = x.Role == SharedLibrary.SharedKernel.Enum.UserRole.Instructor ? new InstructorDetailDTO
                {
                    InstructorId = x.Instructor.Id,
                    Bio = x.Instructor.Bio,
                    ExperienceYear = x.Instructor.Experience
                } : null,
                NoviceDriver = null,
                LicenseTier = x.MaxLicenseLevel ?? DrivingLicenseTier.B,
            }));
        }

        public async Task<Result<IEnumerable<UserDetailDTO>>> GetUserWithNoviceDriverId(IEnumerable<Guid> driver_ids)
        {
            Expression<Func<User, bool>> filter_expression = x => x.Role == SharedLibrary.SharedKernel.Enum.UserRole.Instructor && driver_ids.Contains(x.Instructor.Id);
            string included_properties = "Instructor,NoviceDriver";
            var query_result = await _unitOfWork.UserRepository.GetAllAsync(filter: filter_expression, include_properties: included_properties);

            return Result<IEnumerable<UserDetailDTO>>.Success(query_result.Select(x => new UserDetailDTO
            {
                UserId = x.Id,
                FullName = x.Fullname,
                AvatarUrl = x.Avatar,
                BirthDate = x.DateOfBirth,
                Email = x.Email,
                Phone = x.PhoneNumber,
                Role = x.Role,
                Instructor = null,
                NoviceDriver = x.Role == SharedLibrary.SharedKernel.Enum.UserRole.NoviceDriver ? new NoviceDriverDetailDTO
                {
                    NoviceDriverId = x.NoviceDriver.Id
                } : null,
                LicenseTier = x.MaxLicenseLevel ?? DrivingLicenseTier.B,
            }));
        }

        public async Task<Result<IEnumerable<UserDetailDTO>>> GetUserWithUserId(IEnumerable<Guid> user_ids)
        {
            Expression<Func<User, bool>> filter_expression = x => user_ids.Contains(x.Id);
            string included_properties = "Instructor,NoviceDriver";
            var query_result = await _unitOfWork.UserRepository.GetAllAsync(filter: filter_expression, include_properties: included_properties);

            var mappedUsers = _mapper.Map<IEnumerable<UserDetailDTO>>(query_result);

            return Result<IEnumerable<UserDetailDTO>>.Success(mappedUsers);
        }

        public async Task<Dictionary<Guid, InstructorBasicInfoDTO>> GetBatchInstructorBasicInfo(List<Guid> instructorIds)
        {
            if (instructorIds == null || !instructorIds.Any())
            {
                return new Dictionary<Guid, InstructorBasicInfoDTO>();
            }

            Expression<Func<User, bool>> filter = x => 
                x.Role == UserRole.Instructor && 
                instructorIds.Contains(x.Instructor.Id) &&
                !x.IsDeleted;

            var users = await _unitOfWork.UserRepository.GetAllAsync(
                filter: filter,
                include_properties: "Instructor"
            );

            return users.ToDictionary(
                user => user.Instructor.Id,
                user => new InstructorBasicInfoDTO
                {
                    InstructorId = user.Instructor.Id,
                    Fullname = user.Fullname,
                    AvatarUrl = user.Avatar ?? ""
                }
            );
        }

        public async Task<Dictionary<Guid, NoviceDriverBasicInfoDTO>> GetBatchNoviceDriverBasicInfo(List<Guid> noviceDriverIds)
        {
            if (noviceDriverIds == null || !noviceDriverIds.Any())
            {
                return new Dictionary<Guid, NoviceDriverBasicInfoDTO>();
            }

            Expression<Func<User, bool>> filter = x => 
                x.Role == UserRole.NoviceDriver && 
                noviceDriverIds.Contains(x.NoviceDriver.Id) &&
                !x.IsDeleted;

            var users = await _unitOfWork.UserRepository.GetAllAsync(
                filter: filter,
                include_properties: "NoviceDriver"
            );

            return users.ToDictionary(
                user => user.NoviceDriver.Id,
                user => new NoviceDriverBasicInfoDTO
                {
                    NoviceDriverId = user.NoviceDriver.Id,
                    Fullname = user.Fullname,
                    AvatarUrl = user.Avatar ?? ""
                }
            );
        }
        #endregion

        #region Statistic
        public async Task<Result<UserStatisticDTO>> GetUsersStatistic(UserStatisticFilterDTO filter)
        {
            Func<User, bool> queryFilter;
            IEnumerable<User> users;

            switch (filter.Type)
            {
                case StatisticTimeType.Yearly:
                    queryFilter = x => x.CreatedAt.Year == filter.Year;
                    break;
                case StatisticTimeType.Monthly:
                    queryFilter = x => x.CreatedAt.Year == filter.Year && x.CreatedAt.Month == filter.Month;
                    break;
                case StatisticTimeType.Weekly:
                    queryFilter = x => x.CreatedAt.Year == filter.Year && x.CreatedAt.Month == filter.Month && ((x.CreatedAt.Day - 1) / 7) + 1 == filter.Week;
                    break;
                default:
                    return Result<UserStatisticDTO>.Failure(ServiceError.BadRequestError($"{filter.Type}"), Messages.Common.ActionNotSupported);
            }

            // This is a really expensive query!!! (Does not need this if we follow the old plan)
            users = await _unitOfWork.UserRepository.GetAllAsync(include_properties: "NoviceDriver,Instructor");

            // Filtered for statistic
            IEnumerable<User> filteredUsers = users.Where(queryFilter);

            UserStatisticDTO summarizedStatistic = new UserStatisticDTO
            {
                Type = filter.Type,
                Year = filter.Year,
                Month = filter.Month,
                Week = filter.Week,
                TotalDriverCount = users.Where(x => !x.IsDeleted && x.Role == UserRole.NoviceDriver).Count(),
                TotalInstructorCount = users.Where(x => !x.IsDeleted && x.Role == UserRole.Instructor).Count(),
                TotalInspectorCount = users.Where(x => !x.IsDeleted && x.Role == UserRole.Inspector).Count(),
                TotalUserCount = users.Where(x => !x.IsDeleted).Count(),
                NewDriverCount = filteredUsers.Where(x => !x.IsDeleted).Count(),
                NewInstructorCount = filteredUsers.Where(x => !x.IsDeleted).Count(),
                NewUserCount = filteredUsers.Where(x => !x.IsDeleted).Count(),
                DeletedDriverCount = filteredUsers.Where(x => x.IsDeleted && x.Role == UserRole.NoviceDriver).Count(),
                DeletedInstructorCount = filteredUsers.Where(x => x.IsDeleted && x.Role != UserRole.Instructor).Count(),
                DeletedUserCount = filteredUsers.Where(x => x.IsDeleted).Count()
            };

            summarizedStatistic.NewDriverPercentage = summarizedStatistic.TotalDriverCount > 0 ? (double) summarizedStatistic.NewDriverCount / summarizedStatistic.TotalDriverCount : 0;
            summarizedStatistic.NewInstructorPercentage = summarizedStatistic.TotalInstructorCount > 0 ? (double) summarizedStatistic.NewInstructorCount / summarizedStatistic.TotalInstructorCount : 0;
            summarizedStatistic.NewUserPercentage = summarizedStatistic.TotalUserCount > 0 ? (double) summarizedStatistic.NewUserCount / summarizedStatistic.TotalUserCount : 0;

            summarizedStatistic.NoviceDriverStatistic = users.Where(x => x.Role == UserRole.NoviceDriver).GroupBy(u => u.MaxLicenseLevel.ToString()).ToDictionary(u => u.Key, u => (double)u.Count() / summarizedStatistic.TotalDriverCount);
            summarizedStatistic.InstructorsStatistic = users.Where(x => x.Role == UserRole.Instructor).GroupBy(u => u.MaxLicenseLevel.ToString()).ToDictionary(u => u.Key, u => (double)u.Count() / summarizedStatistic.TotalInstructorCount);

            summarizedStatistic.UserRoleCount = users.Where(x => !x.IsDeleted).GroupBy(u => u.Role.ToString()).ToDictionary(u => u.Key, u => u.Count());
            summarizedStatistic.UserRolePercentage = users.Where(x => !x.IsDeleted).GroupBy(u => u.Role.ToString()).ToDictionary(u => u.Key, u => (double) u.Count() / summarizedStatistic.TotalUserCount);

            summarizedStatistic.UserStatusCount = users.Where(x => !x.IsDeleted).GroupBy(u => u.AccountStatus.ToString()).ToDictionary(u => u.Key, u => u.Count());
            summarizedStatistic.UserStatusPercentage = users.Where(x => !x.IsDeleted).GroupBy(u => u.AccountStatus.ToString()).ToDictionary(u => u.Key, u => (double) u.Count() / summarizedStatistic.TotalUserCount);

            return Result<UserStatisticDTO>.Success(summarizedStatistic);
        }
        #endregion

        public async Task<Result<bool>> UnBanUser(Guid userId)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);

            if (user == null || user.IsDeleted)
            {
                return Result<bool>.Failure(ServiceError.NotFoundError($"{userId}"), Messages.User.UserNotFound);
            }

            if (!string.Equals(user.Email, user.Email, StringComparison.OrdinalIgnoreCase))
            {
                return Result<bool>.Failure(ServiceError.BadRequestError($"{user.Email}"), Messages.Common.UnknownError);
            }

            user.AccountStatus = AccountStatus.Normal;
            user.LastModifiedAt = DateTime.Now;
            _unitOfWork.UserRepository.Update(user);
            await _unitOfWork.CommitChangesAsync();

            var placeholders = new Dictionary<string, string>
            {
                { "username", user.Fullname ?? user.Email },
                { "reason", "Tài khoản bị khóa bởi quản trị viên." }
            };

            var emailSent = await _emailService.SendingEmail(
                user.Email,
                placeholders,
                "Tài khoản của bạn đã được mở khóa",
                EmailType.UnbanUser);

            if (!emailSent)
            {
                return Result<bool>.Failure(new ServiceError(ServiceError.Unhandled, Messages.Common.EmailError));
            }

            return Result<bool>.Success(true, Messages.Common.Success);
        }
    }
}
