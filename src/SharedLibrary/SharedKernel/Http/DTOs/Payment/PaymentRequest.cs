namespace SharedLibrary.SharedKernel.Http.DTOs.Payment
{
    public class PaymentRequest
    { 
       public Guid UserId { get; set; }
       public decimal Amount { get; set; }
             
    }
}
