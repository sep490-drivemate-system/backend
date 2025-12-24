using CloudinaryDotNet;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PaymentService.Application.Features.Transactions.Commands.Withdraw;
using PaymentService.Application.Features.Transactions.Commands.WithdrawRequest;
using PaymentService.Application.Features.Wallet.Commands.Deposit;
using PaymentService.Application.Features.Wallet.Commands.UpdateWalletBalance;
using PaymentService.Application.Features.Wallet.Queries.GetRequestDeposit;
using PaymentService.Application.Features.Wallet.Queries.GetSystemWalletInfo;
using PaymentService.Application.Features.Wallet.Queries.GetUserWalletWithId;
using PaymentService.Application.Features.Wallet.Queries.GetWallet;
using PaymentService.Application.Features.Wallet.Queries.IsEnoughPayment;
using PaymentService.Application.Features.Wallet.Queries.IsEnoughSessionPayment;
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

        [HttpPost("payment-booking")]
        public async Task<IActionResult> CheckWallet([FromBody] PaymentRequest paymentRequest)
        {
            var query = new PaymentBookingQuery
            {
                UserId = paymentRequest.UserId,
                Amount = paymentRequest.Amount,
                BookingId = paymentRequest.BookingId,
                DrivingSessionId = paymentRequest.DrivingSessionId
            };

            var result = await _mediator.Send(query);

            return Ok(result);
        }
        [HttpPost("payment-session")]
        public async Task<IActionResult> CheckWalletSession([FromBody] PaymentRequest paymentRequest)
        {
            var query = new PaymentSessionQuery
            {
                UserId = paymentRequest.UserId,   
                InstructorId = paymentRequest.InstructorId,
                Amount = paymentRequest.Amount,
                BookingId = paymentRequest.BookingId,
                DrivingSessionId = paymentRequest.DrivingSessionId
            };

            var result = await _mediator.Send(query);

            return Ok(result);
        }
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

        [HttpPost("withdraw")]
        public async Task<IActionResult> Withdraw(WithdrawCommand withdrawCommand)
        {
            var command = new WithdrawCommand
            {
                Reason = withdrawCommand.Reason,
                Email = withdrawCommand.Email,
                FullName = withdrawCommand.FullName,
                Status = withdrawCommand.Status,
                TransactionId = withdrawCommand.TransactionId,
                PaymentMethod = withdrawCommand.PaymentMethod,
            };
            var result = await _mediator.Send(command);

            return result.ToActionResult();
        }
        [HttpPost("withdraw-request")]
        public async Task<IActionResult> WithdrawRequest(WithdrawRequestCommand withdrawRequestCommand)
        {
            var userId = await _jwtService.ExtractUserIdFromToken(Request.Headers["Authorization"].ToString());
            var depositCommand = new WithdrawRequestCommand
            {
                Amount = withdrawRequestCommand.Amount,
                TransactionNote = withdrawRequestCommand.TransactionNote,
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
