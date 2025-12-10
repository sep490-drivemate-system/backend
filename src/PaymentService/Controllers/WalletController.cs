using CloudinaryDotNet;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PaymentService.Application.Features.Wallet.Commands.Deposit;
using PaymentService.Application.Features.Wallet.Commands.UpdateWalletBalance;
using PaymentService.Application.Features.Wallet.Queries.GetRequestDeposit;
using PaymentService.Application.Features.Wallet.Queries.GetSystemWalletInfo;
using PaymentService.Application.Features.Wallet.Queries.GetUserWalletWithId;
using PaymentService.Application.Features.Wallet.Queries.GetWallet;
using PaymentService.Application.Features.Wallet.Queries.IsEnoughPayment;
using SharedLibrary.Jwt;
using SharedLibrary.SharedKernel.Enum;
using SharedLibrary.SharedKernel.Http.DTOs.Payment;
using SharedLibrary.SharedKernel.Http.DTOs.Wallet;
using SharedLibrary.SharedKernel.ServiceResult;


namespace PaymentService.Controllers
{
    [ApiController]
    [Route("api/wallet")]
    public class WalletController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IJwtService _jwtService;
        private readonly ILogger _logger;

        public WalletController(IMediator mediator, IJwtService jwtService, ILogger<WalletController> logger)
        {
            _mediator = mediator;
            _jwtService = jwtService;
            _logger = logger;
        }

        [HttpPost("check-payment")]
        public async Task<IActionResult> CheckWallet([FromBody] PaymentRequest paymentRequest)
        {
            var query = new IsEnoughPaymentQuery
            {
                UserId = paymentRequest.UserId,
                Amount = paymentRequest.Amount,
                BookingId = paymentRequest.BookingId,
                DrivingSessionId = paymentRequest.DrivingSessionId
            };

            var result = await _mediator.Send(query);

            return Ok(result);
        }
        //[HttpPost("check-payment-session")]
        //public async Task<IActionResult> CheckWalletSession([FromBody] PaymentRequest paymentRequest)
        //{
        //    var query = new IsEnoughPaymentSessionQuery
        //    {
        //        UserId = paymentRequest.UserId,
        //        Amount = paymentRequest.Amount,
        //        BookingId = paymentRequest.BookingId,
        //        DrivingSessionId = paymentRequest.DrivingSessionId
        //    };

        //    var result = await _mediator.Send(query);

        //    return Ok(result);
        //}
        [HttpPost("deposit")]
        public async Task<IActionResult> Deposit(GetRequestDepositQuery getRequestDepositQuery)
        {        

            var userId = await _jwtService.ExtractUserIdFromToken(Request.Headers["Authorization"].ToString());
            var depositCommand = new GetRequestDepositQuery
            {
                Amount = getRequestDepositQuery.Amount,
                PaymentMethod = getRequestDepositQuery.PaymentMethod,
                UserId = userId
            };
            var result = await _mediator.Send(depositCommand);

            return result.ToActionResult();
        }


        [HttpGet("payment-callback")]
        public async Task<IActionResult> PaymentCallback()
        {
            var data = Request.Query;
            
            var depositCommand = new DepositCommand
            {
                Data = data
            };
            var result = await _mediator.Send(depositCommand);
            return result.ToActionResult();
        }

        [HttpGet("system")]
        public async Task<IActionResult> GetSystemWallet()
        {
            var result = await _mediator.Send(new GetSystemWalletQuery());
            return result.ToActionResult(_logger);
        }

        //public async Task<IActionResult> PaymentCallback()
        //{
        //    var data = Request.Query;
        //    var user = await _userManager.GetUserAsync(User);
        //    double amount =
        //        TempData["Amount"] is string amountString
        //        && double.TryParse(amountString, out var parsedAmount)
        //            ? parsedAmount
        //            : 0;

        //    try
        //    {
        //        var CallBackPayment = await _paymentService.CallbackPayment(data, user, amount);
        //        var instructorPayout = await _paymentService.GetTransferInfo(
        //            _paymentService.GetIdTransaction().Result
        //        );
        //        if (CallBackPayment)
        //        {
        //            var instructor = await _mailService.GetInstructorByIdPayoutAsync(
        //                _paymentService.GetIdTransaction().Result
        //            );
        //            if (instructor != null)
        //            {
        //                var transferInfo = instructorPayout;

        //                _mailService.QueueBillEmail(
        //                    transferInfo,
        //                    "Deposit Invoice",
        //                    "Amazing",
        //                    instructor.Email
        //                );
        //            }
        //        }
        //        if (CallBackPayment)
        //        {
        //            TempData["Message"] = NotificationAlert.GetNofity(
        //                "Deposit successful",
        //                "NOTIFICATION",
        //                Core.Enums.AlertType.success
        //            );
        //            return View("PaymentSuccess");
        //        }
        //        else
        //        {
        //            TempData["Message"] = NotificationAlert.GetNofity(
        //                "Deposit failed",
        //                "NOTIFICATION",
        //                Core.Enums.AlertType.error
        //            );
        //            return View("PaymentFailure");
        //        }
        //    } //...
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Lỗi trong thanh toán : {ex.Message}");
        //        TempData["Message"] = NotificationAlert.GetNofity(
        //            "Deposit failed",
        //            "NOTIFICATION",
        //            Core.Enums.AlertType.error
        //        );
        //        return View("PaymentFailure");
        //    }
        //}

        [HttpPost("balance")]
        public async Task<IActionResult> AddBalanceToWallet([FromBody] WalletBalanceDTO wallet_balance)
        {
            var result = await _mediator.Send(new UpdateWalletBalanceCommand
            {
                UserId= wallet_balance.UserId,
                BalanceAmount = wallet_balance.Balance
            });

            return result.ToActionResult();
        }


        [HttpGet]
        [Authorize(Roles = $"{nameof(UserRole.NoviceDriver)},{nameof(UserRole.Admin)},{nameof(UserRole.Instructor)}")]
        public async Task<IActionResult> GetWallet()
        {
            var token = Request.Headers["Authorization"].ToString();
            var userId = await _jwtService.ExtractUserIdFromToken(token);
            var result = await _mediator.Send(new GetPaymentQuery
            {
                WalletId = userId
            });

            return result.ToActionResult();
        }

        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetWalletForUserId([FromRoute] Guid id)
        {
            var result = await _mediator.Send(new GetUserWalletWithIdQuery { UserId = id });
            return result.ToActionResult();
        }
    }
}
