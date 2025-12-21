using PaymentService.Domain.Enum;

namespace PaymentService.Application.Common.DTOs
{
    public class WithdrawRequestDTO
    {
        // Properties
        public decimal TransactionValue { get; set; }
        public string? TransactionNote { get; set; }
        public bool IsDelete { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }


        // System specific entities
        public DateTime RequestTime { get; set; }


    }
}
