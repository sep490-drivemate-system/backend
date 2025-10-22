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

        public ICollection<TimeRanges> TimeRanges{ get; set; } = new List<TimeRanges>();
        public ICollection<DrivingSkill> DrivingSkills{ get; set; } = new List<DrivingSkill>();
        public ICollection<RoadType> RoadTypes { get; set; } = new List<RoadType>();
    }
    public class TimeRanges
    {
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime
        {
            get; set;
        }
    }
    public class DrivingSkill
    {
        public Guid DrivingSkillId { get; set; }
    }
    public class RoadType
    {
        public Guid RoadTypeId {  get; set; }
    }

}
