using System.Text.Json.Serialization;

namespace BookingService.Infrastructure.Messaging.Models
{
    public class WalletBalanceCheckRequest
    {
        [JsonPropertyName("messageId")]
        public string MessageId { get; set; } = Guid.NewGuid().ToString();

        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        [JsonPropertyName("correlationId")]
        public string? CorrelationId { get; set; }

        [JsonPropertyName("userId")]
        public Guid UserId { get; set; }

        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }

        [JsonPropertyName("bookingId")]
        public Guid BookingId { get; set; }

        [JsonPropertyName("source")]
        public string Source { get; set; } = "BookingService";
    }
}
