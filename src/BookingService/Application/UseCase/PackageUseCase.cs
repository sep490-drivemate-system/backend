using AutoMapper;
using BookingService.Application.Commons.Constants;
using BookingService.Application.Commons.DTOs.Booking;
using BookingService.Application.Commons.DTOs.Package;
using BookingService.Application.Interfaces;
using BookingService.Domain.Entities;
using BookingService.Domain.Enum;
using SharedLibrary.SharedKernel.Http.DTOs.Package;
using SharedLibrary.SharedKernel.Http.Interfaces;
using SharedLibrary.SharedKernel.ServiceResult;
using System.Linq.Expressions;

namespace BookingService.Application.UseCase
{
    public class PackageUseCase : IPackageUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPayment _payment;

        public PackageUseCase(IUnitOfWork unitOfWork, IMapper mapper, IPayment payment)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _payment = payment; 
        }

        public async Task<Result<IEnumerable<Package>>> GetAllPackagesAsync()
        {

            var packages = await _unitOfWork.PackageRepository.GetAllAsync();
            return Result<IEnumerable<Package>>.Success(packages);

        }

        public async Task<Result<Package?>> GetPackageByIdAsync(Guid id)
        {

            var package = await _unitOfWork.PackageRepository.GetByIdAsync(id);
            return Result<Package?>.Success(package);

        }

        public async Task<Result<Package>> CreatePackageAsync(Package package)
        {
            var createdPackage = await _unitOfWork.PackageRepository.CreateAsync(package);

            try
            {
                await _unitOfWork.CommitChangesAsync();
            }
            catch (Exception ex)
            {

                return Result<Package>.Failure(ServiceError.UnhandledException(Messages.Commons.UNHANDLED), Messages.Commons.UNHANDLED);
            }

            return Result<Package>.Success(createdPackage);

        }

        public async Task<Result<Package>> UpdatePackageAsync(Package package)
        {
            _unitOfWork.PackageRepository.Update(package);

            try
            {
                await _unitOfWork.CommitChangesAsync();
            }
            catch (Exception ex)
            {

                return Result<Package>.Failure(ServiceError.UnhandledException(Messages.Commons.UNHANDLED), Messages.Commons.UNHANDLED);
            }

            return Result<Package>.Success(package);
        }

        public async Task<Result<bool>> DeletePackageAsync(Guid id)
        {
            _unitOfWork.PackageRepository.Remove(id);

            try
            {
                await _unitOfWork.CommitChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}\nStacktrace:\n{ex.StackTrace}\nHelp link: {ex.HelpLink}");

                return Result<bool>.Success(false, Messages.Commons.UNHANDLED);
            }

            return Result<bool>.Success(true);

        }

        public async Task<Result<List<PackageDto>>> GetInstructorPackagesAsync(Guid instructorId)
        {
            var packages = await _unitOfWork.PackageRepository.GetInstructorPackages(instructorId);

            var packageDtos = _mapper.Map<List<PackageDto>>(packages);

            return Result<List<PackageDto>>.Success(packageDtos);
        }

        public async Task<Result<Booking>> BuyPackageAsync(PackageBuyingDTO packageBuyingDTO)
        {
            Guid id = Guid.NewGuid();
            var walletCheckResponse = await _payment.CheckWalletBooking(
               packageBuyingDTO.DriverId,
               packageBuyingDTO.PriceAtBuyingTime,
              id);

            if (!walletCheckResponse.IsPayment)
            {
                return Result<Booking>.Failure(ServiceError.BadRequestError(Messages.Booking.INSUFFICENTCREDIT));
            }
            var booking = _mapper.Map<Booking>(packageBuyingDTO);
            booking.Id = id;
            booking.Status = BookingStatus.Purchased;
            var createdBooking = await _unitOfWork.BookingRepository.CreateAsync(booking);
            await _unitOfWork.CommitChangesAsync();

            return Result<Booking>.Success(createdBooking);
            
        }
    }
}
