namespace BookingService.Domain.Enum
{
    public enum BookingStatus
    {
        Purchased = 1,
        InUse = 2,
        Used = 3,
        CancellationWithRefund = 4,
        CancellationWithoutRefund = 5,
    }
}
