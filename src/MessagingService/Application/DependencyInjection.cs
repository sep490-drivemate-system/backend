using MessagingService.Application.Interfaces;
using MessagingService.Application.UseCases;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedLibrary.SharedKernel.Http;
using SharedLibrary.SharedKernel.Http.Implementation;
using SharedLibrary.SharedKernel.Http.Interfaces;

namespace MessagingService.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(
            this IServiceCollection services, IConfiguration configuration)
        {
            // Add AutoMapper
            services.AddAutoMapper(typeof(DependencyInjection).Assembly);

            // Register UseCases
            services.AddScoped<IChatUseCase, ChatUseCase>();
            services.AddScoped<INotificationUseCase, NotificationUseCase>();

            // Register HTTP services for calling other microservices
            services.AddHttpClient<HttpService>();
            services.AddScoped<HttpService>();

            return services;
        }
    }
}

