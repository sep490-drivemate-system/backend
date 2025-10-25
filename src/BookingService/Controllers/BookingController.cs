using BookingService.Application.Commons.DTOs.Booking;
using BookingService.Application.Interfaces;
using BookingService.Domain.Entities;
using BookingService.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Jwt;
using SharedLibrary.SharedKernel.Enum;
using SharedLibrary.SharedKernel.ServiceResult;
using Twilio.Jwt.AccessToken;

namespace BookingService.Controllers
{
    [ApiController]
    [Route("api/booking")]
    public class BookingController : ControllerBase
    {
        private readonly IBookingUseCase _bookingUseCase;
        private readonly IJwtService _jwtService;

        public BookingController(IBookingUseCase bookingUseCase,IJwtService jwtService)
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

      
    }
}
