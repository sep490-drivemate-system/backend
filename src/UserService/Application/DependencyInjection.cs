using Microsoft.AspNetCore.Identity;
using SharedLibrary.Email;
using SharedLibrary.Jwt;
using SharedLibrary.SharedKernel.Http;
using SharedLibrary.SharedKernel.Http.Implementation;
using SharedLibrary.SharedKernel.Http.Interfaces;
using SharedLibrary.SharedKernel.Password;
using SharedLibrary.Sms;
using System;
using UserService.Application.Interfaces;
using UserService.Application.UseCases;
using UserService.Domain.Interfaces;
using UserService.Infrastructure.Repositories;
using UserService.Infrastructure.UoW;
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
            services.AddScoped<INoviceDriverUseCase, NoviceDriverUseCase>();
            services.AddScoped<IInstructorUseCase, InstructorUseCase>();
            services.AddScoped<IPolicyUseCase, PolicyUseCase>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // Register library external
            services.Configure<EmailSettings>(configuration.GetSection("Email"));
            services.Configure<SpeedSmsSettings>(configuration.GetSection("SpeedSMS"));
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IPasswordHasherService, PasswordHasherService>();
            services.AddHttpClient<SpeedSmsService>();
            services.AddScoped<ISmsService, SpeedSmsService>();
            services.AddScoped<PasswordHasherService>();
            services.AddScoped<HttpService>();

            services.AddScoped<IFeedback,Feedback>();
            services.AddScoped<IPackage, SharedLibrary.SharedKernel.Http.Implementation.Package>();

            // Đăng ký service khác (cache, email, storage…)
            // services.AddScoped<IEmailService, EmailService>();

            return services;
        }
    }
}
