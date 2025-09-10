using SharedLibrary.Email;
using SharedLibrary.Jwt;
using SharedLibrary.SharedKernel.Password;
using SharedLibrary.Sms;
using System;
using UserService.Application.Interfaces;
using UserService.Application.UseCases;
using UserService.Domain.Interfaces;
using UserService.Infrastructure.Repositories;
using UserService.Infrastructure.UnitOfWork;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace UserService.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(
           this IServiceCollection services, IConfiguration configuration)
        {

            // Register Usecase
            services.AddScoped<IAuthUseCase, AuthUseCase>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IJwtService, JwtService>();
            


            //  services.AddScoped<IOrderRepository, OrderRepository>();
            // Register library external
            services.Configure<EmailSettings>(configuration.GetSection("Email"));
            services.Configure<SpeedSmsSettings>(configuration.GetSection("SpeedSMS"));
            services.AddScoped<IEmailService, EmailService>();
            services.AddHttpClient<SpeedSmsService>();
            services.AddScoped<ISmsService, SpeedSmsService>();

            // Đăng ký service khác (cache, email, storage…)
            // services.AddScoped<IEmailService, EmailService>();

            return services;
        }
    }
}
