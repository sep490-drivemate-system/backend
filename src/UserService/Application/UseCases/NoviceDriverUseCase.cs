using SharedLibrary.SharedKernel.ServiceResult;
using UserService.Application.Commons.DTOs.NoviceDriver;
using UserService.Application.Interfaces;

namespace UserService.Application.UseCases
{
    public class NoviceDriverUseCase(IUnitOfWork unitOfWork): INoviceDriverUseCase
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result<List<UserAddressDTO>>> GetNoviceDriverAddress(Guid id)
        {
            var driver = await _unitOfWork.NoviceDriverRepository.GetByIdAsync(id);

            if (driver == null)
            {
                return Result<List<UserAddressDTO>>
                    .Failure(ServiceError.NotFoundError(Commons.Constants.Messages.Common.NotFoundError));
            }

            return Result<List<UserAddressDTO>>.Success(driver.User.Addresses.Select(x => new UserAddressDTO
            {
                Id = x.Id,
                AddressString = x.Location
            }).ToList()); 
        }
    }
}
