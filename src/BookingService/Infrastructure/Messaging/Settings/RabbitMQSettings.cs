namespace BookingService.Infrastructure.Messaging.Settings
{
    public class RabbitMQSettings
    {
        public string HostName { get; set; } = "collie.lmq.cloudamqp.com";
        public int Port { get; set; } = 5671;
        public string UserName { get; set; } = "rmktcjwj";
        public string Password { get; set; } = "m1-L9KSptCjuFUuFFM6FAG9z0XPHUPJV";
        public string VirtualHost { get; set; } = "rmktcjwj";
        
        // Exchange for BookingService
        public string BookingExchange { get; set; } = "booking.exchange";
        
        // Queues that BookingService publishes to
        public string WalletBalanceCheckQueue { get; set; } = "wallet.balance.check";
        public string BookingCreatedQueue { get; set; } = "booking.created";
        public string BookingUpdatedQueue { get; set; } = "booking.updated";
        public string BookingCancelledQueue { get; set; } = "booking.cancelled";
        
        // Queue that BookingService consumes from (responses)
        public string WalletBalanceResponseQueue { get; set; } = "wallet.balance.response";
        
        // Routing keys
        public string WalletBalanceCheckRoutingKey { get; set; } = "wallet.balance.check";
        public string WalletBalanceResponseRoutingKey { get; set; } = "wallet.balance.response";
        public string BookingCreatedRoutingKey { get; set; } = "booking.created";
        public string BookingUpdatedRoutingKey { get; set; } = "booking.updated";
        public string BookingCancelledRoutingKey { get; set; } = "booking.cancelled";
    }
}
