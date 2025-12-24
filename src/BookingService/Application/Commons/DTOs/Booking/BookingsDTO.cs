using BookingService.Domain.Enum;

namespace BookingService.Application.Commons.DTOs.Booking
{
    public class BookingsDTO
    {
        public Guid Id {get; set;}
        public string NamePackage {get; set;}
        public BookingStatus BookingStatus {get; set;}
        public decimal Price { get; set; }
        public Guid CarId { get; set; }
        public string CarName { get; set; }
        public DateTime BuyDate { get; set;}
        public int Duration { get; set;}
        public int DurationInUse { get; set;}
        public int PrecentInUse { get; set;}
        public int RemainingTime { get; set;}

        public Guid InstructorId { get; set; }

        public List<string> RoadTypes { get; set; }
        public List<string> DrivingSkills { get; set; }


    }



}
