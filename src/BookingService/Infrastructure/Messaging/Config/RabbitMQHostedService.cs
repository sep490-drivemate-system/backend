using BookingService.Infrastructure.Messaging.Interface;

namespace BookingService.Infrastructure.Messaging.Config
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
