using ExpenseAlly.Application.Common.Models;

namespace ExpenseAlly.Application.Features.SavingGoals.Commands;

public class UpdateSavingGoalCommand : IRequest<ResponseDto>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal TargetAmount { get; set; }
    public decimal CurrentAmount { get; set; }
    public DateTime? Deadline { get; set; }
    public bool IsCompleted { get; set; }
    public string? Notes { get; set; }
}