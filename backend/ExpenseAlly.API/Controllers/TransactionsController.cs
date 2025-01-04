using ExpenseAlly.Application.Features.Transactions.Commands;
using ExpenseAlly.Application.Features.Transactions.Dtos;
using ExpenseAlly.Application.Features.Transactions.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseAlly.API.Controllers;

public class TransactionsController : ApiControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Guid>> CreateTransaction([FromBody] CreateTransactionCommand command)
    {
        var transactionId = await _mediator.Send(command);
        return CreatedAtAction(nameof(CreateTransaction), new { id = transactionId }, transactionId);
    }


    [HttpGet]
    public async Task<ActionResult<List<TransactionDto>>> GetTransactions([FromQuery] GetTransactionsQuery query)
    {
        var transactions = await _mediator.Send(query);
        return Ok(transactions);
    }
}
