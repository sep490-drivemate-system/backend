namespace ResourceService.Services.Commons.Constants
{
    public static class Messages
    {
        public static class Blog
        {
            public const string NOTFOUND = "Không tìm thấy blog";
            public const string RETRIEVE_ERROR = "Lỗi khi lấy danh sách blog";
            public const string DELETE_SUCCESS = "Xóa blog thành công";
            public const string DELETE_FAILED = "Không thể xóa blog";
            public const string CREATE_FAILED = "Tạo không thành công";
            public const string CREATE_SUCCESS = "Tạo thành công";
            public const string UPDATE_FAILED = "Cập nhật blog không thành công";
            public const string UPDATE_SUCCESS = "Cập nhật blog thành công";
            public const string CATEGORY_NOT_FOUND = "Không tìm thấy danh mục";
            public const string APPROVE_SUCCESS = "Duyệt blog thành công";
            public const string APPROVE_FAILED = "Duyệt blog không thành công";
            public const string REJECT_SUCCESS = "Từ chối blog thành công";
            public const string REJECT_FAILED = "Từ chối blog không thành công";
            public const string BAN_SUCCESS = "Cấm blog thành công";
            public const string BAN_FAILED = "Cấm blog không thành công";
        }

        public static class Commons
        {
            public const string SUCCESS = "Thành công";
            public const string NOTFOUND = "Không thể tìm thấy tài nguyên cần thiết để thực hiện";
            public const string UNHANDLED = "Đã có lỗi xảy ra và hệ thống không thể khắc phục, vui lòng kiểm tra lại thông tin và thử lại sau.";
        }
    }
}

