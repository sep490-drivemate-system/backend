



using BookingService.Application.Interfaces;
using BookingService.Application.UseCase;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedLibrary.Email;
using SharedLibrary.Jwt;
using SharedLibrary.SharedKernel.Http;
using SharedLibrary.SharedKernel.Http.Implementation;
using SharedLibrary.SharedKernel.Http.Interfaces;

namespace BookingService.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(
            this IServiceCollection services, IConfiguration configuration)
        {
            // Add AutoMapper
            services.AddAutoMapper(typeof(DependencyInjection).Assembly);

            // Add application services here
            services.AddScoped<IBookingUseCase, BookingUseCase>();
            services.AddScoped<IDrivingSessionUseCase, DrivingSessionUseCase>();
            services.AddScoped<IDrivingSkillUseCase, DrivingSkillUseCase>();
            services.AddScoped<IRoadTypeUseCase, RoadTypeUseCase>();
            services.AddScoped<IFeedbackUseCase, FeedbackUseCase>();
            services.AddScoped<IPackageUseCase, PackageUseCase>();
            services.AddScoped<ICarUseCase, CarUseCase>();
            services.AddScoped<IBrandUseCase, BrandUseCase>();
          //  services.AddScoped<IInstructorRoutesUseCase, InstructorRoutesUseCase>();

            services.AddScoped<IPayment, Payment>();
            services.AddScoped<IUser, User>();

            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddHttpClient<HttpService>();
            services.AddScoped<HttpService>();


            return services;
        }
    }
}
