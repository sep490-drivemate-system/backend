namespace SharedLibrary.SharedKernel.Http.DTOs.Payment
{
    public class PaymentResponse
    {
        public bool IsPayment { get; set; }
        public string? Message { get; set; }
        public decimal? CurrentBalance { get; set; }
    }
}
