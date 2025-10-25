using System.Text.Json.Serialization;

namespace BookingService.Infrastructure.Messaging.Models
{
    public class WalletBalanceCheckRequest
    {
        public Guid UserId { get; set; }
        public decimal Amount { get; set; }


    }
}
