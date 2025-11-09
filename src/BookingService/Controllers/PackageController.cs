using BookingService.Application.Commons.DTOs.Package;
using BookingService.Application.Interfaces;
using BookingService.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.SharedKernel.ServiceResult;

namespace BookingService.Controllers
{
    [ApiController]
    [Route("api/package")]
    public class PackageController : ControllerBase
    {
        private readonly IPackageUseCase _packageUseCase;

        public PackageController(IPackageUseCase packageUseCase)
        {
            _packageUseCase = packageUseCase;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPackages()
        {
            var result = await _packageUseCase.GetAllPackagesAsync();
            return result.ToActionResult();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPackage(Guid id)
        {
            var result = await _packageUseCase.GetPackageByIdAsync(id);
            return result.ToActionResult();
        }

        [HttpPost]
        public async Task<IActionResult> CreatePackage([FromBody] Package package)
        {
            var result = await _packageUseCase.CreatePackageAsync(package);
            return result.ToActionResult();
        }

        [HttpPost("buy-package")]
        public async Task<IActionResult> BuyPackage([FromBody] PackageBuyingDTO packageBuyingDTO)
        {
            var result = await _packageUseCase.BuyPackageAsync(packageBuyingDTO);
            return result.ToActionResult();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePackage(Guid id, [FromBody] Package package)
        {
            if (id != package.Id)
            {
                return BadRequest("ID mismatch");
            }

            var result = await _packageUseCase.UpdatePackageAsync(package);
            return result.ToActionResult();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePackage(Guid id)
        {
            var result = await _packageUseCase.DeletePackageAsync(id);
            return result.ToActionResult();
        }

        [HttpGet("instructor/{id}")]
        public async Task<IActionResult> GetInstructorPackages(Guid id)
        {
            var result = await _packageUseCase.GetInstructorPackagesAsync(id);
            return result.ToActionResult();
        }
    }
}
