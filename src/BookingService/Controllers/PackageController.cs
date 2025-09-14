using Microsoft.AspNetCore.Mvc;
using BookingService.Domain.Entities;
using BookingService.Domain.Interfaces;

namespace BookingService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PackageController : ControllerBase
    {
        private readonly IPackageRepository _packageRepository;

        public PackageController(IPackageRepository packageRepository)
        {
            _packageRepository = packageRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Package>>> GetAllPackages()
        {
            var packages = await _packageRepository.GetAllAsync();
            return Ok(packages);
        }

        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<Package>>> GetActivePackages()
        {
            var packages = await _packageRepository.GetActivePackagesAsync();
            return Ok(packages);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Package>> GetPackage(Guid id)
        {
            var package = await _packageRepository.GetByIdAsync(id);
            if (package == null)
            {
                return NotFound();
            }
            return Ok(package);
        }

        [HttpPost]
        public async Task<ActionResult<Package>> CreatePackage(Package package)
        {
            var createdPackage = await _packageRepository.CreateAsync(package);
            return CreatedAtAction(nameof(GetPackage), new { id = createdPackage.Id }, createdPackage);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePackage(Guid id, Package package)
        {
            if (id != package.Id)
            {
                return BadRequest();
            }

            var updatedPackage = await _packageRepository.UpdateAsync(package);
            return Ok(updatedPackage);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePackage(Guid id)
        {
            await _packageRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
