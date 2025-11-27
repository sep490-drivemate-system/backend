using ResourceService.Services.DTOs.Vouchers;
using SharedLibrary.SharedKernel.ServiceResult;

namespace ResourceService.Services.Interfaces
{
    public interface IVoucherService
    {
        Task<Result<IEnumerable<VoucherDTO>>> GetAllVouchers(VoucherFilterDTO? filter = null);
        Task<Result<VoucherDTO>> CreateVoucher(VoucherCreateDTO voucher, Guid adminId);
    }
}

