using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace BookingService.Application.DTOs.DrivingSkills
{
    public class DrivingSkillDTO
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("display_name")]
        public string Name { get; set; }

        /*[JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("difficulty_level")]
        public int DifficultyLevel { get; set; }

        [JsonPropertyName("difficulty_text")]
        public int DifficultyText { get; set; }

        [JsonPropertyName("image_url")]
        public string ImageUrl { get; set; }*/
    }
}
