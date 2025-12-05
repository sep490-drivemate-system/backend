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
using SharedLibrary.Payment.PayOs;
using SharedLibrary.Payment.VnPay;
using SharedLibrary.Payment.ZaloPay;

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

            // Add UserService httpClient
            services.AddHttpClient("UserServiceClient", cfg =>
            {
                var base_address = configuration.GetConnectionString("Userservice_connection");

                if (string.IsNullOrEmpty(base_address))
                {
                    throw new ApplicationException("Can not find base address for user service");
                }

                cfg.BaseAddress = new Uri(base_address);
                cfg.DefaultRequestHeaders.Add("User-Agent", "DriveMate_PaymentService");
            });

            // Add Wallet service
            services.AddScoped<IWalletService, WalletService>();

            // Add payment gateway services (PayOS, VNPay, ZaloPay)
            services.AddScoped<IPayOSService, PayOSService>();
            services.AddScoped<IVNPayService, VNPayService>();
            services.AddScoped<IZaloPayService, ZaloPayService>();
            
            // Add RabbitMQ hosted service
            services.AddHostedService<RabbitMQHostedService>();

            return services;
        }
    }
}
