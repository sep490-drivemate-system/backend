using System.Text.Json.Serialization;

namespace PaymentService.Infrastructure.Messaging.Models
{
    public class WalletBalanceCheckRequest
    {
        [JsonPropertyName("messageId")]
        public string MessageId { get; set; } = string.Empty;

        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }

        [JsonPropertyName("correlationId")]
        public string? CorrelationId { get; set; }

        [JsonPropertyName("userId")]
        public Guid UserId { get; set; }

        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }

        [JsonPropertyName("bookingId")]
        public Guid BookingId { get; set; }

        [JsonPropertyName("source")]
        public string Source { get; set; } = string.Empty;
    }
}
