using BookingService.Domain.Enum;

namespace BookingService.Application.Commons.DTOs.Booking
{
    public class BookingsDTO
    {
        public Guid Id {get; set;}
        public string NameInstructor {get; set;}
        public Guid  InstructorId {get; set;}
        public Guid? CarId {get; set;}
        public decimal? CarPrice {get; set;}
        public string NamePackake {get; set;}
        public BookingStatus BookingStatus {get; set;}
        public string AvatarInstructor {get; set;}

        public DateTime BuyDate { get; set;}
        public int Duration { get; set;}
        public int DurationInUse { get; set;}
        public int PrecentInUse { get; set;}
        public int RemainingTime { get; set;}


    }
}
