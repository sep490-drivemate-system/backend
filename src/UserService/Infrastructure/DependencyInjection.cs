using Dapper;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using SharedLibrary.CloudinaryStorage;
using SharedLibrary.Email;
using SharedLibrary.SharedKernel.Password;
using UserService.Application.Interfaces;
using UserService.Application.Jobs;
using UserService.Infrastructure.Persistence.Context;
using UserService.Infrastructure.UoW;

namespace UserService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Đăng ký DbContext
            services.AddDbContext<UserServiceDbContext>(options =>
            {
                var connectionString = configuration.GetConnectionString("USERSERVICECONNECTION");
                // Add timezone to connection string if not already present
                if (!connectionString.Contains("TimeZone", StringComparison.OrdinalIgnoreCase))
                {
                    connectionString += ";TimeZone=Asia/Ho_Chi_Minh";
                }
                options.UseNpgsql(connectionString);
            });

            // Đăng ký Hangfire
            CreateHangFireDatabase(configuration); // Create hangfire db if not exist.

            services.AddHangfire((sp, config) =>
            {
                config.UsePostgreSqlStorage(options => options.UseNpgsqlConnection(configuration.GetConnectionString("USERSERVICEHANGFIRE")));
            });

            services.AddHangfireServer();

            services.AddScoped<RunningJobs>();

            // Đăng ký service khác (cache, email, storage…)
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IPasswordHasherService, PasswordHasherService>();
            services.AddScoped<IEmailService, EmailService>();
            
            // Đăng ký sử dụng HttpClient gọi đến các Microservice bằng phương thức http.
            services.AddHttpClient("BookingServiceClient", client =>
            {
                var base_address = configuration.GetConnectionString("Bookingservice_connection");

                if (string.IsNullOrEmpty(base_address))
                {
                    throw new ApplicationException("Can not find base address for booking service");
                }

                client.BaseAddress = new Uri(base_address);
                client.DefaultRequestHeaders.Add("User-Agent", "DriveMate_UserService");
            });

            // Đăng ký các service bên thứ 3
            services.AddScoped<ICloudinaryServiceProvider, CloudinaryServiceProvider>();

            return services;
        }

        private static void CreateHangFireDatabase(IConfiguration configuration)
        {
            string? hangfireDbConnectionString = configuration.GetConnectionString("USERSERVICEHANGFIRE");

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
