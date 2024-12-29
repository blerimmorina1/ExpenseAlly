using ExpenseAlly.Application.Common.Models;
using ExpenseAlly.Application.Features.Budgets.Commands;
using ExpenseAlly.Application.Features.Budgets.Dtos;

namespace ExpenseAlly.Application.Features.Budgets.Queries;

public class GetBudgetQuery : IRequest<BudgetDto>
{
    public DateTime Date { get; set; }
}

public class GetBudgetQueryHandler : IRequestHandler<GetBudgetQuery, BudgetDto>
{
    public async Task<BudgetDto> Handle(GetBudgetQuery request, CancellationToken cancellationToken)
    {
        return new BudgetDto
        {

        };
    }
}