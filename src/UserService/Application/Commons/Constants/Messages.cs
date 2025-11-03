namespace UserService.Application.Commons.Constants
{
    public static class Messages
    {
        public static class Auth
        {
            public const string EmailAlreadyExists = "Email đã tồn tại.";
            public const string PhoneAlreadyExists = "Số điện thoại đã tồn tại.";
            public const string UserNameAlreadyExists = "Tên người dùng đã tồn tại.";
            public const string UserNorExists = "Người dùng không tồn tại.";
            public const string EmailSentSuccess = "Email đã gửi thành công.";
            public const string WrongPassword = "Sai mật khẩu";
            public const string SmsSentSuccess = "Sms đã gửi thành công.";
            public const string InvalidCredentials = "Email hoặc mật khẩu không đúng.";
            public const string TokenNoExists = "Xảy ra lỗi hãy thử lại sao.";
        }

        public static class User
        {
            public const string UserNotFound = "Không tìm thấy người dùng.";
            public const string UserCreated = "Tạo tài khoản thành công.";
        }

        public static class Common
        {
            public const string Success = "Thành công";
            public const string UnknownError = "Có lỗi xảy ra, vui lòng thử lại sau!";
            public const string ActionNotSupported = "Hành động không được hỗ trợ bởi hệ thống!";
            public const string InvalidDocumentType = "Tài liệu tải lên không phù hợp hoặc có định dạng không được hỗ trợ!";
            public const string RequiredField = "Vui lòng nhập đầy đủ thông tin cần thiết!";
            public const string EmailError = "Có lỗi xảy ra khi gửi email, vui lòng thử lại sau.";
            public const string PhoneError = "Có lỗi xảy ra khi gửi sms, vui lòng thử lại sau.";
            public const string NotFoundError = "Tài nguyên bạn đang cố truy cập không tồn tại";
        }

        public static class InstructorApplication
        {
            public const string InvalidDrivingLicenseTier = "Hạng bằng lái xe của hướng dẫn viên không phù hợp!";
            public const string InvalidGender = "Hướng dẫn viên bắt buộc phải chọn giới tính bản thân!";
            public const string EmailUsed = "Đã tồn tại tài khoản sử dụng email này!";
        }
    }
}
