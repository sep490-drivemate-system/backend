
using PaymentService.Application;
using PaymentService.Infrastructure;
using Microsoft.EntityFrameworkCore;
using PaymentService.Infrastructure.Data;

namespace PaymentService
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            DotNetEnv.Env.Load("../../.env");
            var builder = WebApplication.CreateBuilder(args);
            builder.Configuration.AddEnvironmentVariables();
            // Add services to the container.
            builder.Services.AddControllers();
            
            // Add CQRS Application layer
            builder.Services.AddApplication();
            
            // Add Infrastructure layer
            builder.Services.AddInfrastructure(builder.Configuration);

            // Add CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new() { Title = "Payment Service API", Version = "v1" });
            });

            var app = builder.Build();

            // Auto-migrate database
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<PaymentDbContext>();
                await context.Database.MigrateAsync();
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors("AllowAll");
            app.UseAuthorization();

            // Add health check endpoints
            app.MapHealthChecks("/health");
            app.MapHealthChecks("/health/database", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
            {
                Predicate = check => check.Tags.Contains("database")
            });

            app.MapControllers();

            await app.RunAsync();
        }
    }
}
