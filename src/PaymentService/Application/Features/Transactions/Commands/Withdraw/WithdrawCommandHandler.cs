using MediatR;
using Microsoft.Extensions.Configuration;
using PaymentService.Application.Features.Transactions.Commands.WithdrawRequest;
using PaymentService.Application.Interfaces;
using PaymentService.Domain.Enum;
using SharedLibrary.Email;
using SharedLibrary.Payment.PayOs;
using SharedLibrary.Payment.PayOs.DTOs;
using SharedLibrary.Payment.VnPay;
using SharedLibrary.Payment.ZaloPay;
using SharedLibrary.SharedKernel.Enum;
using SharedLibrary.SharedKernel.ServiceResult;
using System;
using System.Collections.Generic;
using Net.payOS.Types;

namespace PaymentService.Application.Features.Transactions.Commands.Withdraw
{
    public class WithdrawCommandHandler : IRequestHandler<WithdrawCommand, Result<string?>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly IPayOSService _payOsService;
        private readonly IVNPayService _vnPayService;
        private readonly IZaloPayService _zaloPayService;

        public WithdrawCommandHandler(
            IUnitOfWork unitOfWork,
            IEmailService emailService,
            IConfiguration configuration,
            IPayOSService payOsService,
            IVNPayService vnPayService,
            IZaloPayService zaloPayService)
        {
            _unitOfWork = unitOfWork;
            _emailService = emailService;
            _configuration = configuration;
            _payOsService = payOsService;
            _vnPayService = vnPayService;
            _zaloPayService = zaloPayService;
        }

        public async Task<Result<string?>> Handle(WithdrawCommand request, CancellationToken cancellationToken)
        {
            var transaction = await _unitOfWork.TransactionRepository.GetByIdAsync(request.TransactionId);

            if (request.Status == WithdrawStatus.Approved)
            {
                var callbackUrl = _configuration["PAYMENTCALLBACK_WEBAPP:URL"];
                
                string? paymentUrl = null;
                string? referenceCode = null;

                switch (request.PaymentMethod)
                {
                    case PaymentMethod.PayOs:
                        var (payOsResult, payOsRefCode) = await HandlePayOsPayment(transaction.TransactionValue, transaction.FromWalletId ?? Guid.Empty, callbackUrl);
                        if (!payOsResult.IsSuccess)
                        {
                            return Result<string?>.Failure(payOsResult.Error);
                        }
                        paymentUrl = payOsResult.Data;
                        referenceCode = payOsRefCode;
                        break;

                    case PaymentMethod.VnPay:
                        var (vnPayResult, vnPayRefCode) = await HandleVnPayPayment(transaction.TransactionValue, callbackUrl);
                        if (!vnPayResult.IsSuccess)
                        {
                            return Result<string?>.Failure(vnPayResult.Error);
                        }
                        paymentUrl = vnPayResult.Data;
                        referenceCode = vnPayRefCode;
                        break;

                    case PaymentMethod.ZaloPay:
                        var (zaloPayResult, zaloPayRefCode) = await HandleZaloPayPayment(transaction.TransactionValue, callbackUrl);
                        if (!zaloPayResult.IsSuccess)
                        {
                            return Result<string?>.Failure(zaloPayResult.Error);
                        }
                        paymentUrl = zaloPayResult.Data;
                        referenceCode = zaloPayRefCode;
                        break;

                    default:
                        return Result<string?>.Failure(
                            ServiceError.BadRequestError("Unsupported payment method"));
                }

                transaction.PaymentMethod = request.PaymentMethod;
                if (!string.IsNullOrEmpty(referenceCode))
                {
                    transaction.ReferenceCode = referenceCode;
                }
                transaction.UpdatedAt = DateTime.Now;

                await _unitOfWork.TransactionRepository.UpdateAsync(transaction);
                await _unitOfWork.SaveChangesAsync();

                return Result<string?>.Success(paymentUrl);
            }
            else if (request.Status == WithdrawStatus.Rejected)
            {
                transaction.Status = PaymentStatus.Failed;
                transaction.UpdatedAt = DateTime.Now;

                await _unitOfWork.TransactionRepository.UpdateAsync(transaction);
                await _unitOfWork.SaveChangesAsync();

                var emailData = new Dictionary<string, string>
                {
                    { "FullName", request.FullName },
                    { "Amount", transaction.TransactionValue.ToString("N0") },
                    { "TransactionNote", transaction.TransactionNote ?? "Không có ghi chú" },
                    { "Reason", request.Reason ?? "Yêu cầu rút tiền không đáp ứng các điều kiện của hệ thống." }
                };

                await _emailService.SendingEmail(
                    request.Email,
                    emailData,
                    "Thông báo: Yêu cầu rút tiền đã bị từ chối",
                    EmailType.WithdrawRejected
                );

                return Result<string?>.Success(null);
            }

            return Result<string?>.Failure(
                ServiceError.BadRequestError("Invalid withdraw status"));
        }

        private async Task<(Result<string> Result, string? ReferenceCode)> HandlePayOsPayment(decimal amount, Guid userId, string callbackUrl)
        {
            try
            {
                var paymentDto = new PayOSPaymentDTO
                {
                    UnitPrice = (int)amount,
                    Items = new List<ItemData>
                    {
                        new ItemData(
                            name: "Withdrawal request",
                            quantity: 1,
                            price: (int)amount
                        )
                    },
                };
                var (paymentUrl, referenceCode) = await _payOsService.CreatePayOSLink(paymentDto, callbackUrl);
                return (Result<string>.Success(paymentUrl), referenceCode?.ToString());
            }
            catch (Exception ex)
            {
                return (Result<string>.Failure(
                    ServiceError.ServiceUnavailableError($"Error creating PayOS payment: {ex.Message}")), null);
            }
        }

        private async Task<(Result<string> Result, string? ReferenceCode)> HandleVnPayPayment(decimal amount, string callbackUrl)
        {
            try
            {
                var (paymentUrl, referenceCode) = await _vnPayService.CreateVNPayOrder(amount, callbackUrl);
                return (Result<string>.Success(paymentUrl), referenceCode.ToString());
            }
            catch (Exception ex)
            {
                return (Result<string>.Failure(
                    ServiceError.ServiceUnavailableError($"Error creating VNPay payment: {ex.Message}")), null);
            }
        }

        private async Task<(Result<string> Result, string? ReferenceCode)> HandleZaloPayPayment(decimal amount, string callbackUrl)
        {
            try
            {
                var (paymentUrl, referenceCode) = await _zaloPayService.CreateZaloPayOrder(amount, callbackUrl);
                return (Result<string>.Success(paymentUrl), referenceCode?.ToString());
            }
            catch (Exception ex)
            {
                return (Result<string>.Failure(
                    ServiceError.ServiceUnavailableError($"Error creating ZaloPay payment: {ex.Message}")), null);
            }
        }
    }
}
