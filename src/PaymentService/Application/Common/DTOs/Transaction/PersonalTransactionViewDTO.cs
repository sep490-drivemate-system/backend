using PaymentService.Domain.Enum;

namespace PaymentService.Application.Common.DTOs.Transaction
{
    public class PersonalTransactionViewDTO
    {
        public string Title { get; set; }
        public decimal Value { get; set; }
        public DateTime Date { get; set; }
        public PaymentStatus Status { get; set; }
        public string StatusText { get; set; }
    }
}
