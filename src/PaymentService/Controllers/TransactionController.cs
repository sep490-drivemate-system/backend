using MediatR;
using Microsoft.AspNetCore.Mvc;
using PaymentService.Application.Features.Transactions.Commands.CreateTransaction;
using PaymentService.Application.Features.Transactions.Commands.UpdateTransactionStatus;
using PaymentService.Application.Features.Transactions.Queries.GetTransactionById;
using PaymentService.Application.Features.Transactions.Queries.GetTransactionsByBookingId;

namespace PaymentService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TransactionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTransactionById(Guid id)
        {
            var query = new GetTransactionByIdQuery(id);
            var result = await _mediator.Send(query);
            
            if (result.IsSuccess)
                return Ok(result);
            
            return NotFound(result);
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
