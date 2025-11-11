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
        }

        public static class Commons
        {
            public const string SUCCESS = "Thành công";
            public const string NOTFOUND = "Không thể tìm thấy tài nguyên cần thiết để thực hiện";
            public const string UNHANDLED = "Đã có lỗi xảy ra và hệ thống không thể khắc phục, vui lòng kiểm tra lại thông tin và thử lại sau.";
        }
    }
}

