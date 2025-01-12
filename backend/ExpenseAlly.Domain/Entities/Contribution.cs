using ExpenseAlly.Domain.Common;

namespace ExpenseAlly.Domain.Entities;

public class Contribution : BaseAuditableEntity
{
    public Guid SavingGoalId { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public SavingGoal SavingGoal { get; set; }
}