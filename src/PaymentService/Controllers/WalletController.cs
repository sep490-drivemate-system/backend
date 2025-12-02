using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentService.Application.Features.Wallet.Commands.UpdateWalletBalance;
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

        public WalletController(IMediator mediator, IJwtService jwtService)
        {
            _mediator = mediator;
            _jwtService = jwtService;
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
