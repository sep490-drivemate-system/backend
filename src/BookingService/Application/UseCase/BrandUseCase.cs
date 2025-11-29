using BookingService.Application.Commons.Constants;
using BookingService.Application.Commons.DTOs.Brands;
using BookingService.Application.Commons.DTOs.RoadTypes;
using BookingService.Application.Interfaces;
using BookingService.Domain.Entities;
using SharedLibrary.SharedKernel.ServiceResult;
using System.Linq.Expressions;

namespace BookingService.Application.UseCase
{
    public class BrandUseCase(IUnitOfWork unitOfWork): IBrandUseCase
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result<bool>> CreateNewBrand(BrandCreationDTO brand_info)
        {
            try
            {
                Expression<Func<Manufacturer, bool>> filter = x => x.Name.ToLower() == brand_info.Name.ToLower() && !x.IsDeleted;
                var exstingSkills = await _unitOfWork.ManufacturerRepository.GetAllAsync(filter);

                if (exstingSkills.Count != 0)
                {
                    return Result<bool>.Failure(ServiceError.ExistedError($"{brand_info.Name}"), Messages.Commons.UNHANDLED);
                }

                await _unitOfWork.ManufacturerRepository.CreateAsync(new Manufacturer { Name = brand_info.Name });
                await _unitOfWork.CommitChangesAsync();
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure(ServiceError.UnhandledException($"{ex.Message}"), Messages.Commons.UNHANDLED);
            }

            return Result<bool>.Success(true);
        }

        public async Task<Result<bool>> DeleteBrand(Guid id)
        {
            var brand = await _unitOfWork.ManufacturerRepository.GetByIdAsync(id);

            if (brand == null || brand.IsDeleted)
            {
                return Result<bool>.Failure(ServiceError.NotFoundError($"{id}"), Messages.Commons.NOTFOUND);
            }

            brand.IsDeleted = true;
            _unitOfWork.ManufacturerRepository.Update(brand);
            await _unitOfWork.CommitChangesAsync();
            return Result<bool>.Success(true, Messages.Commons.SUCCESS);
        }

        public async Task<Result<IEnumerable<BrandDTO>>> GetAllBrand()
        {
            var brands = await _unitOfWork.ManufacturerRepository.GetAllAsync(filter: x => !x.IsDeleted);
            return Result<IEnumerable<BrandDTO>>.Success(brands.Select(x => new BrandDTO { Id = x.Id, Name = x.Name}),Messages.Commons.SUCCESS);
        }

        public async Task<Result<BrandDTO>> GetBrandDetail(Guid id)
        {
            var brand = await _unitOfWork.ManufacturerRepository.GetByIdAsync(id);
            if (brand != null && !brand.IsDeleted)
            {
                return Result<BrandDTO>.Success(new BrandDTO { Id = id,Name = brand.Name });
            }

            return Result<BrandDTO>.Failure(ServiceError.NotFoundError($"{id}"), Messages.Commons.NOTFOUND);
        }

        public async Task<Result<bool>> UpdateBrand(Guid id, BrandCreationDTO brand_info)
        {
            var brand = await _unitOfWork.ManufacturerRepository.GetByIdAsync(id);

            if (brand == null || brand.IsDeleted)
            {
                return Result<bool>.Failure(ServiceError.NotFoundError($"{id}"), Messages.Commons.NOTFOUND);
            }

            brand.Name = brand_info.Name;

            try
            {
                _unitOfWork.ManufacturerRepository.Update(brand);
                await _unitOfWork.CommitChangesAsync();
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure(ServiceError.UnhandledException(ex.Message), Messages.Commons.UNHANDLED);
            }

            return Result<bool>.Success(true, Messages.Commons.SUCCESS);
        }
    }
}
