using BookingService.Application.Commons.DTOs.Booking;
using BookingService.Domain.Entities;
using BookingService.Domain.Enum;
using SharedLibrary.SharedKernel.Http.DTOs.User;

namespace BookingService.Application.Commons.Mapping
{
    public static class BookingMappingExtensions
    {
        public static int CalculateDurationUsed(this Booking booking)
        {
            var durationInUse = booking.DrivingSessions?
                .Where(ds => !ds.IsDeleted
                    && ds.Status == SessionStatus.Completed)
                .Sum(ds => (ds.ActualEnd - ds.ActualStart).TotalHours) ?? 0;

            return (int)Math.Round(durationInUse);
        }
        public static int CalculateRemainingTime(this Booking booking)
        {
            var duration = (int)booking.DurationWhenBought;

            var completedDuration = booking.DrivingSessions?
                .Where(ds => !ds.IsDeleted
                    && ds.Status == SessionStatus.Completed
                    && ds.ActualStart != default
                    && ds.ActualEnd != default)
                .Sum(ds => (ds.ActualEnd - ds.ActualStart).TotalHours) ?? 0;

            var upcomingDuration = booking.DrivingSessions?
                .Where(ds => !ds.IsDeleted
                    && ds.Status == SessionStatus.Upcoming
                    && ds.StartTime != default
                    && ds.EndTime != default)
                .Sum(ds => (ds.EndTime - ds.StartTime).TotalHours) ?? 0;

            var totalUsedAndPlanned = (int)Math.Round(completedDuration + upcomingDuration);

            return Math.Max(0, duration - totalUsedAndPlanned);
        }
        public static int CalculatePercentInUse(this Booking booking)
        {
            var duration = (int)booking.DurationWhenBought;
            if (duration <= 0) return 0;

            var durationUsed = booking.CalculateDurationUsed();
            return (int)Math.Round((double)durationUsed / duration * 100);
        }
    }
}
