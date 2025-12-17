namespace ResourceService.Domain.Constants
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
            public const string APPROVE_INVALID_STATUS = "Không thể duyệt blog. Blog đã được duyệt hoặc từ chối trước đó";
            public const string REJECT_SUCCESS = "Từ chối blog thành công";
            public const string REJECT_FAILED = "Từ chối blog không thành công";
            public const string REJECT_INVALID_STATUS = "Không thể từ chối blog. Blog đã được duyệt hoặc từ chối trước đó";
            public const string BAN_SUCCESS = "Cấm blog thành công";
            public const string BAN_FAILED = "Cấm blog không thành công";
            public const string UNBAN_SUCCESS = "Gỡ cấm blog thành công";
            public const string UNBAN_FAILED = "Gỡ cấm blog không thành công";
            public const string UNBAN_INVALID_STATUS = "Không thể gỡ cấm blog. Blog hiện không ở trạng thái bị cấm";
        }

        public static class Category
        {
            public const string CREATE_SUCCESS = "Tạo danh mục thành công";
            public const string CREATE_FAILED = "Tạo danh mục không thành công";
            public const string UPDATE_SUCCESS = "Cập nhật danh mục thành công";
            public const string UPDATE_FAILED = "Cập nhật danh mục không thành công";
            public const string NAME_REQUIRED = "Tên danh mục không được để trống";
            public const string NAME_ALREADY_EXISTS = "Tên danh mục đã tồn tại";
            public const string DELETE_SUCCESS = "Xóa danh mục thành công";
            public const string DELETE_FAILED = "Xóa danh mục không thành công";
            public const string CANNOT_DELETE_IN_USE = "Không thể xóa danh mục vì đang được sử dụng bởi một hoặc nhiều blog";
        }

        public static class Commons
        {
            public const string SUCCESS = "Thành công";
            public const string NOTFOUND = "Không thể tìm thấy tài nguyên cần thiết để thực hiện";
            public const string UNHANDLED = "Đã có lỗi xảy ra và hệ thống không thể khắc phục, vui lòng kiểm tra lại thông tin và thử lại sau.";
        }

        public static class Voucher
        {
            public const string CODE_ALREADY_EXISTS = "Mã voucher đã tồn tại";
            public const string INVALID_DISCOUNT_PERCENTAGE = "Phần trăm giảm giá phải từ 1 đến 100";
            public const string INVALID_DATE_RANGE = "Ngày bắt đầu phải nhỏ hơn ngày kết thúc";
            public const string INVALID_MIN_ORDER_AMOUNT = "Giá trị đơn hàng tối thiểu không được âm";
            public const string INVALID_MAX_DISCOUNT_AMOUNT = "Giá trị giảm tối đa không được âm";
            public const string INVALID_USAGE_LIMIT = "Số lần sử dụng tối đa phải lớn hơn 0";
            public const string CREATE_FAILED = "Tạo voucher không thành công";
            public const string CREATE_SUCCESS = "Tạo voucher thành công";
            public const string UPDATE_SUCCESS = "Cập nhật voucher thành công";
            public const string UPDATE_FAILED = "Cập nhật voucher không thành công";
            public const string DELETE_SUCCESS = "Xóa voucher thành công";
            public const string DELETE_FAILED = "Xóa voucher không thành công";
            public const string NOT_FOUND = "Không tìm thấy voucher";
            public const string NOT_ACTIVE = "Voucher đang tạm khóa";
            public const string NOT_STARTED = "Voucher chưa bắt đầu hiệu lực";
            public const string EXPIRED = "Voucher đã hết hạn";
            public const string MIN_ORDER_NOT_REACHED = "Giá trị đơn hàng chưa đạt mức tối thiểu để áp dụng voucher";
            public const string USAGE_LIMIT_REACHED = "Bạn đã dùng hết số lần cho phép của voucher này";
            public const string INVALID_ORDER_AMOUNT = "Giá trị đơn hàng phải lớn hơn 0";
            public const string USE_SUCCESS = "Áp dụng voucher thành công";
            public const string CHECK_SUCCESS = "Voucher hợp lệ, bạn có thể áp dụng";
        }
        public static class Tag
        {
            public const string SLUG_ALREADY_EXISTS = "Slug này đã tồn tại.";
            public const string TAG_ALREADY_EXISTS = "Tag này đã tồn tại.";


        }
    }
}

