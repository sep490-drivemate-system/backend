namespace SharedLibrary.SharedKernel.Http.DTOs.Payment
{
    public class PaymentRequest
    {
        public Guid UserId { get; set; }
        public decimal Amount { get; set; }
        public Guid BookingId { get; set; }
        public Guid? DrivingSessionId { get; set; }
        public Guid InstructorId { get; set; }
}
}
