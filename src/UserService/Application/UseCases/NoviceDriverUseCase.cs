using AutoMapper;
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
    public class NoviceDriverUseCase(IUnitOfWork unitOfWork,IMapper mapper): INoviceDriverUseCase
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
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

        
        
    }
}
