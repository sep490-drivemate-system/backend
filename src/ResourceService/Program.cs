
using Microsoft.EntityFrameworkCore;
namespace ResourceService
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
           

            app.Run();
        }
    }
}
