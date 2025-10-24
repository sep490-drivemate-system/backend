using System.Text.Json.Serialization;

namespace PaymentService.Infrastructure.Messaging.Models
{
    public class WalletBalanceCheckResponse
    {
        [JsonPropertyName("messageId")]
        public string MessageId { get; set; } = Guid.NewGuid().ToString();

        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        [JsonPropertyName("correlationId")]
        public string? CorrelationId { get; set; }

        [JsonPropertyName("isSuccess")]
        public bool IsSuccess { get; set; }

        [JsonPropertyName("hasSufficientBalance")]
        public bool HasSufficientBalance { get; set; }

        [JsonPropertyName("currentBalance")]
        public decimal CurrentBalance { get; set; }

        [JsonPropertyName("requestedAmount")]
        public decimal RequestedAmount { get; set; }

        [JsonPropertyName("userId")]
        public Guid UserId { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;

        [JsonPropertyName("source")]
        public string Source { get; set; } = "PaymentService";
    }
}
