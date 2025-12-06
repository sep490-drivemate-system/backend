using AutoMapper;
using SharedLibrary.CloudinaryStorage;
using SharedLibrary.SharedKernel.Http.DTOs.Feedback;
using SharedLibrary.SharedKernel.ServiceResult;
using System;
using System.Linq;
using UserService.Application.Commons.Constants;
using UserService.Application.Commons.DTOs.Users;
using UserService.Application.Interfaces;
using UserService.Domain.Entities;

namespace UserService.Application.UseCases
{
    public class NoviceDriverUseCase(IUnitOfWork unitOfWork, ICloudinaryServiceProvider cloudinary, IMapper mapper): INoviceDriverUseCase
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ICloudinaryServiceProvider _cloudinary = cloudinary;
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
    }
}
