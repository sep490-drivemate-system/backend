using BookingService.Application.Commons.DTOs.Cars.Create;
using BookingService.Application.Commons.DTOs.Cars.Get;
using BookingService.Application.Commons.DTOs.Cars.Update;
using BookingService.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Jwt;
using SharedLibrary.SharedKernel.Enum;
using SharedLibrary.SharedKernel.ServiceResult;

namespace BookingService.Controllers
{
    [Route("api/car")]
    [ApiController]
    public class CarController : ControllerBase
    {
        private readonly ICarUseCase _usecase;
        private readonly IJwtService _jwtService;

        public CarController(ICarUseCase usecases, IJwtService jwtService)
        {
            _usecase = usecases;
            _jwtService = jwtService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCarList([FromQuery] CarListFilterDTO filter)
        {
            var result = await _usecase.GetCarPaginatedList(filter);
            return result.ToActionResult();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCarDetail([FromRoute] Guid id)
        {
            var result = await _usecase.GetCarDetail(id);
            return result.ToActionResult();
        }

        [HttpGet("recommendation")]
        public async Task<IActionResult> GetRecommendedCars([FromQuery] int max_count = 10)
        {
            var result = await _usecase.GetRecommendedCarList(max_count);
            return result.ToActionResult();
        }

        [HttpGet("instructor/cars")]
        [Authorize(Roles = nameof(UserRole.Instructor))]
        public async Task<IActionResult> GetInstructorCars()
        {
            var instructorId = await _jwtService.ExtractUserIdFromToken(Request.Headers["Authorization"].ToString());
            var result = await _usecase.GetInstructorCarWithUserId(instructorId);
            return result.ToActionResult();
        }

        
        [HttpGet("instructor/{id}/cars")]
        public async Task<IActionResult> GetInstructorCars([FromRoute] Guid id)
        {
            var result = await _usecase.GetInstructorCarList(id);
            return result.ToActionResult();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCar([FromForm] CarCreationDTO car)
        {
            var result = await _usecase.CreateNewCar(car);
            return result.ToActionResult();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCar([FromRoute] Guid id, [FromForm] CarUpdateDTO car)
        {
            var result = await _usecase.UpdateCarInformation(id, car);
            return result.ToActionResult();
        }

        // Support partial update of a car entity. Will be implemented later
        [HttpPatch("{id}")] 
        public async Task<IActionResult> PartialUpdateCar([FromRoute] Guid id)
        {
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteACar([FromRoute] Guid id)
        {
            var result = await _usecase.DeleteCar(id);
            return result.ToActionResult();
        }
    }
}
