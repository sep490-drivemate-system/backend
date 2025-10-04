
using ResourceService.Repositories;

namespace ResourceService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            DotNetEnv.Env.Load("../../.env");
            var builder = WebApplication.CreateBuilder(args);
            var config = builder.Configuration;
            builder.Configuration.AddEnvironmentVariables();
            // Add services to the container.

            builder.Services.AddControllers();
            builder.Configuration.AddEnvironmentVariables();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddRepositories(config);
            builder.Services.AddSwaggerGen();


            Console.WriteLine($"CONNECTIONSTRINGS__BLOGSERVICECONNECTION");
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
