using Microsoft.AspNetCore.Mvc;
using SharedLibrary.SharedKernel.ServiceResult;
using UserService.Application.Commons.DTOs.Cars;
using UserService.Application.Interfaces;

namespace UserService.Controllers
{
    [Route("api/cars")]
    [ApiController]
    public class CarController(ICarUseCase usecase): ControllerBase
    {
        private readonly ICarUseCase _usecase = usecase;

        [HttpGet]
        public async Task<IActionResult> GetCars([FromQuery] CarFilterDTO filter)
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


        [HttpGet("/instructor/{id}/cars")]
        public async Task<IActionResult> GetInstructorCars([FromRoute] Guid id)
        {
            var result = await _usecase.GetInstructorCarList(id);

            return result.ToActionResult();
        }
    }
}
