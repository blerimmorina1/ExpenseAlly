namespace ExpenseAlly.Application.Features.Budgets.Dtos;

public class BudgetCategoryDto
{
    public Guid CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public decimal Limit { get; set; }
}
