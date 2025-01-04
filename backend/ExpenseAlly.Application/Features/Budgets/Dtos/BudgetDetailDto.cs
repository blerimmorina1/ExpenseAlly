using ExpenseAlly.Domain.Entities;

namespace ExpenseAlly.Application.Features.Budgets.Dtos;

public class BudgetDetailDto
{
    public Guid? Id { get; set; }
    public Guid CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public decimal Limit { get; set; }
    public decimal Spent { get; set; }
}
