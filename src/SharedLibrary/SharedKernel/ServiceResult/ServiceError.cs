using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.SharedKernel.ServiceResult
{
    public sealed record ServiceError(string Code, string? Description = null, Dictionary<string, object>? Information = null)
    {
        public const string BadRequest = "BadRequestError";                 // 400
        public const string Unauthorized = "UnauthorizedError";            // 401
        public const string Forbidden = "ForbiddenError";                  // 403
        public const string NotFound = "NotFoundError";                    // 404
        public const string MethodNotAllowed = "MethodNotAllowedError";    // 405
        public const string Conflict = "ConflictError";                    // 409
        public const string UnprocessableEntity = "UnprocessableEntityError"; // 422
        public const string TooManyRequests = "TooManyRequestsError";      // 429

        public const string Unhandled = "InternalError";                   // 500
        public const string ExternalService = "ExternalServiceError";     // 502
        public const string ServiceUnavailable = "ServiceUnavailableError"; // 503
        public const string GatewayTimeout = "GatewayTimeoutError";       // 504
        public const string NotImplemented = "NotImplementedError";       // 501

        public const string Existed = "EntityExistedError";
        public const string EntityNotFound = "EntityNotFoundError";
        public const string EntityInUse = "EntityInUseError";
        public const string InvalidState = "InvalidStateError";
        public const string QuotaExceeded = "QuotaExceededError";
        public const string AlreadyProcessed = "AlreadyProcessedError";
        public const string RuleViolation = "RuleViolationError";

        public static readonly ServiceError None = new(string.Empty);

        // === Factory Methods ===
        public static ServiceError BadRequestError(string description) => new(BadRequest, description);
        public static ServiceError UnauthorizedError(string description) => new(Unauthorized, description);
        public static ServiceError ForbiddenError(string description) => new(Forbidden, description);
        public static ServiceError NotFoundError(string description) => new(NotFound, description);
        public static ServiceError MethodNotAllowedError(string description) => new(MethodNotAllowed, description);
        public static ServiceError ConflictError(string description) => new(Conflict, description);
        public static ServiceError UnprocessableEntityError(string description) => new(UnprocessableEntity, description);
        public static ServiceError TooManyRequestsError(string description) => new(TooManyRequests, description);

        public static ServiceError UnhandledException(string description) => new(Unhandled, description);
        public static ServiceError ExternalServiceError(string description) => new(ExternalService, description);
        public static ServiceError ServiceUnavailableError(string description) => new(ServiceUnavailable, description);
        public static ServiceError GatewayTimeoutError(string description) => new(GatewayTimeout, description);
        public static ServiceError NotImplementedError(string description) => new(NotImplemented, description);

        public static ServiceError ExistedError(string description) => new(Existed, description);
        public static ServiceError EntityNotFoundError(string description) => new(EntityNotFound, description);
        public static ServiceError EntityInUseError(string description) => new(EntityInUse, description);
        public static ServiceError InvalidStateError(string description) => new(InvalidState, description);
        public static ServiceError QuotaExceededError(string description) => new(QuotaExceeded, description);
        public static ServiceError AlreadyProcessedError(string description) => new(AlreadyProcessed, description);
        public static ServiceError RuleViolationError(string description) => new(RuleViolation, description);
    }
}
