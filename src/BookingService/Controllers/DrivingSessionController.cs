using BookingService.Application.Commons.DTOs.DrivingSessions;
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
        public async Task<IActionResult> GetAllDrivingSession(SessionStatus sessionStatus) 
        {
            var result = await _drivingSessionUseCase.GetAllDrivingSession(sessionStatus);
            return result.ToActionResult();
        }


        [HttpPost]
        public async Task<IActionResult> CreateDrivingSession(DrivingSessionCreationDTO sessionStatus)
        {
            var result = await _drivingSessionUseCase.CreateDrivingSession(sessionStatus);
            return result.ToActionResult();
        }
        [HttpGet("/users/{id}/sessions")]
        public async Task<IActionResult> GetAllUserSessions([FromRoute] Guid id, [FromQuery] SessionFilterDTO filter)
        {
            var result = await _drivingSessionUseCase.GetUserSessions(id, filter);
            return result.ToActionResult();
        }

        [HttpGet("/users/{id}/requests")]
        public async Task<IActionResult> GetAllSessionsWithChangeRequest([FromRoute] Guid id)
        {
            var result = await _drivingSessionUseCase.GetUserSessionsWithChangeRequest(id);
            return result.ToActionResult();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSessionDetail([FromRoute] Guid id)
        {
            var result = await _drivingSessionUseCase.GetSessionDetail(id);
            return result.ToActionResult();
        }

        [HttpPost("{id}/cancel")]
        public async Task<IActionResult> CancelSession([FromRoute] Guid id, [FromBody] SessionCancelRequestDTO request)
        {
            request.JwtToken = Request.Headers.Authorization.FirstOrDefault();
            var result = await _drivingSessionUseCase.CancelSession(id, request);
            return result.ToActionResult();
        }

        [HttpPost("{id}/reschedule")]
        public async Task<IActionResult> RescheduleSession([FromRoute] Guid id, [FromBody] SessionRescheduleRequestDTO request)
        {
            request.JwtToken = Request.Headers.Authorization.FirstOrDefault();
            var result = await _drivingSessionUseCase.RescheduleSession(id, request);
            return result.ToActionResult();
        }
        //[HttpGet("{id}/schedule/instructor")]
        //public async Task<IActionResult> ScheduleInstructor([FromRoute] Guid id )
        //{           
        //    var result = await _drivingSessionUseCase.ScheduleInstructor(id);
        //    return result.ToActionResult();
        //}
    }
}
