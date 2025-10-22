using BookingService.Application.Interfaces;
using BookingService.Domain.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.SharedKernel.ServiceResult;

namespace BookingService.Controllers
{
    [Route("api/session")]
    [ApiController]
    public class DrivingSessionController : ControllerBase
    {
        private readonly IDrivingSessionUseCase _drivingSessionUseCase;
        public DrivingSessionController(IDrivingSessionUseCase drivingSessionUseCase)
        {
            _drivingSessionUseCase = drivingSessionUseCase;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllDrrivingSession(SessionStatus sessionStatus) 
        {
            var result = await _drivingSessionUseCase.GetAllDrrivingSession(sessionStatus);
            return result.ToActionResult();
        }
    }
}
