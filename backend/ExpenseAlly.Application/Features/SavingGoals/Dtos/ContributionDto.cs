namespace ExpenseAlly.Application.Features.SavingGoals.Dtos;

public class ContributionDto
{
    public Guid Id { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public Guid SavingGoalId { get; set; }
    public string SavingGoalName { get; set; }
}