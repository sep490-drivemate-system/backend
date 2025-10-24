using System.Text.Json.Serialization;

namespace BookingService.Application.Commons.DTOs.Payment
{
    public class WalletBalanceCheckResponse
    {
        [JsonPropertyName("isSuccess")]
        public bool IsSuccess { get; set; }

        [JsonPropertyName("hasSufficientBalance")]
        public bool HasSufficientBalance { get; set; }

        [JsonPropertyName("currentBalance")]
        public decimal CurrentBalance { get; set; }

        [JsonPropertyName("requestedAmount")]
        public decimal RequestedAmount { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;

        [JsonPropertyName("userId")]
        public Guid UserId { get; set; }
    }
}
