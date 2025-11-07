using Resend;
using SharedLibrary.Email;
using SharedLibrary.Jwt;
using SharedLibrary.SharedKernel.Http;
using SharedLibrary.SharedKernel.Http.Implementation;
using SharedLibrary.SharedKernel.Http.Interfaces;
using SharedLibrary.SharedKernel.Password;
using SharedLibrary.Sms;
using UserService.Application.Interfaces;
using UserService.Application.UseCases;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace UserService.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(
           this IServiceCollection services, IConfiguration configuration)
        {

            // Register Usecase
            services.AddScoped<IUserUseCase, UserUseCase>();
            services.AddScoped<IAuthUseCase, AuthUseCase>();
            services.AddScoped<INoviceDriverUseCase, NoviceDriverUseCase>();
            services.AddScoped<IInstructorUseCase, InstructorUseCase>();
            services.AddScoped<IPolicyUseCase, PolicyUseCase>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // Register library external
            services.Configure<SpeedSmsSettings>(configuration.GetSection("SpeedSMS"));
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IPasswordHasherService, PasswordHasherService>();
            services.AddHttpClient<SpeedSmsService>();
            services.AddScoped<ISmsService, SpeedSmsService>();
            services.AddScoped<PasswordHasherService>();
            services.AddScoped<HttpService>();

            services.AddScoped<IFeedback,Feedback>();
            services.AddScoped<IPackage, SharedLibrary.SharedKernel.Http.Implementation.Package>();

            services.AddHttpClient<ResendClient>();
            services.AddTransient<IResend, ResendClient>();
            services.Configure<ResendClientOptions>(o =>
            {
                o.ApiToken = Environment.GetEnvironmentVariable("RESEND_APITOKEN")!;
            });

            // Đăng ký service khác (cache, email, storage…)
            // services.AddScoped<IEmailService, EmailService>();

            return services;
        }
    }
}
