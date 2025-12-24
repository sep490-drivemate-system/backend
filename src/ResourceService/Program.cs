using ResourceService.Infrastructure;
using Microsoft.Extensions.Logging;

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

            // Bind to PORT provided by hosting platform (e.g., Railway)
            var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
            // Railway tự động bind HTTPS - không cần ép HTTP
            // Kestrel sẽ tự động lắng nghe trên PORT được Railway cung cấp

            // Add services to the container.
            builder.Services.ConfigureInfrastructure(config);
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            var app = builder.Build();
            
            var logger = app.Services.GetRequiredService<ILogger<Program>>();
            logger.LogInformation("ResourceService is starting...");
            logger.LogInformation($"Environment: {app.Environment.EnvironmentName}");
            logger.LogInformation($"Listening on port: {port}");

            // Add request logging middleware
            app.Use(async (context, next) =>
            {
                logger.LogInformation($"Incoming request: {context.Request.Method} {context.Request.Path}");
                try
                {
                    await next();
                    logger.LogInformation($"Request completed: {context.Request.Method} {context.Request.Path} - Status: {context.Response.StatusCode}");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, $"Error processing request: {context.Request.Method} {context.Request.Path}");
                    throw;
                }
            });

            // Enable Swagger in all environments (including Production on Railway)
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Resource Service API v1");
                c.RoutePrefix = "swagger";
            });

            // Do not force HTTPS redirection inside container; Railway terminates HTTPS at the edge
            // and forwards HTTP into the container. Forcing HTTPS here can cause redirect issues.
            // app.UseHttpsRedirection();

            app.UseCors("AllowAll");
            
            // Simple health endpoint BEFORE authentication (so it's always accessible)
            app.MapGet("/health", () => 
            {
                logger.LogInformation("Health endpoint called");
                return Results.Ok("OK from ResourceService");
            }).AllowAnonymous();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            
            logger.LogInformation("ResourceService startup complete. Ready to accept requests.");
            
            app.Run();
        }
    }
}
