using Microsoft.AspNetCore.Mvc;
using BookingService.Domain.Entities;
using BookingService.Domain.Interfaces;

namespace BookingService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly IBookingRepository _bookingRepository;

        public BookingController(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Booking>>> GetAllBookings()
        {
            var bookings = await _bookingRepository.GetAllAsync();
            return Ok(bookings);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Booking>> GetBooking(Guid id)
        {
            var booking = await _bookingRepository.GetByIdAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            return Ok(booking);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<Booking>>> GetBookingsByUser(Guid userId)
        {
            var bookings = await _bookingRepository.GetByUserIdAsync(userId);
            return Ok(bookings);
        }

        [HttpGet("instructor/{instructorId}")]
        public async Task<ActionResult<IEnumerable<Booking>>> GetBookingsByInstructor(Guid instructorId)
        {
            var bookings = await _bookingRepository.GetByInstructorIdAsync(instructorId);
            return Ok(bookings);
        }

        [HttpPost]
        public async Task<ActionResult<Booking>> CreateBooking(Booking booking)
        {
            var createdBooking = await _bookingRepository.CreateAsync(booking);
            return CreatedAtAction(nameof(GetBooking), new { id = createdBooking.Id }, createdBooking);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBooking(Guid id, Booking booking)
        {
            if (id != booking.Id)
            {
                return BadRequest();
            }

            var updatedBooking = await _bookingRepository.UpdateAsync(booking);
            return Ok(updatedBooking);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(Guid id)
        {
            await _bookingRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
