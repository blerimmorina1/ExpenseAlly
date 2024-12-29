using ExpenseAlly.Application.Common.Models;
using ExpenseAlly.Application.Features.Budgets.Dtos;
using ExpenseAlly.Application.Features.Transactions.Commands;

namespace ExpenseAlly.Application.Features.Budgets.Commands;

public class CreateBudgetCommand : IRequest<ResponseDto>
{
    public string Name { get; set; }
    public decimal TotalLimit { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public List<BudgetDetailDto> BudgetDetails { get; set; }
}

public class CreateBudgetCommandHandler : IRequestHandler<CreateBudgetCommand, ResponseDto>
{
    public async Task<ResponseDto> Handle(CreateBudgetCommand request, CancellationToken cancellationToken)
    {
        return new ResponseDto
        {

        };
    }
}