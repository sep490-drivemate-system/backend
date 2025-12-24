using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PaymentService.Application.Interfaces;
using PaymentService.Application.Services;
using PaymentService.Domain.Interfaces;
using PaymentService.Infrastructure.Data;
using PaymentService.Infrastructure.Repositories;
using PaymentService.Infrastructure.UoW;
using SharedLibrary.Email;
using SharedLibrary.Payment.PayOs;
using SharedLibrary.Payment.VnPay;
using SharedLibrary.Payment.ZaloPay;
using Resend;

namespace PaymentService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Add DbContext
            services.AddDbContext<PaymentDbContext>(options =>
            {
                var connectionString = configuration.GetConnectionString("PAYMENTSERVICECONNECTION");
                // Add timezone to connection string if not already present
                if (!connectionString.Contains("TimeZone", StringComparison.OrdinalIgnoreCase))
                {
                    connectionString += ";TimeZone=Asia/Ho_Chi_Minh";
                }
                options.UseNpgsql(connectionString);
            });

            // Add Repositories
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<IWalletRepository, WalletRepository>();

            // Add Unit of Work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

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
            
            // Register Resend for email service
            services.AddHttpClient<ResendClient>();
            services.AddTransient<IResend, ResendClient>();
            services.Configure<ResendClientOptions>(o =>
            {
                o.ApiToken = Environment.GetEnvironmentVariable("RESEND_APITOKEN") ?? Environment.GetEnvironmentVariable("RESEND_API_KEY") ?? string.Empty;
            });
            
            // Add Email Service
            services.AddScoped<IEmailService, EmailService>();

            return services;
        }
    }
}
