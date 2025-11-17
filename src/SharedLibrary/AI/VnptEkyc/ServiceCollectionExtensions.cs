using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace SharedLibrary.AI.VnptEkyc
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddVnptEkyc(
            this IServiceCollection services,
            IConfiguration configuration,
            string sectionName = VnptEkycOptions.SectionName)
        {
            services.Configure<VnptEkycOptions>(configuration.GetSection(sectionName));

            services.AddHttpClient<IVnptEkycService, VnptEkycService>((serviceProvider, client) =>
            {
                var options = serviceProvider
                    .GetRequiredService<IOptionsMonitor<VnptEkycOptions>>()
                    .CurrentValue;

                if (string.IsNullOrWhiteSpace(options.BaseUrl))
                {
                    throw new InvalidOperationException("VNPT eKYC base URL is not configured.");
                }

                client.BaseAddress = new Uri(options.BaseUrl);

                if (options.HttpTimeoutSeconds > 0)
                {
                    client.Timeout = TimeSpan.FromSeconds(options.HttpTimeoutSeconds);
                }
            });

            return services;
        }
    }
}

