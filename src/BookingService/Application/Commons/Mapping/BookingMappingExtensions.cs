using BookingService.Application.Commons.DTOs.Booking;
using BookingService.Domain.Entities;
using SharedLibrary.SharedKernel.Http.DTOs.User;

namespace BookingService.Application.Commons.Mapping
{
    public static class BookingMappingExtensions
    {
        /// <summary>
        /// Maps bookings to BookingsDTO with instructor info and calculated duration statistics
        /// </summary>
        public static List<BookingsDTO> MapToBookingsDTOWithStats(
            this IEnumerable<Booking> bookings,
            Dictionary<Guid, InstructorBasicInfoDTO> instructorInfos)
        {
            return bookings.Select(booking =>
            {
                var durationInUse = booking.DrivingSessions?
                    .Where(ds => !ds.IsDeleted && ds.ActualStart != default && ds.ActualEnd != default)
                    .Sum(ds => (ds.ActualEnd - ds.ActualStart).TotalHours) ?? 0;

                var duration = (int)booking.DurationWhenBought;
                var durationUsed = (int)Math.Round(durationInUse);
                var remainingTime = Math.Max(0, duration - durationUsed);
                var percentInUse = duration > 0 ? (int)Math.Round((double)durationUsed / duration * 100) : 0;

                var instructorInfo = instructorInfos.TryGetValue(booking.InstructorId, out var info)
                    ? info
                    : null;

                return new BookingsDTO
                {
                    Id = booking.Id,
                    InstructorId = booking.InstructorId,
                    CarId = booking.CarId,
                    CarPrice = booking.Car?.Price,
                    NameInstructor = instructorInfo?.Fullname ?? "Unknown",
                    AvatarInstructor = instructorInfo?.AvatarUrl ?? "",
                    NamePackake = booking.Package?.Name ?? "",
                    BookingStatus = booking.Status,
                    BuyDate = booking.CreatedAt,
                    Duration = duration,
                    DurationInUse = durationUsed,
                    RemainingTime = remainingTime,
                    PrecentInUse = percentInUse
                };
            }).ToList();
        }
    }
}
