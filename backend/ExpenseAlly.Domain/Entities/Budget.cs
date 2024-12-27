using ExpenseAlly.Domain.Common;

namespace ExpenseAlly.Domain.Entities;

public class Budget : BaseAuditableEntity
{
    public string Name { get; set; }
    public decimal TotalLimit { get; set; }
    public decimal TotalSpent { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public List<BudgetDetail>? BudgetDetails { get; set; }
}
