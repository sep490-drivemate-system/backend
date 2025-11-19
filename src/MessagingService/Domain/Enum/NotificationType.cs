namespace MessagingService.Domain.Enum
{
    public enum NotificationType
    {
        BookingCreated = 0,
        BookingCancelled = 1,
        BookingConfirmed = 2,
        SessionScheduled = 3,
        SessionReminder = 4,
        PaymentReceived = 5,
        PaymentFailed = 6,
        FeedbackReceived = 7,
        SystemAnnouncement = 8,
        MessageReceived = 9
    }
}

