using Microsoft.AspNetCore.Mvc;
using BookingService.Domain.Entities;
using BookingService.Domain.Interfaces;
using BookingService.Application.Interfaces;
using BookingService.Application.Commons.DTOs.Booking;
using SharedLibrary.SharedKernel.ServiceResult;

namespace BookingService.Controllers
{
    [ApiController]
    [Route("api/booking")]
    public class BookingController : ControllerBase
    {
        private readonly IBookingUseCase _bookingUseCase;

        public BookingController(IBookingUseCase bookingUseCase)
        {
            _bookingUseCase = bookingUseCase;
        }

        [HttpPost]
        public async Task<IActionResult> CreateBooking([FromBody] BookingDTO bookingDTO)
        {
            var result = await _bookingUseCase.CreateBooking(bookingDTO);
            return result.ToActionResult();
        }

      
    }
}
