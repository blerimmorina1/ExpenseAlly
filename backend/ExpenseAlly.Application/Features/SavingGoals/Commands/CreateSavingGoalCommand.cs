using ExpenseAlly.Application.Common.Models;


namespace ExpenseAlly.Application.Features.SavingGoals.Commands
{
    public class CreateSavingGoalCommand : IRequest<ResponseDto>
    {
        public string Name { get; set; } = string.Empty;
        public decimal TargetAmount { get; set; }
        public decimal CurrentAmount { get; set; } = 0;
        public DateTime? Deadline { get; set; }
        public bool IsCompleted { get; set; } = false;
        public string? Notes { get; set; }
    }
}