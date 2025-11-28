namespace SharedLibrary.SharedKernel.Messages
{
    public static class LogMessages
    {
        #region Authentication & Authorization Logs

        /// <summary>
        /// Log khi có unauthorized access attempt
        /// Parameters: {Path}, {IP}
        /// </summary>
        public const string UnauthorizedAccessAttempt = "Unauthorized access attempt to {Path} from {IP}";

        /// <summary>
        /// Log khi có forbidden access attempt
        /// Parameters: {User}, {Path}
        /// </summary>
        public const string ForbiddenAccessAttempt = "Forbidden access attempt by {User} to {Path}";

        /// <summary>
        /// Log khi token không hợp lệ
        /// Parameters: {IP}
        /// </summary>
        public const string InvalidTokenDetected = "Invalid token detected from {IP}";

        /// <summary>
        /// Log khi token hết hạn
        /// Parameters: {IP}
        /// </summary>
        public const string ExpiredTokenDetected = "Expired token detected from {IP}";

        /// <summary>
        /// Log khi user login thành công
        /// Parameters: {UserId}, {Email}
        /// </summary>
        public const string UserLoginSuccess = "User {UserId} ({Email}) logged in successfully";

        /// <summary>
        /// Log khi user logout
        /// Parameters: {UserId}
        /// </summary>
        public const string UserLogout = "User {UserId} logged out";

        /// <summary>
        /// Log khi login thất bại
        /// Parameters: {Email}, {IP}
        /// </summary>
        public const string LoginFailed = "Failed login attempt for {Email} from {IP}";

        #endregion

        #region Service Communication Logs

        /// <summary>
        /// Log khi service communication error
        /// </summary>
        public const string ServiceCommunicationError = "Service communication error";

        /// <summary>
        /// Log khi request timeout
        /// </summary>
        public const string RequestTimeout = "Request timeout";

        /// <summary>
        /// Log khi service không khả dụng
        /// Parameters: {ServiceName}
        /// </summary>
        public const string ServiceUnavailable = "Service unavailable: {ServiceName}";

        /// <summary>
        /// Log khi gọi external service
        /// Parameters: {ServiceName}, {Endpoint}
        /// </summary>
        public const string CallingExternalService = "Calling external service: {ServiceName} at {Endpoint}";

        /// <summary>
        /// Log khi external service response
        /// Parameters: {ServiceName}, {StatusCode}, {Duration}
        /// </summary>
        public const string ExternalServiceResponse = "External service {ServiceName} responded with {StatusCode} in {Duration}ms";

        #endregion

        #region Exception Logs

        /// <summary>
        /// Log cho unhandled exception
        /// </summary>
        public const string UnhandledException = "Unhandled exception";

        /// <summary>
        /// Log cho unauthorized exception at Gateway
        /// </summary>
        public const string UnauthorizedAccessException = "Unauthorized access exception at Gateway";

        /// <summary>
        /// Log khi response đã start không thể modify
        /// </summary>
        public const string ResponseAlreadyStarted = "Response has already started, cannot handle exception";

        /// <summary>
        /// Log cho validation exception
        /// Parameters: {Errors}
        /// </summary>
        public const string ValidationException = "Validation failed: {Errors}";

        /// <summary>
        /// Log cho business rule violation
        /// Parameters: {Rule}, {Details}
        /// </summary>
        public const string BusinessRuleViolation = "Business rule violation: {Rule}. Details: {Details}";

        #endregion

        #region Request/Response Logs

        /// <summary>
        /// Log cho incoming request
        /// Parameters: {Method}, {Path}
        /// </summary>
        public const string IncomingRequest = "Incoming request: {Method} {Path}";

        /// <summary>
        /// Log cho outgoing response
        /// Parameters: {StatusCode}, {Path}, {Duration}
        /// </summary>
        public const string OutgoingResponse = "Outgoing response: {StatusCode} for {Path} in {Duration}ms";

        /// <summary>
        /// Log request với body (chỉ dùng cho debug)
        /// Parameters: {Method}, {Path}, {Body}
        /// </summary>
        public const string RequestWithBody = "Request {Method} {Path} with body: {Body}";

        /// <summary>
        /// Log response với body (chỉ dùng cho debug)
        /// Parameters: {StatusCode}, {Body}
        /// </summary>
        public const string ResponseWithBody = "Response {StatusCode} with body: {Body}";

        #endregion

        #region Database Logs

        /// <summary>
        /// Log khi query database
        /// Parameters: {Query}
        /// </summary>
        public const string DatabaseQuery = "Executing database query: {Query}";

        /// <summary>
        /// Log khi database error
        /// Parameters: {Error}
        /// </summary>
        public const string DatabaseError = "Database error: {Error}";

        /// <summary>
        /// Log khi transaction started
        /// Parameters: {TransactionId}
        /// </summary>
        public const string TransactionStarted = "Database transaction started: {TransactionId}";

        /// <summary>
        /// Log khi transaction committed
        /// Parameters: {TransactionId}
        /// </summary>
        public const string TransactionCommitted = "Database transaction committed: {TransactionId}";

        /// <summary>
        /// Log khi transaction rolled back
        /// Parameters: {TransactionId}, {Reason}
        /// </summary>
        public const string TransactionRolledBack = "Database transaction rolled back: {TransactionId}. Reason: {Reason}";

        #endregion

        #region Payment & Wallet Logs

        /// <summary>
        /// Log khi payment initiated
        /// Parameters: {UserId}, {Amount}
        /// </summary>
        public const string PaymentInitiated = "Payment initiated by user {UserId} for amount {Amount}";

        /// <summary>
        /// Log khi payment successful
        /// Parameters: {TransactionId}, {UserId}, {Amount}
        /// </summary>
        public const string PaymentSuccessful = "Payment {TransactionId} successful: User {UserId} paid {Amount}";

        /// <summary>
        /// Log khi payment failed
        /// Parameters: {UserId}, {Amount}, {Reason}
        /// </summary>
        public const string PaymentFailed = "Payment failed for user {UserId}, amount {Amount}. Reason: {Reason}";

        /// <summary>
        /// Log khi wallet created
        /// Parameters: {WalletId}, {UserId}
        /// </summary>
        public const string WalletCreated = "Wallet {WalletId} created for user {UserId}";

        /// <summary>
        /// Log khi balance updated
        /// Parameters: {WalletId}, {OldBalance}, {NewBalance}
        /// </summary>
        public const string BalanceUpdated = "Wallet {WalletId} balance updated from {OldBalance} to {NewBalance}";

        #endregion

        #region Booking Logs

        /// <summary>
        /// Log khi booking created
        /// Parameters: {BookingId}, {UserId}, {InstructorId}
        /// </summary>
        public const string BookingCreated = "Booking {BookingId} created: User {UserId} with Instructor {InstructorId}";

        /// <summary>
        /// Log khi booking cancelled
        /// Parameters: {BookingId}, {Reason}
        /// </summary>
        public const string BookingCancelled = "Booking {BookingId} cancelled. Reason: {Reason}";

        /// <summary>
        /// Log khi booking confirmed
        /// Parameters: {BookingId}
        /// </summary>
        public const string BookingConfirmed = "Booking {BookingId} confirmed";

        #endregion

        #region Background Job Logs

        /// <summary>
        /// Log khi background job started
        /// Parameters: {JobName}
        /// </summary>
        public const string JobStarted = "Background job started: {JobName}";

        /// <summary>
        /// Log khi background job completed
        /// Parameters: {JobName}, {Duration}
        /// </summary>
        public const string JobCompleted = "Background job completed: {JobName} in {Duration}ms";

        /// <summary>
        /// Log khi background job failed
        /// Parameters: {JobName}, {Error}
        /// </summary>
        public const string JobFailed = "Background job failed: {JobName}. Error: {Error}";

        #endregion

        #region Email & Notification Logs

        /// <summary>
        /// Log khi sending email
        /// Parameters: {To}, {Subject}
        /// </summary>
        public const string SendingEmail = "Sending email to {To} with subject: {Subject}";

        /// <summary>
        /// Log khi email sent successfully
        /// Parameters: {To}
        /// </summary>
        public const string EmailSent = "Email sent successfully to {To}";

        /// <summary>
        /// Log khi email failed
        /// Parameters: {To}, {Error}
        /// </summary>
        public const string EmailFailed = "Failed to send email to {To}. Error: {Error}";

        /// <summary>
        /// Log khi notification sent
        /// Parameters: {UserId}, {Type}
        /// </summary>
        public const string NotificationSent = "Notification sent to user {UserId} of type {Type}";

        #endregion

        #region Cache Logs

        /// <summary>
        /// Log khi cache hit
        /// Parameters: {Key}
        /// </summary>
        public const string CacheHit = "Cache hit for key: {Key}";

        /// <summary>
        /// Log khi cache miss
        /// Parameters: {Key}
        /// </summary>
        public const string CacheMiss = "Cache miss for key: {Key}";

        /// <summary>
        /// Log khi cache invalidated
        /// Parameters: {Key}
        /// </summary>
        public const string CacheInvalidated = "Cache invalidated for key: {Key}";

        #endregion

        #region Application Lifecycle Logs

        /// <summary>
        /// Log khi application starting
        /// </summary>
        public const string ApplicationStarting = "Application starting";

        /// <summary>
        /// Log khi application started
        /// </summary>
        public const string ApplicationStarted = "Application started successfully";

        /// <summary>
        /// Log khi application stopping
        /// </summary>
        public const string ApplicationStopping = "Application stopping";

        /// <summary>
        /// Log khi application stopped
        /// </summary>
        public const string ApplicationStopped = "Application stopped";

        #endregion
    }
}

