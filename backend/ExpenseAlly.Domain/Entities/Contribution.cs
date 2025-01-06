namespace ExpenseAlly.Domain.Entities;

public class Contribution
{
    public Guid Id { get; set; }
    public Guid SavingGoalId { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public SavingGoal SavingGoal { get; set; }
}