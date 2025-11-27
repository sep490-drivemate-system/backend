using System;
using System.ComponentModel.DataAnnotations;

namespace ResourceService.Services.DTOs.Vouchers
{
    public class VoucherDTO
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal DiscountPercentage { get; set; }
        public decimal? MinOrderAmount { get; set; }
        public decimal? MaxDiscountAmount { get; set; }
        public int? UsageLimit { get; set; } // Giới hạn sử dụng cho từng Novice Driver (null = không giới hạn)
        public int UsedCount { get; set; } // Số lần đã được dùng trên toàn hệ thống (không liên quan tới UsageLimit)
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
    }

    public class VoucherFilterDTO
    {
        [Display(Name = "Is Active", Description = "Lọc voucher đang hoạt động (true) hoặc đã tắt (false). Bỏ trống để lấy tất cả.")]
        public bool? IsActive { get; set; }

        [Display(Name = "Start Date", Description = "Chỉ lấy các voucher có hiệu lực bao trọn ngày này.")]
        public DateTime? StartDate { get; set; }

        [Display(Name = "End Date", Description = "Chỉ lấy các voucher có hiệu lực bao trọn ngày này.")]
        public DateTime? EndDate { get; set; }
    }

    public class VoucherCreateDTO
    {
        [Required]
        [Display(Name = "Voucher Name", Description = "Tên hiển thị của voucher, ví dụ 'Giảm 10% tất cả các gói'.")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Description", Description = "Mô tả chi tiết điều kiện hoặc ghi chú dành cho admin.")]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Range(1, 100, ErrorMessage = "DiscountPercentage phải nằm trong khoảng 1-100%.")]
        [Display(Name = "Discount (%)", Description = "Phần trăm giảm giá (ví dụ nhập 10 nếu muốn giảm 10%).")]
        public decimal DiscountPercentage { get; set; }

        [Display(Name = "Min Order Amount", Description = "Đơn hàng tối thiểu để áp dụng voucher (đơn vị VNĐ). Để trống nếu không giới hạn.")]
        public decimal? MinOrderAmount { get; set; }

        [Display(Name = "Max Discount Amount", Description = "Số tiền giảm tối đa (đơn vị VNĐ). Để trống nếu không giới hạn.")]
        public decimal? MaxDiscountAmount { get; set; }

        [Display(Name = "Usage Limit / User", Description = "Số lần tối đa mỗi Novice Driver được dùng voucher này. Để trống nếu không giới hạn.")]
        public int? UsageLimit { get; set; }

        [Required]
        [Display(Name = "Start Date", Description = "Ngày bắt đầu hiệu lực của voucher.")]
        public DateTime StartDate { get; set; }

        [Required]
        [Display(Name = "End Date", Description = "Ngày kết thúc hiệu lực của voucher.")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Is Active", Description = "Bật/tắt voucher ngay sau khi tạo.")]
        public bool IsActive { get; set; } = true;
    }
}

