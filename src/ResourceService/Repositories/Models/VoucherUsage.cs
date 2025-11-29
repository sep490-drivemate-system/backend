using SharedLibrary.SharedKernel.Entities;

namespace ResourceService.Repositories.Models
{
    public class VoucherUsage : BaseEntites
    {
        public Guid VoucherId { get; set; }
        public Guid UserId { get; set; } // Novice Driver sử dụng voucher
        public Guid PackageId { get; set; } // Package đã sử dụng voucher
        public decimal DiscountAmount { get; set; } // Số tiền đã giảm
        public decimal OrderAmount { get; set; } // Tổng giá trị đơn hàng trước khi giảm
        
        // Relationships
        public Voucher Voucher { get; set; } = null!;
    }
}

