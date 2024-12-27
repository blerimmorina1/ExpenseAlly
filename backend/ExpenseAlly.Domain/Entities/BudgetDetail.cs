using ExpenseAlly.Domain.Common;

namespace ExpenseAlly.Domain.Entities;

public class BudgetDetail : BaseAuditableEntity
{
    public Guid BudgetId {  get; set; }
    public Guid CategoryId {  get; set; }
    public decimal Limit {  get; set; }
    public decimal Spent {  get; set; }
    public TransactionCategory Category { get; set; }
    public Budget Budget { get; set; }
}
