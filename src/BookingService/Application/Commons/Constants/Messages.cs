namespace BookingService.Application.Commons.Constants
{
    public static class Messages
    {
        public static class Booking
        {
            public const string INSUFFICENTCREDIT = "Bạn không đủ tiền trong ví. Vui lòng nạp thêm tiền";
            public const string PAYMENTSUCCESS = "Thanh toán thành công";
        }

        public static class Commons
        {
            public const string SUCCESS = "Thành công";
            public const string NOTFOUND = "Không thể tìm thấy tài nguyên cần thiết để thực hiện";
            public const string UNHANDLED = "Đã có lỗi xảy ra và hệ thống không thể khắc phục, vui lòng kiểm tra lại thông tin và thử lại sau.";
        }

    }
}
