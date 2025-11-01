using System.Text.Json.Serialization;

namespace BookingService.Application.Commons.DTOs.DrivingSkills
{
    public class DrivingSkillCreationDTO
    {
        [JsonPropertyName("name")]
        public string SkillName { get; set; }
    }
}
