using Microsoft.AspNetCore.Mvc;
using BookingService.Domain.Entities;
using BookingService.Application.Interfaces;
using SharedLibrary.SharedKernel.ServiceResult;

namespace BookingService.Controllers
{
    [ApiController]
    [Route("api/package")]
    public class PackageController : ControllerBase
    {
        private readonly IPackageUseCase _packageService;

        public PackageController(IPackageUseCase packageService)
        {
            _packageService = packageService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPackages()
        {
            var result = await _packageService.GetAllPackagesAsync();
            return result.ToActionResult();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPackage(Guid id)
        {
            var result = await _packageService.GetPackageByIdAsync(id);
            return result.ToActionResult();
        }

        [HttpPost]
        public async Task<IActionResult> CreatePackage([FromBody] Package package)
        {
            var result = await _packageService.CreatePackageAsync(package);
            return result.ToActionResult();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePackage(Guid id, [FromBody] Package package)
        {
            if (id != package.Id)
            {
                return BadRequest("ID mismatch");
            }

            var result = await _packageService.UpdatePackageAsync(package);
            return result.ToActionResult();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePackage(Guid id)
        {
            var result = await _packageService.DeletePackageAsync(id);
            return result.ToActionResult();
        }
    }
}
