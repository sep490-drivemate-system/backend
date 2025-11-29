using AutoMapper;
using ResourceService.Repositories;
using ResourceService.Repositories.Models;
using ResourceService.Services.Commons.Constants;
using ResourceService.Services.DTOs.Vouchers;
using ResourceService.Services.Interfaces;
using SharedLibrary.SharedKernel.ServiceResult;
using System;
using System.Linq;
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

            // Validate usage limit (bắt buộc > 0)
            if (voucher.UsageLimit <= 0)
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
            voucherEntity.StartDate = NormalizeDate(voucher.StartDate);
            voucherEntity.EndDate = NormalizeDate(voucher.EndDate);

            await _unitOfWork.Repository<Voucher>().CreateAsync(voucherEntity);
            await _unitOfWork.SaveChangesWithTransactionAsync();

            var dto = _mapper.Map<VoucherDTO>(voucherEntity);
            return Result<VoucherDTO>.Success(dto, Messages.Voucher.CREATE_SUCCESS);
        }

        public async Task<Result<VoucherDTO>> UpdateVoucher(Guid voucherId, VoucherUpdateDTO voucher)
        {
            var voucherEntity = await _unitOfWork.Repository<Voucher>().GetByIdAsync(voucherId);

            if (voucherEntity == null || voucherEntity.IsDeleted)
            {
                return Result<VoucherDTO>.Failure(
                    ServiceError.NotFoundError(Messages.Voucher.NOT_FOUND),
                    Messages.Voucher.NOT_FOUND);
            }

            if (voucherEntity.StartDate >= voucher.EndDate)
            {
                return Result<VoucherDTO>.Failure(
                    ServiceError.BadRequestError(Messages.Voucher.INVALID_DATE_RANGE),
                    Messages.Voucher.INVALID_DATE_RANGE);
            }

            voucherEntity.Name = voucher.Name.Trim();
            voucherEntity.Description = voucher.Description?.Trim() ?? string.Empty;
            voucherEntity.EndDate = NormalizeDate(voucher.EndDate);
            voucherEntity.IsActive = voucher.IsActive;
            voucherEntity.UpdatedAt = DateTime.Now;

            await _unitOfWork.SaveChangesWithTransactionAsync();

            var dto = _mapper.Map<VoucherDTO>(voucherEntity);
            return Result<VoucherDTO>.Success(dto, Messages.Voucher.UPDATE_SUCCESS);
        }

        public async Task<Result<bool>> DeleteVoucher(Guid voucherId)
        {
            var voucherEntity = await _unitOfWork.Repository<Voucher>().GetByIdAsync(voucherId);

            if (voucherEntity == null || voucherEntity.IsDeleted)
            {
                return Result<bool>.Failure(
                    ServiceError.NotFoundError(Messages.Voucher.NOT_FOUND),
                    Messages.Voucher.NOT_FOUND);
            }

            voucherEntity.IsDeleted = true;
            voucherEntity.IsActive = false;
            voucherEntity.UpdatedAt = DateTime.Now;

            await _unitOfWork.SaveChangesWithTransactionAsync();

            return Result<bool>.Success(true, Messages.Voucher.DELETE_SUCCESS);
        }

        public async Task<Result<VoucherUseResultDTO>> UseVoucher(VoucherUseRequestDTO request, Guid userId)
        {
            var validationResult = await ValidateVoucherForUser(request.Code, request.OrderAmount, userId);
            if (!validationResult.IsSuccess || validationResult.Data is null)
            {
                return Result<VoucherUseResultDTO>.Failure(validationResult.Error!, validationResult.Message);
            }

            var validationData = validationResult.Data!;
            var voucher = validationData.Voucher;
            var discountAmount = validationData.DiscountAmount;
            var finalAmount = validationData.FinalAmount;

            var usage = new VoucherUsage
            {
                VoucherId = voucher.Id,
                UserId = userId,
                PackageId = request.PackageId,
                OrderAmount = request.OrderAmount,
                DiscountAmount = discountAmount
            };

            await _unitOfWork.Repository<VoucherUsage>().CreateAsync(usage);

            voucher.UsedCount += 1;
            voucher.UpdatedAt = DateTime.Now;

            await _unitOfWork.SaveChangesWithTransactionAsync();

            var response = _mapper.Map<VoucherUseResultDTO>(voucher);
            response.UsageId = usage.Id;
            response.DiscountAmount = discountAmount;
            response.OrderAmount = usage.OrderAmount;
            response.FinalAmount = finalAmount;

            return Result<VoucherUseResultDTO>.Success(response, Messages.Voucher.USE_SUCCESS);
        }

        public async Task<Result<VoucherUseResultDTO>> CheckVoucher(VoucherUseRequestDTO request, Guid userId)
        {
            var validationResult = await ValidateVoucherForUser(request.Code, request.OrderAmount, userId);
            if (!validationResult.IsSuccess || validationResult.Data is null)
            {
                return Result<VoucherUseResultDTO>.Failure(validationResult.Error!, validationResult.Message);
            }

            var validationData = validationResult.Data!;
            var voucher = validationData.Voucher;
            var discountAmount = validationData.DiscountAmount;
            var finalAmount = validationData.FinalAmount;

            var response = _mapper.Map<VoucherUseResultDTO>(voucher);
            response.UsageId = Guid.Empty;
            response.DiscountAmount = discountAmount;
            response.OrderAmount = request.OrderAmount;
            response.FinalAmount = finalAmount;

            return Result<VoucherUseResultDTO>.Success(response, Messages.Voucher.CHECK_SUCCESS);
        }

        private static DateTime NormalizeDate(DateTime value) =>
            value.Kind == DateTimeKind.Utc ? value.ToLocalTime() : value;

        private async Task<Result<VoucherValidationData>> ValidateVoucherForUser(string code, decimal orderAmount, Guid userId)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                return Result<VoucherValidationData>.Failure(
                    ServiceError.BadRequestError(Messages.Voucher.NOT_FOUND),
                    Messages.Voucher.NOT_FOUND);
            }

            if (orderAmount <= 0)
            {
                return Result<VoucherValidationData>.Failure(
                    ServiceError.BadRequestError(Messages.Voucher.INVALID_ORDER_AMOUNT),
                    Messages.Voucher.INVALID_ORDER_AMOUNT);
            }

            var normalizedCode = code.Trim().ToLower();

            var vouchers = await _unitOfWork.Repository<Voucher>().GetAllAsync(
                filter: x => !x.IsDeleted && x.Code.ToLower() == normalizedCode);

            var voucher = vouchers.FirstOrDefault();
            if (voucher == null)
            {
                return Result<VoucherValidationData>.Failure(
                    ServiceError.NotFoundError(Messages.Voucher.NOT_FOUND),
                    Messages.Voucher.NOT_FOUND);
            }

            if (!voucher.IsActive)
            {
                return Result<VoucherValidationData>.Failure(
                    ServiceError.BadRequestError(Messages.Voucher.NOT_ACTIVE),
                    Messages.Voucher.NOT_ACTIVE);
            }

            var now = DateTime.Now;
            if (voucher.StartDate > now)
            {
                return Result<VoucherValidationData>.Failure(
                    ServiceError.BadRequestError(Messages.Voucher.NOT_STARTED),
                    Messages.Voucher.NOT_STARTED);
            }

            if (voucher.EndDate < now)
            {
                return Result<VoucherValidationData>.Failure(
                    ServiceError.BadRequestError(Messages.Voucher.EXPIRED),
                    Messages.Voucher.EXPIRED);
            }

            if (voucher.MinOrderAmount.HasValue && orderAmount < voucher.MinOrderAmount.Value)
            {
                return Result<VoucherValidationData>.Failure(
                    ServiceError.BadRequestError(Messages.Voucher.MIN_ORDER_NOT_REACHED),
                    Messages.Voucher.MIN_ORDER_NOT_REACHED);
            }

            var userUsage = await _unitOfWork.Repository<VoucherUsage>().GetAllAsync(
                filter: x => x.VoucherId == voucher.Id && x.UserId == userId);

            if (userUsage.Count >= voucher.UsageLimit)
            {
                return Result<VoucherValidationData>.Failure(
                    ServiceError.BadRequestError(Messages.Voucher.USAGE_LIMIT_REACHED),
                    Messages.Voucher.USAGE_LIMIT_REACHED);
            }

            var discountAmount = Math.Round(orderAmount * voucher.DiscountPercentage / 100m, 2, MidpointRounding.AwayFromZero);
            if (voucher.MaxDiscountAmount.HasValue)
            {
                discountAmount = Math.Min(discountAmount, voucher.MaxDiscountAmount.Value);
            }

            if (discountAmount < 0)
            {
                discountAmount = 0;
            }

            var finalAmount = Math.Max(orderAmount - discountAmount, 0);

            var data = new VoucherValidationData
            {
                Voucher = voucher,
                DiscountAmount = discountAmount,
                FinalAmount = finalAmount
            };

            return Result<VoucherValidationData>.Success(data);
        }

        private sealed class VoucherValidationData
        {
            public Voucher Voucher { get; init; } = null!;
            public decimal DiscountAmount { get; init; }
            public decimal FinalAmount { get; init; }
        }
    }
}

