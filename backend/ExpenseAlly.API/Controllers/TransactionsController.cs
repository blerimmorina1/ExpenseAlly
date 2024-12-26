using ExpenseAlly.Application.Features.Transactions.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Mvc;

namespace ExpenseAlly.API.Controllers
{
    [AllowAnonymous]
    public class TransactionsController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public TransactionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> CreateTransaction([FromBody] CreateTransactionCommand command)
        {
            var transactionId = await _mediator.Send(command);
            return CreatedAtAction(nameof(CreateTransaction), new { id = transactionId }, transactionId);
        }
    }
}

