using BookingService.Domain.Enum;

namespace BookingService.Application.Commons.DTOs.Booking
{
    public class BookingsDTO
    {
        public Guid Id {get; set;}
        public string NamePackake {get; set;}
        public BookingStatus BookingStatus {get; set;}
        public DateTime BuyDate { get; set;}
        public int Duration { get; set;}
        public int DurationInUse { get; set;}
        public int PrecentInUse { get; set;}
        public int RemainingTime { get; set;}


    }
}
