using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.SharedKernel.ServiceResult
{
    public static class ResultExtensions
    {
        public static IActionResult ToActionResult<T>(this Result<T> result)
        {
            var response = new ApiResponse<T>
            {
                IsSuccess = result.IsSuccess,
                Message = result.Message ?? (result.IsSuccess ? "Success" : "Error"),
                ErrorCode = result.Error?.Code,
                Data = result.Data
            };

            var statusCode = result.IsSuccess ? StatusCodes.Status200OK : MapErrorCodeToStatusCode(result.Error?.Code);

            return new ObjectResult(response)
            {
                StatusCode = statusCode
            };
        }

        private static int MapErrorCodeToStatusCode(string? code)
        {
            return code switch
            {
                // 4xx: Client Errors
                ServiceError.BadRequest => StatusCodes.Status400BadRequest,
                ServiceError.Unauthorized => StatusCodes.Status401Unauthorized,
                ServiceError.Forbidden => StatusCodes.Status403Forbidden,
                ServiceError.NotFound => StatusCodes.Status404NotFound,
                ServiceError.MethodNotAllowed => StatusCodes.Status405MethodNotAllowed,
                ServiceError.Conflict => StatusCodes.Status409Conflict,
                ServiceError.UnprocessableEntity => StatusCodes.Status422UnprocessableEntity,
                ServiceError.TooManyRequests => StatusCodes.Status429TooManyRequests,

                // 5xx: Server Errors
                ServiceError.NotImplemented => StatusCodes.Status501NotImplemented,
                ServiceError.Unhandled => StatusCodes.Status500InternalServerError,
                ServiceError.ExternalService => StatusCodes.Status502BadGateway,
                ServiceError.ServiceUnavailable => StatusCodes.Status503ServiceUnavailable,
                ServiceError.GatewayTimeout => StatusCodes.Status504GatewayTimeout,

                // Business Logic Errors 
                ServiceError.Existed => StatusCodes.Status409Conflict,
                ServiceError.EntityNotFound => StatusCodes.Status404NotFound,
                ServiceError.EntityInUse => StatusCodes.Status400BadRequest,
                ServiceError.InvalidState => StatusCodes.Status400BadRequest,
                ServiceError.QuotaExceeded => StatusCodes.Status429TooManyRequests,
                ServiceError.AlreadyProcessed => StatusCodes.Status409Conflict,
                ServiceError.RuleViolation => StatusCodes.Status422UnprocessableEntity,

                _ => StatusCodes.Status500InternalServerError
            };
        }
    }
}
