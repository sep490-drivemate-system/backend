using BookingService.Application.Commons.DTOs.Booking;
using BookingService.Application.Commons.DTOs.Package;
using BookingService.Application.Interfaces;
using BookingService.Application.UseCase;
using BookingService.Domain.Entities;
using BookingService.Domain.Enum;
using BookingService.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Jwt;
using SharedLibrary.SharedKernel.Enum;
using SharedLibrary.SharedKernel.Http.Implementation;
using SharedLibrary.SharedKernel.Http.Interfaces;
using SharedLibrary.SharedKernel.ServiceResult;
using System.Threading.Tasks;
using Twilio.Jwt.AccessToken;

namespace BookingService.Controllers
{
    [ApiController]
    [Route("api/booking")]
    public class BookingController : ControllerBase
    {
        private readonly IBookingUseCase _bookingUseCase;
        private readonly IJwtService _jwtService;
        private readonly ISystemConfigurationHttpService testService;


        public BookingController(IBookingUseCase bookingUseCase, IJwtService jwtService, ISystemConfigurationHttpService service)
        {
            _bookingUseCase = bookingUseCase;
            _jwtService = jwtService;
            testService = service;
        }

        [HttpPost("buy-package")]
        [Authorize(Roles = nameof(UserRole.NoviceDriver))]
        public async Task<IActionResult> BuyPackage([FromBody] PackageBuyingDTO packageBuyingDTO)
        {
            var driverId = await _jwtService.ExtractUserIdFromToken(Request.Headers["Authorization"].ToString());
            var result = await _bookingUseCase.BuyPackage(packageBuyingDTO, driverId);
            return result.ToActionResult();
        }
        [HttpGet]
        [Authorize(Roles = nameof(UserRole.NoviceDriver))]
        public async Task<IActionResult> GetBookings([FromQuery] BookingFilterDTO bookingFilterDTO)
        {
            var driverId = await _jwtService.ExtractUserIdFromToken(Request.Headers["Authorization"].ToString());
            var result = await _bookingUseCase.GetBookings(bookingFilterDTO, 
            driverId);
            return result.ToActionResult();
        }

        [HttpPost("{id}/cancel")]
        [Authorize(Roles = nameof(UserRole.NoviceDriver))]
        public async Task<IActionResult> CancelBooking([FromRoute] Guid id)
        {
            var userId = await _jwtService.ExtractUserIdFromToken(Request.Headers["Authorization"].ToString());
            var result = await _bookingUseCase.CancelBooking(id, userId);
            return result.ToActionResult();
        }

        [HttpGet("instructor/{instructorId}/upcoming-sessions")]
        public async Task<IActionResult> GetUpcomingDrivingSessions([FromRoute] Guid instructorId)
        {
            var result = await _bookingUseCase.GetUpcomingDrivingSessions(instructorId);
            return result.ToActionResult();
        }

        [HttpGet("sessions")]
        public async Task<IActionResult> GetDrivingSessions([FromQuery] SessionStatus status = 0)
        {
            var instructorId = await _jwtService.ExtractUserIdFromToken(Request.Headers["Authorization"].ToString());
            var result = await _bookingUseCase.GetDrivingSessions(status, instructorId);
            return result.ToActionResult();
        }

        [HttpGet("statistic")]
        public async Task<IActionResult> GetBookingStatistic(BookingStatisticFilterDTO filter)
        {
            var result = await _bookingUseCase.GetBookingStatistic(filter);
            return result.ToActionResult();
        }

        [HttpGet("instructor-statistic")]
        public async Task<IActionResult> GetInstructorBookingStatistic([FromQuery] InstructorStatisticFilterDTO filter)
        {
            Guid user_id = await _jwtService.ExtractUserIdFromToken(Request.Headers["Authorization"].ToString());
            var result = await _bookingUseCase.GetInstructorStatistic(user_id, filter);
            return result.ToActionResult();
        }
    }
    
}
