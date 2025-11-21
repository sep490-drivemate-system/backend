using BookingService.Application.Commons.DTOs.Package;
using BookingService.Application.Interfaces;
using BookingService.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Jwt;
using SharedLibrary.SharedKernel.ServiceResult;
using BookingService.Application.Commons.DTOs.Package;
using Microsoft.AspNetCore.Authorization;
using SharedLibrary.SharedKernel.Enum;

namespace BookingService.Controllers
{
    [ApiController]
    [Route("api/package")]
    public class PackageController : ControllerBase
    {
        private readonly IPackageUseCase _packageUseCase;
        private readonly IJwtService _jwtService;

        public PackageController(IPackageUseCase packageUseCase,IJwtService jwtService)
        {
            _packageUseCase = packageUseCase;
            _jwtService = jwtService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPackages(PackageListFilterDTO filter)
        {
            var result = await _packageUseCase.GetAllPackagesAsync(filter);
            return result.ToActionResult();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPackage(Guid id)
        {
            var result = await _packageUseCase.GetPackageByIdAsync(id);
            return result.ToActionResult();
        }

        [HttpPost]
        [Authorize(Roles = nameof(UserRole.Instructor))]
        public async Task<IActionResult> CreatePackage([FromBody] PackageCreationDTO package)
        {
            package.InstructorId = await _jwtService.ExtractUserIdFromToken(Request.Headers["Authorization"].ToString());
            var result = await _packageUseCase.CreatePackageAsync(package);
            return result.ToActionResult();
        }

        [HttpPost("buy-package")]
        [Authorize(Roles = nameof(UserRole.NoviceDriver))]
        public async Task<IActionResult> BuyPackage([FromBody] PackageBuyingDTO packageBuyingDTO)
        {
            var driverId = await _jwtService.ExtractUserIdFromToken(Request.Headers["Authorization"].ToString());
            var result = await _packageUseCase.BuyPackageAsync(packageBuyingDTO, driverId);
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
