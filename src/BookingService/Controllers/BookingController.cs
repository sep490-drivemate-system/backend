using BookingService.Application.Commons.DTOs.Booking;
using BookingService.Application.Interfaces;
using BookingService.Domain.Entities;
using BookingService.Domain.Enum;
using BookingService.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Jwt;
using SharedLibrary.SharedKernel.Enum;
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

        public BookingController(IBookingUseCase bookingUseCase, IJwtService jwtService)
        {
            _bookingUseCase = bookingUseCase;
            _jwtService = jwtService;
        }


        [HttpPost]
        //   [Authorize(Roles = nameof(UserRole.NoviceDriver))]
        public async Task<IActionResult> CreateBooking([FromBody] BookingDTO bookingDTO)
        {
            var driverId = await _jwtService.ExtractUserIdFromToken(Request.Headers["Authorization"].ToString());
            var result = await _bookingUseCase.CreateBooking(bookingDTO, driverId);
            return result.ToActionResult();
        }

        [HttpGet]
        public async Task<IActionResult> GetBooking(BookingStatus bookingStatus)
        {
            var driverId = await _jwtService.ExtractUserIdFromToken(Request.Headers["Authorization"].ToString());
            var result = await _bookingUseCase.GetBookings(bookingStatus, driverId);
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
    }
    
}
