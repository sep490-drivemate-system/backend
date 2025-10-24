using System.Text.Json.Serialization;

namespace BookingService.Application.Commons.DTOs.Payment
{
    public class WalletBalanceCheckRequest
    {
        [JsonPropertyName("userId")]
        public Guid UserId { get; set; }

        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }

        [JsonPropertyName("bookingId")]
        public Guid BookingId { get; set; }
    }
}
