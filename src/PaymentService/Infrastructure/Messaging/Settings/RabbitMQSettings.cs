namespace PaymentService.Infrastructure.Messaging.Settings
{
    public class RabbitMQSettings
    {
        public string HostName { get; set; } = "collie.lmq.cloudamqp.com";
        public int Port { get; set; } = 5672;
        public string UserName { get; set; } = "rmktcjwj";
        public string Password { get; set; } = "m1-L9KSptCjuFUuFFM6FAG9z0XPHUPJV";
        public string VirtualHost { get; set; } = "rmktcjwj";
        
        // Exchange for PaymentService
        public string PaymentExchange { get; set; } = "payment.exchange";
        
        // Queues that PaymentService consumes from
        public string WalletBalanceCheckQueue { get; set; } = "wallet.balance.check";
        
        // Queues that PaymentService publishes to
        public string WalletBalanceResponseQueue { get; set; } = "wallet.balance.response";
        public string PaymentProcessedQueue { get; set; } = "payment.processed";
        public string PaymentFailedQueue { get; set; } = "payment.failed";
        
        // Routing keys
        public string WalletBalanceCheckRoutingKey { get; set; } = "wallet.balance.check";
        public string WalletBalanceResponseRoutingKey { get; set; } = "wallet.balance.response";
        public string PaymentProcessedRoutingKey { get; set; } = "payment.processed";
        public string PaymentFailedRoutingKey { get; set; } = "payment.failed";
    }
}
