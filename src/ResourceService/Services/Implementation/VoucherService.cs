using AutoMapper;
using ResourceService.Repositories;
using ResourceService.Repositories.Models;
using ResourceService.Services.Commons.Constants;
using ResourceService.Services.DTOs.Vouchers;
using ResourceService.Services.Interfaces;
using SharedLibrary.SharedKernel.ServiceResult;
using System;
using System.Linq.Expressions;
using System.Security.Cryptography;

namespace ResourceService.Services.Implementation
{
    public class VoucherService(IUnitOfWork unitOfWork, IMapper mapper) : IVoucherService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<Result<IEnumerable<VoucherDTO>>> GetAllVouchers(VoucherFilterDTO? filter = null)
        {
            Expression<Func<Voucher, bool>> filterExpression = x => !x.IsDeleted
                && (filter == null || !filter.IsActive.HasValue || x.IsActive == filter.IsActive.Value)
                && (filter == null || !filter.StartDate.HasValue || (x.StartDate <= filter.StartDate.Value && x.EndDate >= filter.StartDate.Value))
                && (filter == null || !filter.EndDate.HasValue || (x.StartDate <= filter.EndDate.Value && x.EndDate >= filter.EndDate.Value));

            var vouchers = await _unitOfWork.Repository<Voucher>().GetAllAsync(
                filter: filterExpression,
                orderBy: q => q.OrderByDescending(x => x.CreatedAt)
            );

            var result = vouchers.Select(x => _mapper.Map<VoucherDTO>(x));

            return Result<IEnumerable<VoucherDTO>>.Success(result);
        }

        public async Task<Result<VoucherDTO>> CreateVoucher(VoucherCreateDTO voucher, Guid adminId)
        {
            // Validate discount percentage
            if (voucher.DiscountPercentage <= 0 || voucher.DiscountPercentage > 100)
            {
                return Result<VoucherDTO>.Failure(
                    ServiceError.BadRequestError(Messages.Voucher.INVALID_DISCOUNT_PERCENTAGE),
                    Messages.Voucher.INVALID_DISCOUNT_PERCENTAGE);
            }

            // Validate date range
            if (voucher.StartDate >= voucher.EndDate)
            {
                return Result<VoucherDTO>.Failure(
                    ServiceError.BadRequestError(Messages.Voucher.INVALID_DATE_RANGE),
                    Messages.Voucher.INVALID_DATE_RANGE);
            }

            // Validate min order amount
            if (voucher.MinOrderAmount.HasValue && voucher.MinOrderAmount.Value < 0)
            {
                return Result<VoucherDTO>.Failure(
                    ServiceError.BadRequestError(Messages.Voucher.INVALID_MIN_ORDER_AMOUNT),
                    Messages.Voucher.INVALID_MIN_ORDER_AMOUNT);
            }

            // Validate max discount amount
            if (voucher.MaxDiscountAmount.HasValue && voucher.MaxDiscountAmount.Value < 0)
            {
                return Result<VoucherDTO>.Failure(
                    ServiceError.BadRequestError(Messages.Voucher.INVALID_MAX_DISCOUNT_AMOUNT),
                    Messages.Voucher.INVALID_MAX_DISCOUNT_AMOUNT);
            }

            // Validate usage limit
            if (voucher.UsageLimit.HasValue && voucher.UsageLimit.Value <= 0)
            {
                return Result<VoucherDTO>.Failure(
                    ServiceError.BadRequestError(Messages.Voucher.INVALID_USAGE_LIMIT),
                    Messages.Voucher.INVALID_USAGE_LIMIT);
            }

            // Generate unique voucher code (8 chữ số)
            string generatedCode;
            List<Voucher> existingVouchers;
            do
            {
                var number = RandomNumberGenerator.GetInt32(0, 100_000_000); // 0 -> 99_999_999
                generatedCode = number.ToString("D8"); // pad trái đủ 8 chữ số
                existingVouchers = await _unitOfWork.Repository<Voucher>().GetAllAsync(
                    filter: x => x.Code.ToLower() == generatedCode.ToLower() && !x.IsDeleted
                );
            } while (existingVouchers.Count > 0);

            // Create voucher entity using AutoMapper
            var voucherEntity = _mapper.Map<Voucher>(voucher);
            voucherEntity.Code = generatedCode;
            voucherEntity.UsedCount = 0;
            voucherEntity.CreatedBy = adminId;
            voucherEntity.IsDeleted = false;
            voucherEntity.StartDate = voucher.StartDate.Kind == DateTimeKind.Utc
                ? voucher.StartDate.ToLocalTime()
                : voucher.StartDate;
            voucherEntity.EndDate = voucher.EndDate.Kind == DateTimeKind.Utc
                ? voucher.EndDate.ToLocalTime()
                : voucher.EndDate;

            await _unitOfWork.Repository<Voucher>().CreateAsync(voucherEntity);
            await _unitOfWork.SaveChangesWithTransactionAsync();

            var dto = _mapper.Map<VoucherDTO>(voucherEntity);
            return Result<VoucherDTO>.Success(dto, Messages.Voucher.CREATE_SUCCESS);
        }
    }
}

