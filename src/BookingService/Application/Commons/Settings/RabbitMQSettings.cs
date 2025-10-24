namespace BookingService.Application.Commons.Settings
{
    public class RabbitMQSettings
    {
        public string HostName { get; set; } = "localhost";
        public int Port { get; set; } = 5672;
        public string UserName { get; set; } = "guest";
        public string Password { get; set; } = "guest";
        public string VirtualHost { get; set; } = "/";
        
        // Queue names
        public string WalletBalanceCheckQueue { get; set; } = "wallet.balance.check";
        public string WalletBalanceResponseQueue { get; set; } = "wallet.balance.response";
        
        // Exchange names
        public string PaymentExchange { get; set; } = "payment.exchange";
        
        // Routing keys
        public string WalletBalanceCheckRoutingKey { get; set; } = "wallet.balance.check";
        public string WalletBalanceResponseRoutingKey { get; set; } = "wallet.balance.response";
    }
}
