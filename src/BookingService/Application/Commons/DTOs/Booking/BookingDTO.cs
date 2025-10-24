namespace BookingService.Application.Commons.DTOs.Booking
{
    public class BookingDTO
    {
        public Guid PackageId { get; set; }
        public DateOnly StartDate {  get; set; }
        public DateOnly EndDate {  get; set; }
        public string Note { get; set; } = string.Empty;
        public string PickUpPoint { get; set; } = string.Empty;
        public decimal Price { get; set; }

        public ICollection<TimeRangeRequest> TimeRanges{ get; set; } = new List<TimeRangeRequest>();
        public ICollection<DrivingSkillRequest> DrivingSkills{ get; set; } = new List<DrivingSkillRequest>();
        public ICollection<RoadTypeRequest> RoadTypes { get; set; } = new List<RoadTypeRequest>();
    }
    public class TimeRangeRequest
    {
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime
        {
            get; set;
        }
    }
    public class DrivingSkillRequest
    {
        public Guid DrivingSkillId { get; set; }
    }
    public class RoadTypeRequest
    {
        public Guid RoadTypeId {  get; set; }
    }

}
