namespace PaymentService.Application.Common.DTOs
{
    public class WalletDto
    {
        public Guid Id { get; set; }
        public decimal Balance { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class CreateWalletDto
    {
        public decimal InitialBalance { get; set; } = 0;
    }

    public class UpdateWalletBalanceDto
    {
        public Guid WalletId { get; set; }
        public decimal Balance { get; set; }
    }
}
