using System.Text.Json.Serialization;

namespace BookingService.Application.Commons.DTOs.Feedbacks
{
    public class CarFeedbackDTO
    {
        [JsonPropertyName("user_name")]
        public string Username { get; set; }

        [JsonPropertyName("avatar_url")]
        public string AvatarUrl { get; set; }

        [JsonPropertyName("rating_score")]
        public int Rating { get; set; }

        [JsonPropertyName("comment")]
        public string Comment { get; set; }

        [JsonPropertyName("feedback_date")]
        public DateTime FeedbackDate { get; set; }
    }
}
