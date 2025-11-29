using BookingService.Application.Commons.DTOs.Brands;
using SharedLibrary.SharedKernel.ServiceResult;

namespace BookingService.Application.Interfaces
{
    public interface IBrandUseCase
    {
        Task<Result<bool>> CreateNewBrand(BrandCreationDTO brand_info);
        Task<Result<IEnumerable<BrandDTO>>> GetAllBrand();
        Task<Result<BrandDTO>> GetBrandDetail(Guid id);
        Task<Result<bool>> UpdateBrand(Guid id, BrandCreationDTO brand);
        Task<Result<bool>> DeleteBrand(Guid id);
    }
}
