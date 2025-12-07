using AutoMapper;
using SharedLibrary.CloudinaryStorage;
using SharedLibrary.SharedKernel.Enum;
using SharedLibrary.SharedKernel.Http.DTOs.Feedback;
using SharedLibrary.SharedKernel.Password;
using SharedLibrary.SharedKernel.ServiceResult;
using System;
using System.Linq;
using UserService.Application.Commons.Constants;
using UserService.Application.Commons.DTOs.NoviceDriver;
using UserService.Application.Commons.DTOs.Users;
using UserService.Application.Interfaces;
using UserService.Domain.Entities;
using UserService.Domain.Enum;

namespace UserService.Application.UseCases
{
    public class NoviceDriverUseCase(IUnitOfWork unitOfWork, ICloudinaryServiceProvider cloudinary, IPasswordHasherService passwordHasher, IMapper mapper): INoviceDriverUseCase
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ICloudinaryServiceProvider _cloudinary = cloudinary;
        private readonly IPasswordHasherService _passwordHasher = passwordHasher;
        private readonly IMapper _mapper = mapper;

        public async Task<Result<bool>> HasValidDrivingLicenseAsync(Guid noviceDriverId)
        {
            var noviceDriver = await _unitOfWork.NoviceDriverRepository.GetByIdAsync(noviceDriverId);

            var hasLicenseDocument = !string.IsNullOrWhiteSpace(noviceDriver.DrivingLicense);
            var currentDate = DateOnly.FromDateTime(DateTime.UtcNow);
            var isLicenseValid = noviceDriver.DrivingLicenseExpirationDate >= currentDate;

            return Result<bool>.Success(hasLicenseDocument && isLicenseValid);
        }

        public async Task<Result<NoviceDriverInfoFeedbackDTO>> GetNoviceDriverInfoForFeedback(Guid noviceDriverId)
        {
            var driver = await _unitOfWork.NoviceDriverRepository.GetByIdWithUserAsync(noviceDriverId);

            if (driver == null)
            {
                return Result<NoviceDriverInfoFeedbackDTO>
                    .Failure(ServiceError.NotFoundError(Commons.Constants.Messages.Common.NotFoundError));
            }

            var result = new NoviceDriverInfoFeedbackDTO
            {
                Name = driver.User?.Fullname ?? "Unknown",
                Avatar = driver.User?.Avatar ?? string.Empty
            };

            return Result<NoviceDriverInfoFeedbackDTO>.Success(result);
        }

        public async Task<Result<string>> UpdateNoviceDriverDrivingLicense(Guid id, IFormFile driving_license)
        {
            var novice = await _unitOfWork.NoviceDriverRepository.GetByIdAsync(id);

            if (novice == null)
            {
                return Result<string>.Failure(ServiceError.NotFoundError($"{id}"), Messages.Common.NotFoundError);
            }

            if (!driving_license.ContentType.StartsWith("image"))
            {
                return Result<string>.Failure(ServiceError.UnprocessableEntityError($"{driving_license.ContentType}"), Messages.Common.UnknownError);
            }

            var image_url = _cloudinary.UploadImageFormFileResourceToCloudinaryWithExactName(driving_license, novice.DrivingLicense.Split("/").Last());

            novice.DrivingLicense = image_url;

            _unitOfWork.NoviceDriverRepository.Update(novice);
            await _unitOfWork.CommitChangesAsync();

            return Result<string>.Success(image_url);

        }

        public async Task<Result<bool>> RegistratingNoviceDriverAccount(NoviceDriverRegistrationDTO registration_info)
        {
            // Validating registration info
            if (await _unitOfWork.UserRepository.IsEsxitEmail(registration_info.Email))
            {
                return Result<bool>.Failure(ServiceError.ConflictError($"{registration_info.Email}"), "Email Existed");
            }

            // Creating user information
            User user = new User
            {
                Avatar = null, // User with no avatar
                Email = registration_info.Email,
                Username = registration_info.Email,
                Fullname = registration_info.Email,
                PhoneNumber = registration_info.PhoneNumber,
                AccountStatus = AccountStatus.Normal,
                Gender = GenderType.Male,
                Role = UserRole.NoviceDriver,
                HashedPassword = await _passwordHasher.HashPassword(registration_info.Password),
                DateOfBirth = DateOnly.FromDateTime(DateTime.Now),
                MaxLicenseLevel = DrivingLicenseTier.B,
                NoviceDriver = new NoviceDriver
                {
                    DrivingLicense = $"{Guid.NewGuid()}-license.jpg", // Default for new registered novice driver
                    DrivingLicenseExpirationDate = DateOnly.FromDateTime(DateTime.Now.AddYears(5)),
                }
            };

            // Persist changes to database
            await _unitOfWork.UserRepository.CreateAsync(user);
            await _unitOfWork.CommitChangesAsync();
            return Result<bool>.Success(true);
        }
    }
}
