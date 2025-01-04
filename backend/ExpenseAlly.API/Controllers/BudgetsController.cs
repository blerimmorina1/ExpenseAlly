using ExpenseAlly.Application.Common.Models;
using ExpenseAlly.Application.Features.Budgets.Commands;
using ExpenseAlly.Application.Features.Budgets.Dtos;
using ExpenseAlly.Application.Features.Budgets.Queries;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseAlly.API.Controllers;

public class BudgetsController : ApiControllerBase
{
    [HttpPost("createBudget")]
    public async Task<ResponseDto> CreateBudget(CreateBudgetCommand request)
    {
        return await _mediator.Send(request);
    }

    [HttpPost("editBudget")]
    public async Task<ResponseDto> EditBudget(EditBudgetCommand request)
    {
        return await _mediator.Send(request);
    }

    [HttpGet("getBudget")]
    public async Task<BudgetDto> GetBudgets(DateTime date)
    {        return await _mediator.Send(new GetBudgetQuery(date));
    }

    [HttpGet("getExpenseCategories")]
    public async Task<BudgetCategoriesDto> GetExpenseCategories(Guid? budgetId)
    {
        return await _mediator.Send(new GetExpenseCategoriesQuery());
    }

    [HttpDelete("deleteBudget")]
    public async Task<ResponseDto> DeleteBudget(Guid id)
    {
        return await _mediator.Send(new DeleteBudgetCommand(id));
    }

    [HttpPut("saveCategoryBudget")]
    public async Task<ResponseDto> SaveCategoryBudget(SaveCategoryBudgetCommand request)
    {
        return await _mediator.Send(request);
    }
}
