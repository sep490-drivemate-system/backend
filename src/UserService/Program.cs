
using Microsoft.EntityFrameworkCore;
using UserService.Application;
using UserService.Infrastructure;
using UserService.Infrastructure.Persistence.Context;

namespace UserService
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            DotNetEnv.Env.Load("../../.env");
            var builder = WebApplication.CreateBuilder(args);
            var config = builder.Configuration;
            builder.Configuration.AddEnvironmentVariables();
            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddInfrastructure(config);
            builder.Services.AddApplication(config);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });
            var app = builder.Build();

            // Auto-migrate database on startup
            app.Logger.LogInformation("Starting database migration...");
            using (var scope = app.Services.CreateScope())
            {
                try
                {
                    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    app.Logger.LogInformation("Got ApplicationDbContext, checking connection...");
                    
                    // Test connection first
                    await context.Database.CanConnectAsync();
                    app.Logger.LogInformation("Database connection successful, running migrations...");
                    
                    // Run migrations
                    await context.Database.MigrateAsync();
                    app.Logger.LogInformation("Database migration completed successfully");
                    
                    // Log existing tables
                    var tables = await context.Database.SqlQueryRaw<string>("SELECT table_name FROM information_schema.tables WHERE table_schema = 'public'").ToListAsync();
                    app.Logger.LogInformation($"Tables in database: {string.Join(", ", tables)}");
                }
                catch (Exception ex)
                {
                    app.Logger.LogError(ex, "Database migration failed: {Message}", ex.Message);
                    app.Logger.LogError("Connection string: {ConnectionString}", app.Configuration.GetConnectionString("USERSERVICECONNECTION"));
                }
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            
            // Enable Swagger in production for Railway
            if (app.Environment.IsProduction())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();
            app.UseCors("AllowAll");

            app.MapControllers();
            
            // Health check endpoints
            app.MapGet("/health", () => Results.Ok(new { status = "healthy", service = "UserService", timestamp = DateTime.UtcNow }));
            
            app.MapGet("/health/database", async (ApplicationDbContext context) =>
            {
                try
                {
                    await context.Database.CanConnectAsync();
                    return Results.Ok(new { status = "healthy", database = "connected", timestamp = DateTime.UtcNow });
                }
                catch (Exception ex)
                {
                    return Results.Problem($"Database connection failed: {ex.Message}");
                }
            });

            app.Run();
        }
    }
}
