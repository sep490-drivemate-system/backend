using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PaymentService.Infrastructure.Messaging.Interfaces;

namespace PaymentService.Infrastructure.Messaging.Services
{
    public class RabbitMQHostedService : BackgroundService
    {
        private readonly IRabbitMQService _rabbitMQService;
        private readonly ILogger<RabbitMQHostedService> _logger;

        public RabbitMQHostedService(IRabbitMQService rabbitMQService, ILogger<RabbitMQHostedService> logger)
        {
            _rabbitMQService = rabbitMQService;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                _logger.LogInformation("Starting RabbitMQ wallet balance consumer...");
                
                // Start the wallet balance consumer
                _rabbitMQService.StartWalletBalanceConsumer();
                
                _logger.LogInformation("RabbitMQ wallet balance consumer started successfully");

                // Keep the service running
                while (!stoppingToken.IsCancellationRequested)
                {
                    await Task.Delay(1000, stoppingToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in RabbitMQ hosted service");
                throw;
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping RabbitMQ hosted service...");
            await base.StopAsync(cancellationToken);
        }
    }
}
