using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PaymentService.Application.Interfaces;
using PaymentService.Application.Services;
using PaymentService.Domain.Interfaces;
using PaymentService.Infrastructure.Data;
using PaymentService.Infrastructure.Repositories;
using PaymentService.Infrastructure.UoW;
using PaymentService.Infrastructure.Messaging.Config;
using PaymentService.Infrastructure.Messaging.Interfaces;
using PaymentService.Infrastructure.Messaging.Implementation;

namespace PaymentService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Add DbContext
            services.AddDbContext<PaymentDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("PAYMENTSERVICECONNECTION")));

            // Add Repositories
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<IWalletRepository, WalletRepository>();

            // Add Unit of Work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Register RabbitMQ service (with fallback to mock if connection fails)
   
           services.AddSingleton<IRabbitMQService, RabbitMQService>();
           
            
            // Add Wallet service
            services.AddScoped<IWalletService, WalletService>();
            
            // Add RabbitMQ hosted service
            services.AddHostedService<RabbitMQHostedService>();

            return services;
        }
    }
}
