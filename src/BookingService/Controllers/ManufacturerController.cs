using BookingService.Application.Commons.DTOs.Brands;
using BookingService.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.SharedKernel.ServiceResult;
using System.Threading.Tasks;

namespace BookingService.Controllers
{
    [Route("api/manufacturers")]
    [ApiController]
    public class ManufacturerController(IBrandUseCase usecase) : ControllerBase
    {
        private readonly IBrandUseCase _brandUseCase = usecase;

        [HttpGet]
        public async Task<IActionResult> GetAllManufacturer()
        {
            var result = await _brandUseCase.GetAllBrand();
            return result.ToActionResult();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetManufacturerDetail([FromRoute] Guid id)
        {
            var result = await _brandUseCase.GetBrandDetail(id);
            return result.ToActionResult();
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewManufacturer([FromBody] BrandCreationDTO brand_info)
        {
            var result = await _brandUseCase.CreateNewBrand(brand_info);
            return result.ToActionResult();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateManufacturerDetail([FromRoute] Guid id, [FromBody] BrandCreationDTO brand_info)
        {
            var result = await _brandUseCase.UpdateBrand(id, brand_info);
            return result.ToActionResult();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteManufacturer([FromRoute] Guid id)
        {
            var result = await _brandUseCase.DeleteBrand(id);
            return result.ToActionResult();
        }
    }
}
