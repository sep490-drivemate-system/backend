namespace SharedLibrary.SharedKernel.Messages
{
    public static class ErrorMessages
    {
        #region Authentication & Authorization Messages (401, 403)
        public const string Unauthorized = "Bạn chưa đăng nhập hoặc phiên đăng nhập đã hết hạn. Vui lòng đăng nhập lại.";
        public const string Forbidden = "Bạn không có quyền truy cập tài nguyên này. Vui lòng liên hệ quản trị viên nếu cần hỗ trợ.";

        public const string InvalidToken = "Token xác thực không hợp lệ. Vui lòng đăng nhập lại.";

        public const string TokenExpired = "Token xác thực đã hết hạn. Vui lòng đăng nhập lại.";
        public const string MissingToken = "Vui lòng đăng nhập để tiếp tục.";

        #endregion

        #region Service Communication Messages (502, 503, 504)
        public const string ServiceUnavailable = "Dịch vụ tạm thời không khả dụng. Vui lòng thử lại sau.";

        /// <summary>
        /// Message khi request timeout - 504
        /// Sử dụng khi: Service phản hồi quá lâu
        /// </summary>
        public const string GatewayTimeout = "Yêu cầu hết thời gian chờ. Vui lòng thử lại.";

        /// <summary>
        /// Message khi service trả về lỗi - 502
        /// Sử dụng khi: Không thể kết nối đến service
        /// </summary>
        public const string BadGateway = "Không thể kết nối đến dịch vụ. Vui lòng thử lại sau.";

        /// <summary>
        /// Message khi communication error giữa các services
        /// </summary>
        public const string ServiceCommunicationError = "Lỗi kết nối giữa các dịch vụ. Vui lòng thử lại.";

        #endregion

        #region Client Error Messages (400, 404, 409, 422)

        /// <summary>
        /// Message cho lỗi hệ thống chung - 500
        /// </summary>
        public const string InternalServerError = "Đã xảy ra lỗi hệ thống. Vui lòng thử lại sau.";

        /// <summary>
        /// Message cho lỗi không tìm thấy resource - 404
        /// </summary>
        public const string NotFound = "Không tìm thấy tài nguyên yêu cầu.";

        /// <summary>
        /// Message cho request không hợp lệ - 400
        /// </summary>
        public const string BadRequest = "Yêu cầu không hợp lệ. Vui lòng kiểm tra lại dữ liệu.";

        /// <summary>
        /// Message cho lỗi validation - 422
        /// </summary>
        public const string ValidationError = "Dữ liệu không hợp lệ. Vui lòng kiểm tra lại thông tin.";

        /// <summary>
        /// Message cho lỗi conflict - 409
        /// Sử dụng khi: Duplicate key, resource đã tồn tại
        /// </summary>
        public const string Conflict = "Dữ liệu đã tồn tại trong hệ thống.";

        /// <summary>
        /// Message cho method không được hỗ trợ - 405
        /// </summary>
        public const string MethodNotAllowed = "Phương thức này không được hỗ trợ.";

        #endregion

        #region Rate Limiting Messages (429)

        /// <summary>
        /// Message khi vượt quá số lần request - 429
        /// </summary>
        public const string TooManyRequests = "Bạn đã gửi quá nhiều yêu cầu. Vui lòng thử lại sau {0} giây.";

        /// <summary>
        /// Message khi vượt quota
        /// </summary>
        public const string QuotaExceeded = "Bạn đã vượt quá hạn mức cho phép.";

        #endregion

        #region Business Logic Error Messages

        /// <summary>
        /// Message khi entity đã tồn tại
        /// </summary>
        public const string EntityAlreadyExists = "Dữ liệu đã tồn tại. Vui lòng kiểm tra lại.";

        /// <summary>
        /// Message khi entity không tìm thấy
        /// </summary>
        public const string EntityNotFound = "Không tìm thấy dữ liệu yêu cầu.";

        /// <summary>
        /// Message khi entity đang được sử dụng
        /// </summary>
        public const string EntityInUse = "Dữ liệu đang được sử dụng, không thể thực hiện thao tác.";

        /// <summary>
        /// Message khi trạng thái không hợp lệ
        /// </summary>
        public const string InvalidState = "Trạng thái hiện tại không cho phép thực hiện thao tác này.";

        /// <summary>
        /// Message khi vi phạm rule nghiệp vụ
        /// </summary>
        public const string RuleViolation = "Thao tác vi phạm quy tắc nghiệp vụ.";

        /// <summary>
        /// Message khi đã xử lý rồi
        /// </summary>
        public const string AlreadyProcessed = "Yêu cầu đã được xử lý trước đó.";

        #endregion

        #region Payment & Wallet Messages

        /// <summary>
        /// Message khi số dư không đủ
        /// </summary>
        public const string InsufficientBalance = "Số dư trong ví không đủ để thực hiện giao dịch.";

        /// <summary>
        /// Message khi payment thất bại
        /// </summary>
        public const string PaymentFailed = "Thanh toán thất bại. Vui lòng thử lại.";

        /// <summary>
        /// Message khi wallet không tồn tại
        /// </summary>
        public const string WalletNotFound = "Không tìm thấy ví của bạn.";

        /// <summary>
        /// Message khi transaction thất bại
        /// </summary>
        public const string TransactionFailed = "Giao dịch thất bại. Vui lòng thử lại.";

        #endregion

        #region User & Authentication Business Messages

        /// <summary>
        /// Message khi email đã được sử dụng
        /// </summary>
        public const string EmailAlreadyExists = "Email này đã được đăng ký.";

        /// <summary>
        /// Message khi username đã được sử dụng
        /// </summary>
        public const string UsernameAlreadyExists = "Tên đăng nhập này đã tồn tại.";

        /// <summary>
        /// Message khi mật khẩu không đúng
        /// </summary>
        public const string InvalidCredentials = "Email hoặc mật khẩu không đúng.";

        /// <summary>
        /// Message khi account bị khóa
        /// </summary>
        public const string AccountLocked = "Tài khoản của bạn đã bị khóa. Vui lòng liên hệ quản trị viên.";

        /// <summary>
        /// Message khi account chưa được kích hoạt
        /// </summary>
        public const string AccountNotActivated = "Tài khoản chưa được kích hoạt. Vui lòng kiểm tra email.";

        #endregion

        #region Maintenance Messages

        /// <summary>
        /// Message khi hệ thống đang bảo trì
        /// </summary>
        public const string UnderMaintenance = "Hệ thống đang được bảo trì. Vui lòng quay lại sau.";

        /// <summary>
        /// Message khi feature chưa được implement
        /// </summary>
        public const string NotImplemented = "Tính năng này chưa được triển khai.";

        #endregion

        #region File Upload Messages

        /// <summary>
        /// Message khi file quá lớn
        /// </summary>
        public const string FileTooLarge = "Kích thước file vượt quá giới hạn cho phép.";

        /// <summary>
        /// Message khi file type không hợp lệ
        /// </summary>
        public const string InvalidFileType = "Loại file không được hỗ trợ.";

        /// <summary>
        /// Message khi upload thất bại
        /// </summary>
        public const string UploadFailed = "Tải file lên thất bại. Vui lòng thử lại.";

        #endregion
    }
}

