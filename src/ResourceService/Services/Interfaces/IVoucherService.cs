using ResourceService.Services.DTOs.Vouchers;
using SharedLibrary.SharedKernel.ServiceResult;

namespace ResourceService.Services.Interfaces
{
    public interface IVoucherService
    {
        Task<Result<IEnumerable<VoucherDTO>>> GetAllVouchers(VoucherFilterDTO? filter = null);
        Task<Result<VoucherDTO>> CreateVoucher(VoucherCreateDTO voucher, Guid adminId);
        Task<Result<VoucherDTO>> UpdateVoucher(Guid voucherId, VoucherUpdateDTO voucher);
        Task<Result<bool>> DeleteVoucher(Guid voucherId);
        Task<Result<VoucherUseResultDTO>> UseVoucher(VoucherUseRequestDTO request, Guid userId);
        Task<Result<VoucherUseResultDTO>> CheckVoucher(VoucherUseRequestDTO request, Guid userId);
    }
}

