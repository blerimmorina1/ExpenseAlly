using ExpenseAlly.Application.Common.Models;
using ExpenseAlly.Application.Features.Account.Commands;
using ExpenseAlly.Application.Features.Budgets.Commands;
using ExpenseAlly.Application.Features.Budgets.Dtos;
using ExpenseAlly.Application.Features.Budgets.Queries;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseAlly.API.Controllers;

public class BudgetController : ApiControllerBase
{
    [HttpPost("createBudget")]
    public async Task<ResponseDto> CreateBudget(CreateBudgetCommand request)
    {
        return await _mediator.Send(request);
    }

    [HttpGet("getBudget")]
    public async Task<BudgetDto> GetBudgets()
    {        return await _mediator.Send(new GetBudgetQuery());
    }
}
