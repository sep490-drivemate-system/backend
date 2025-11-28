using System.Net;
using System.Text.Json;
using SharedLibrary.SharedKernel.ServiceResult;
using SharedLibrary.SharedKernel.Messages;

namespace ApiGetwate.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public GlobalExceptionMiddleware(
            RequestDelegate next,
            ILogger<GlobalExceptionMiddleware> logger,
            IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, LogMessages.UnauthorizedAccessException);
                await HandleExceptionAsync(context, ex, HttpStatusCode.Unauthorized, ServiceError.Unauthorized);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, LogMessages.ServiceCommunicationError);
                await HandleExceptionAsync(context, ex, HttpStatusCode.ServiceUnavailable, ServiceError.ServiceUnavailable);
            }
            catch (TaskCanceledException ex)
            {
                _logger.LogWarning(ex, LogMessages.RequestTimeout);
                await HandleExceptionAsync(context, ex, HttpStatusCode.GatewayTimeout, ServiceError.GatewayTimeout);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, LogMessages.UnhandledException);
                await HandleExceptionAsync(context, ex, HttpStatusCode.InternalServerError, ServiceError.Unhandled);
            }
        }

        private async Task HandleExceptionAsync(
            HttpContext context,
            Exception exception,
            HttpStatusCode statusCode,
            string errorCode)
        {
            if (context.Response.HasStarted)
            {
                _logger.LogWarning(LogMessages.ResponseAlreadyStarted);
                return;
            }

            context.Response.Clear();
            context.Response.ContentType = "application/json; charset=utf-8";
            context.Response.StatusCode = (int)statusCode;

            var message = statusCode switch
            {
                HttpStatusCode.Unauthorized => ErrorMessages.Unauthorized,
                HttpStatusCode.Forbidden => ErrorMessages.Forbidden,
                HttpStatusCode.ServiceUnavailable => ErrorMessages.ServiceUnavailable,
                HttpStatusCode.GatewayTimeout => ErrorMessages.GatewayTimeout,
                HttpStatusCode.BadRequest => ErrorMessages.BadRequest,
                HttpStatusCode.NotFound => ErrorMessages.NotFound,
                _ => _env.IsDevelopment()
                    ? $"Lỗi hệ thống: {exception.Message}"
                    : ErrorMessages.InternalServerError
            };

            var response = new ApiResponse<object>
            {
                IsSuccess = false,
                Message = message,
                ErrorCode = errorCode,
                Value = _env.IsDevelopment() ? new
                {
                    ExceptionType = exception.GetType().Name,
                    exception.Message,
                    exception.StackTrace,
                    InnerException = exception.InnerException?.Message
                } : null
            };

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = _env.IsDevelopment(),
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            };

            var json = JsonSerializer.Serialize(response, options);
            await context.Response.WriteAsync(json);
        }
    }

    public static class GlobalExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<GlobalExceptionMiddleware>();
        }
    }
}

