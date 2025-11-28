namespace SharedLibrary.SharedKernel.Messages
{
    public static class SuccessMessages
    {
        #region General Success Messages

        /// <summary>
        /// Message chung cho thành công
        /// </summary>
        public const string Success = "Thành công";

        /// <summary>
        /// Message cho operation thành công
        /// </summary>
        public const string OperationSuccessful = "Thao tác thực hiện thành công.";

        #endregion

        #region Authentication Messages

        /// <summary>
        /// Message khi login thành công
        /// </summary>
        public const string LoginSuccessful = "Đăng nhập thành công.";

        /// <summary>
        /// Message khi logout thành công
        /// </summary>
        public const string LogoutSuccessful = "Đăng xuất thành công.";

        /// <summary>
        /// Message khi register thành công
        /// </summary>
        public const string RegistrationSuccessful = "Đăng ký tài khoản thành công.";

        /// <summary>
        /// Message khi refresh token thành công
        /// </summary>
        public const string TokenRefreshed = "Làm mới token thành công.";

        /// <summary>
        /// Message khi đổi mật khẩu thành công
        /// </summary>
        public const string PasswordChanged = "Đổi mật khẩu thành công.";

        /// <summary>
        /// Message khi reset password thành công
        /// </summary>
        public const string PasswordReset = "Đặt lại mật khẩu thành công.";

        #endregion

        #region CRUD Operations

        /// <summary>
        /// Message khi tạo mới thành công
        /// </summary>
        public const string Created = "Tạo mới thành công.";

        /// <summary>
        /// Message khi cập nhật thành công
        /// </summary>
        public const string Updated = "Cập nhật thành công.";

        /// <summary>
        /// Message khi xóa thành công
        /// </summary>
        public const string Deleted = "Xóa thành công.";

        /// <summary>
        /// Message khi lấy dữ liệu thành công
        /// </summary>
        public const string Retrieved = "Lấy dữ liệu thành công.";

        /// <summary>
        /// Message khi lưu thành công
        /// </summary>
        public const string Saved = "Lưu dữ liệu thành công.";

        #endregion

        #region Payment & Wallet Messages

        /// <summary>
        /// Message khi thanh toán thành công
        /// </summary>
        public const string PaymentSuccessful = "Thanh toán thành công.";

        /// <summary>
        /// Message khi nạp tiền thành công
        /// </summary>
        public const string DepositSuccessful = "Nạp tiền vào ví thành công.";

        /// <summary>
        /// Message khi rút tiền thành công
        /// </summary>
        public const string WithdrawalSuccessful = "Rút tiền thành công.";

        /// <summary>
        /// Message khi chuyển tiền thành công
        /// </summary>
        public const string TransferSuccessful = "Chuyển tiền thành công.";

        #endregion

        #region Booking Messages

        /// <summary>
        /// Message khi đặt lịch thành công
        /// </summary>
        public const string BookingCreated = "Đặt lịch học thành công.";

        /// <summary>
        /// Message khi hủy booking thành công
        /// </summary>
        public const string BookingCancelled = "Hủy lịch học thành công.";

        /// <summary>
        /// Message khi xác nhận booking thành công
        /// </summary>
        public const string BookingConfirmed = "Xác nhận lịch học thành công.";

        #endregion

        #region File Upload Messages

        /// <summary>
        /// Message khi upload file thành công
        /// </summary>
        public const string FileUploaded = "Tải file lên thành công.";

        /// <summary>
        /// Message khi xóa file thành công
        /// </summary>
        public const string FileDeleted = "Xóa file thành công.";

        #endregion

        #region Email Messages

        /// <summary>
        /// Message khi gửi email thành công
        /// </summary>
        public const string EmailSent = "Gửi email thành công.";

        /// <summary>
        /// Message khi verify email thành công
        /// </summary>
        public const string EmailVerified = "Xác thực email thành công.";

        #endregion

        #region Notification Messages

        /// <summary>
        /// Message khi gửi thông báo thành công
        /// </summary>
        public const string NotificationSent = "Gửi thông báo thành công.";

        #endregion
    }
}

