using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentService.Application.Common.DTOs;
using PaymentService.Application.Features.Transactions.Commands.CreateTransaction;
using PaymentService.Application.Features.Transactions.Commands.UpdateTransactionStatus;
using PaymentService.Application.Features.Transactions.Queries.GetDashboardStatistic;
using PaymentService.Application.Features.Transactions.Queries.GetInstructorDashboardStatistic;
using PaymentService.Application.Features.Transactions.Queries.GetTransactionById;
using PaymentService.Application.Features.Transactions.Queries.GetTransactions;
using PaymentService.Application.Features.Transactions.Queries.GetTransactionsByBookingId;
using PaymentService.Application.Features.Transactions.Queries.GetUserTransactions;
using SharedLibrary.Jwt;
using SharedLibrary.SharedKernel.ServiceResult;

namespace PaymentService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IJwtService _jwtService;
        public TransactionController(IMediator mediator,IJwtService jwtService)
        {
            _mediator = mediator;
            _jwtService = jwtService;
        }

        [HttpGet("statistic")]
        public async Task<IActionResult> GetPaymentStatistics(PaymentStatisticFilterDTO filter)
        {
            var result = await _mediator.Send(new GetDashboardStatisticQuery { Type = filter.Type, Year = filter.Year, Month = filter.Month, Week = filter.Week});
            return result.ToActionResult();
        }

        [HttpGet("users/{id}/statistic")]
        public async Task<IActionResult> GetUserSpecificStatistics([FromRoute] Guid id)
        {
            var result = await _mediator.Send(new GetInstructorStatisticQuery { UserId = id });
            return result.ToActionResult();
        }

        [HttpGet]
        public async Task<IActionResult> GetTransactionById(Guid id)
        {
            var query = new GetTransactionByIdQuery(id);
            var result = await _mediator.Send(query);

            if (result.IsSuccess)
                return Ok(result);

            return NotFound(result);
        }

        [HttpGet("user")]
        public async Task<IActionResult> GetTransactions([FromQuery] TransactionFilter filter, Guid? id = null)
        {
            Guid userId = id != null ? (Guid) id : await _jwtService.ExtractUserIdFromToken(Request.Headers["Authorization"].ToString());
            var query = new GetUserTransactionQuery
            {
                UserId = userId,
                Filter = filter
            };
            var result = await _mediator.Send(query);
            return result.ToActionResult();
        }

        [HttpGet("booking/{bookingId}")]
        public async Task<IActionResult> GetTransactionsByBookingId(Guid bookingId)
        {
            var query = new GetTransactionsByBookingIdQuery(bookingId);
            var result = await _mediator.Send(query);
            
            if (result.IsSuccess)
                return Ok(result);
            
            return BadRequest(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTransaction([FromBody] CreateTransactionCommand command)
        {
            var result = await _mediator.Send(command);
            
            if (result.IsSuccess)
                return CreatedAtAction(nameof(GetTransactionById), new { id = result.Data.Id }, result);
            
            return BadRequest(result);
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateTransactionStatus(Guid id, [FromBody] UpdateTransactionStatusCommand command)
        {
            command.TransactionId = id;
            var result = await _mediator.Send(command);
            
            if (result.IsSuccess)
                return Ok(result);
            
            return BadRequest(result);
        }
    }
}
