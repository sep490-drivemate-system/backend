namespace PaymentService.Domain.Enum
{
    public enum PaymentStatus
    {
        Pending = 1,
        Processing = 2, 
        Completed = 3, 
        Failed = 4,
        Cancelled = 5, 
        Refunded = 6,
        Deposit=7 
    }
}
