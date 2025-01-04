using ExpenseAlly.Application.Common.Models;

namespace ExpenseAlly.Application.Features.Budgets.Dtos;

public class BudgetDto: ResponseDto
{
    public Guid? Id { get; set; }
    public string Name { get; set; }
    public decimal TotalLimit { get; set; }
    public decimal TotalSpent { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public List<BudgetDetailDto>? BudgetDetails { get; set; }
}
