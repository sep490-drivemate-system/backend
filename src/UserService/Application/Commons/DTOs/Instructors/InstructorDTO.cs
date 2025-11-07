using System.Text.Json.Serialization;
using UserService.Domain.Enum;

namespace UserService.Application.Commons.DTOs.Instructors
{
    public class InstructorDTO
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Avatar { get; set; }
        public int ExperienceYear { get; set; }
        public GenderType Gender { get; set; }
        public string Bio {  get; set; }
        public int BookingCount { get; set; }
        public int PackageCount { get; set; }
        public decimal AverageRating { get; set; }
    }
    public class InstructorDetailDTO : InstructorDTO
    {
    }

}
