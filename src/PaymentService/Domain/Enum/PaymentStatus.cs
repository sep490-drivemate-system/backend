namespace PaymentService.Domain.Enum
{
    public enum PaymentStatus
    {
        Pending = 1, // đợi sau khi yêu cầu rút tiền
        Processing = 2, // trong quá trình xử lí
        Completed = 3, // hoàn thành giao dịch
        Failed = 4,
        Cancelled = 5,// 
        Refunded = 6,
        Deposit=7 // nạp tiền xong
    }
}
