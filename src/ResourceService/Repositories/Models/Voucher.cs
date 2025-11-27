using SharedLibrary.SharedKernel.Entities;

namespace ResourceService.Repositories.Models
{
    public class Voucher : BaseEntites
    {
        public string Code { get; set; } = string.Empty; // Mã voucher để người dùng nhập
        public string Name { get; set; } = string.Empty; // Tên voucher
        public string Description { get; set; } = string.Empty; // Mô tả
        
        // Thông tin giảm giá (chỉ giảm theo %)
        public decimal DiscountPercentage { get; set; } // Phần trăm giảm giá (ví dụ: 10 = 10%)
        public decimal? MinOrderAmount { get; set; } // Giá trị đơn hàng tối thiểu để áp dụng
        public decimal? MaxDiscountAmount { get; set; } // Số tiền giảm tối đa
        
        // Thông tin sử dụng
        public int? UsageLimit { get; set; } // Số lần sử dụng tối đa cho từng Novice Driver (null = không giới hạn theo user)
        public int UsedCount { get; set; } = 0; // Tổng số lần voucher đã được sử dụng trên toàn hệ thống (để thống kê / báo cáo)
        // Mỗi Novice Driver được check limit riêng thông qua bảng VoucherUsage
        
        // Thông tin thời gian
        public DateTime StartDate { get; set; } // Ngày bắt đầu hiệu lực
        public DateTime EndDate { get; set; } // Ngày kết thúc hiệu lực
        
        // Trạng thái
        public bool IsActive { get; set; } = true; // Voucher có đang hoạt động không
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
        
        // Quan hệ
        public Guid CreatedBy { get; set; } // Admin tạo voucher
        public ICollection<VoucherUsage> VoucherUsages { get; set; } = new List<VoucherUsage>();
    }
}

