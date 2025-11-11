using SharedLibrary.SharedKernel.Enum;
using SharedLibrary.SharedKernel.Http.DTOs.User;
using SharedLibrary.SharedKernel.Password;
using SharedLibrary.SharedKernel.ServiceResult;
using System.Linq.Expressions;
using UserService.Application.Commons.Constants;
using UserService.Application.Commons.DTOs.Auth;
using UserService.Application.Commons.DTOs.Users;
using UserService.Application.Interfaces;
using UserService.Domain.Entities;
using UserService.Domain.Enum;

namespace UserService.Application.UseCases
{
    public class UserUseCase(IUnitOfWork unitOfWork, IPasswordHasherService passwordHasher): IUserUseCase
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IPasswordHasherService _passwordHasherService = passwordHasher;

        public async Task<Result<bool>> CreateDefaultUserAccount(UserCreationDTO user_information)
        {
            if ((await _unitOfWork.UserRepository.GetAllAsync(filter: x => x.Username == user_information.Username)).Any())
            {
                return Result<bool>.Failure(ServiceError.ExistedError($"{user_information.Username}"), Messages.Auth.UserNameAlreadyExists);
            }

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
                Username = user_information.Username,
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

        public async Task<Result<IEnumerable<UserDetailDTO>>> GetUserWithInstructorId(IEnumerable<Guid> instructor_ids)
        {
            Expression<Func<User, bool>> filter_expression = x => x.Role == SharedLibrary.SharedKernel.Enum.UserRole.Instructor && instructor_ids.Contains(x.Instructor.Id);
            string included_properties = "Instructor,NoviceDriver";
            var query_result = await _unitOfWork.UserRepository.GetAllAsync(filter: filter_expression, include_properties:  included_properties);

            return Result<IEnumerable<UserDetailDTO>>.Success(query_result.Select(x => new UserDetailDTO
            {
                UserId = x.Id,
                Fullname = x.Fullname,
                AvatarUrl = x.Avatar,
                BirthDate = x.DateOfBirth,
                Role = x.Role,
                Instructor = x.Role == SharedLibrary.SharedKernel.Enum.UserRole.Instructor ? new InstructorDetailDTO
                {
                    InstructorId = x.Instructor.Id,
                    Bio = x.Instructor.Bio,
                    ExperienceYear = x.Instructor.Experience
                } : null,
                NoviceDriver = null,
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
                Fullname = x.Fullname,
                AvatarUrl = x.Avatar,
                BirthDate = x.DateOfBirth,
                Role = x.Role,
                Instructor = null,
                NoviceDriver = x.Role == SharedLibrary.SharedKernel.Enum.UserRole.NoviceDriver ? new NoviceDriverDetailDTO
                {
                    NoviceDriverId = x.NoviceDriver.Id
                } : null,
            }));
        }

        public async Task<Result<IEnumerable<UserDetailDTO>>> GetUserWithUserId(IEnumerable<Guid> user_ids)
        {
            Expression<Func<User, bool>> filter_expression = x => user_ids.Contains(x.Id);
            string included_properties = "Instructor,NoviceDriver";
            var query_result = await _unitOfWork.UserRepository.GetAllAsync(filter: filter_expression, include_properties: included_properties);

            return Result<IEnumerable<UserDetailDTO>>.Success(query_result.Select(x => new UserDetailDTO
            {
                UserId = x.Id,
                Fullname = x.Fullname,
                AvatarUrl = x.Avatar,
                BirthDate = x.DateOfBirth,
                Role = x.Role,
                Instructor = x.Role == SharedLibrary.SharedKernel.Enum.UserRole.Instructor ? new InstructorDetailDTO 
                { 
                    InstructorId = x.Instructor.Id,
                    Bio = x.Instructor.Bio,
                    ExperienceYear = x.Instructor.Experience
                } : null,
                NoviceDriver = x.Role == SharedLibrary.SharedKernel.Enum.UserRole.NoviceDriver ? new NoviceDriverDetailDTO
                {
                    NoviceDriverId = x.NoviceDriver.Id
                } : null,
            }));
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
                    Fullname = user.Username,
                    AvatarUrl = user.Avatar ?? ""
                }
            );
        }
    }
}
