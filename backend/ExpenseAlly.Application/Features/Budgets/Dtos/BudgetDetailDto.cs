using ExpenseAlly.Domain.Entities;

namespace ExpenseAlly.Application.Features.Budgets.Dtos;

public class BudgetDetailDto
{
    public Guid CategoryId { get; set; }
    public decimal Limit { get; set; }
}
