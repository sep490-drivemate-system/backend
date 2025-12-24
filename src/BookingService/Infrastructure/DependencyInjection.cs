using BookingService.Application.Interfaces;
using BookingService.Infrastructure.Jobs;
using BookingService.Infrastructure.Persistence.Context;
using BookingService.Infrastructure.UoW;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Resend;
using SharedLibrary.CloudinaryStorage;
using SharedLibrary.Email;
using SharedLibrary.Jwt;
using SharedLibrary.SharedKernel.Http.Implementation;
using SharedLibrary.SharedKernel.Http.Interfaces;


namespace BookingService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
           this IServiceCollection services, IConfiguration configuration)
        {
            // Register database context
            services.AddDbContext<BookingDbContext>(options =>
            {
                var connectionString = configuration.GetConnectionString("BOOKINGSERVICECONNECTION");
                options.UseNpgsql(connectionString);
            });
            
            // Đăng ký dịch vụ hệ thống
            services.AddScoped<IUnitOfWork,UnitOfWork>();
            services.AddScoped<IJwtService, JwtService>();

            // Đăng kí Hangfire
            CreateHangFireDatabase(configuration); // Create hangfire db if not exist.

            services.AddHangfire((sp, config) =>
            {
                config.UsePostgreSqlStorage(options => options.UseNpgsqlConnection(configuration.GetConnectionString("BOOKINGSERVICEHANGFIRE")));
            });

            services.AddHangfireServer();

            // Đăng kí job chạy ngầm
            services.AddScoped<RunningJobs>();

            // Đăng ký sử dụng HttpClient gọi đến các Microservice bằng phương thức http.
            services.AddHttpClient("UserServiceClient", client =>
            {
                var base_address = configuration.GetConnectionString("Userservice_connection");

                if (string.IsNullOrEmpty(base_address))
                {
                    throw new ApplicationException("Can not find base address for user service");
                }

                client.BaseAddress = new Uri(base_address);
                client.DefaultRequestHeaders.Add("User-Agent", "DriveMate_BookingService");
            });

            services.AddHttpClient("PaymentServiceClient", client =>
            {
                var base_address = configuration.GetConnectionString("Paymentservice_connection");

                if (string.IsNullOrEmpty(base_address))
                {
                    throw new ApplicationException("Can not find base address for payment service");
                }

                client.BaseAddress = new Uri(base_address);
                client.DefaultRequestHeaders.Add("User-Agent", "DriveMate_BookingService");
            });

            // Đăng ký dịch vụ bên thứ ba
            services.AddScoped<ICloudinaryServiceProvider, CloudinaryServiceProvider>();
            services.AddScoped<ISystemConfigurationHttpService, SystemConfigurationHttpService>();
            
            // Register Resend for email service
            services.AddHttpClient<ResendClient>();
            services.AddTransient<IResend, ResendClient>();
            services.Configure<ResendClientOptions>(o =>
            {
                o.ApiToken = Environment.GetEnvironmentVariable("RESEND_APITOKEN") ?? Environment.GetEnvironmentVariable("RESEND_API_KEY") ?? string.Empty;
            });
            
            services.AddScoped<IEmailService, EmailService>();

            // Register RabbitMQ service (with fallback to mock if connection fails)            
            //services.AddSingleton<IRabbitMQService, RabbitMQService>();
            //services.AddHostedService<RabbitMQHostedService>();
            return services;
        }

        private static void CreateHangFireDatabase(IConfiguration configuration)
        {
            string? hangfireDbConnectionString = configuration.GetConnectionString("BOOKINGSERVICEHANGFIRE");

            if (hangfireDbConnectionString == null)
            {
                throw new InvalidOperationException("Can not instantiate Hangfire database because no connection string found");
            }

            var dictionary = hangfireDbConnectionString.Split(';', StringSplitOptions.RemoveEmptyEntries)
                .Select(part => part.Split('='))
                .ToDictionary(split => split[0], split => split[1]);

            // Master Db
            string masterDbConnectionString = String.Join(';', dictionary.Select(kv => kv.Key == "Database" ? $"{kv.Key}=postgres" : $"{kv.Key}={kv.Value}"));

            using (var connection = new NpgsqlConnection(masterDbConnectionString))
            {
                connection.Open();

                bool exists = false;
                string checkSql = "SELECT 1 FROM pg_database WHERE datname = @dbName";

                using (var cmd = new NpgsqlCommand(checkSql, connection))
                {
                    cmd.Parameters.AddWithValue("dbName", dictionary["Database"]);
                    exists = cmd.ExecuteScalar() != null;
                }

                if (!exists)
                {
                    string createSql = $"CREATE DATABASE \"{dictionary["Database"]}\"";

                    using (var cmd = new NpgsqlCommand(createSql, connection))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    Console.WriteLine($"Database '{dictionary["Database"]}' created successfully.");
                }
                connection.Close();
            }
        }
    }
}
