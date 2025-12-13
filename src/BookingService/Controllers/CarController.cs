using BookingService.Application.Commons.DTOs.Car_Documents;
using BookingService.Application.Commons.DTOs.Cars.Create;
using BookingService.Application.Commons.DTOs.Cars.Get;
using BookingService.Application.Commons.DTOs.Cars.Update;
using BookingService.Application.Interfaces;
using BookingService.Domain.Entities;
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
        private readonly ILogger _logger;
        private readonly IJwtService _jwtService;

        public CarController(ICarUseCase usecases, IJwtService jwtService, ILogger<CarController> logger)
        {
            _usecase = usecases;
            _jwtService = jwtService;
            _logger = logger;
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

        [HttpGet("/api/packages/{id}/cars")]
        public async Task<IActionResult> GetCarForPackage([FromRoute] Guid id)
        {
            var result = await _usecase.GetAllCarsForPackage(id);
            return result.ToActionResult();
        }

        [HttpGet("{id}/feedbacks")]
        public async Task<IActionResult> GetCarFeedbacks([FromRoute] Guid id)
        {
            var result = await _usecase.GetCarFeedback(id);
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
            var result = await _usecase.GetInstructorCarList(instructorId);
            return result.ToActionResult();
        }

        [HttpGet("instructors/{id}/cars")]
        public async Task<IActionResult> GetInstructorCarsButADifferentOne([FromRoute] Guid id)
        {
            var result = await _usecase.GetInstructorCarList(id);
            return result.ToActionResult();
        }

        [HttpGet("instructor/{id}/cars")]
        public async Task<IActionResult> GetInstructorCars([FromRoute] Guid id)
        {
            var result = await _usecase.GetInstructorCarsList(id);
            return result.ToActionResult();
        }

        [HttpPost("{id}/moderate")]
        public async Task<IActionResult> ModerateInstructorCar([FromRoute] Guid id, [FromQuery]string action)
        {
            var result = await _usecase.ModerateInstructorCar(id, action);
            return result.ToActionResult();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCar([FromForm] CarCreationDTO car)
        {
            var result = await _usecase.CreateNewCar(car);
            return result.ToActionResult();
        }

        /// <summary>
        ///     This endpoint requires all information to be filled! Only use this if you got the original car information.
        ///     Do not fill the images if you don't want to add new image.
        /// </summary>
        /// <param name="id">The car id</param>
        /// <param name="car">The car information</param>
        /// <returns>the creation result</returns>
        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateCar([FromRoute] Guid id, [FromForm] CarUpdateDTO car)
        {
            var result = await _usecase.UpdateCarInformation(id, car);
            return result.ToActionResult();
        }

        // Support partial update of a car entity. (For example: only update the car name or only update car brand)
        [HttpPatch("{id}")] 
        public async Task<IActionResult> PartialUpdateCar([FromRoute] Guid id, [FromForm] CarUpdateDTO car)
        {
            var result = await _usecase.UpdateCarInformation(id, car);
            return result.ToActionResult();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteACar([FromRoute] Guid id)
        {
            var result = await _usecase.DeleteCar(id);
            return result.ToActionResult();
        }

        [HttpGet("{id}/document")]
        public async Task<IActionResult> GetCarDocuments(Guid id)
        {
            var result = await _usecase.GetCarDocuments(id);
            return result.ToActionResult(_logger);
        }

        /// Testing
        [HttpPost("{id}/documents")]
        public async Task<IActionResult> UploadCarDocumentBatch(Guid id, [FromForm] CarDocumentBatchDTO documents)
        {
            return Ok();
        }

        [HttpPost("{id}/document")]
        public async Task<IActionResult> UpdateCarDocument(Guid id, [FromForm] CarDocumenDTO document)
        {
            var result = await _usecase.UploadCarDocument(id, document);
            return result.ToActionResult(_logger);
        }

        [HttpDelete("{id}/document")]
        public async Task<IActionResult> DeleteCarDocumentOfType(Guid id, [FromQuery] string type)
        {
            var result = await _usecase.DeleteCarDocument(id, type);
            return result.ToActionResult(_logger);
        }
    }
}
