using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PaymentService.Infrastructure.Messaging.Interfaces;

namespace PaymentService.Infrastructure.Messaging.Config
{
    public class RabbitMQHostedService : BackgroundService
    {
        private readonly IRabbitMQService _rabbitMQService;

        public RabbitMQHostedService(IRabbitMQService rabbitMQService)
        {
            _rabbitMQService = rabbitMQService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Start the wallet balance consumer
            _rabbitMQService.StartWalletBalanceConsumer();

            // Keep the service running
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }

        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await base.StopAsync(cancellationToken);
        }
    }
}
