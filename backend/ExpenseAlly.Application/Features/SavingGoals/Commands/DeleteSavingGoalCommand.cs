using ExpenseAlly.Application.Common.Models;

namespace ExpenseAlly.Application.Features.SavingGoals.Commands;

public class DeleteSavingGoalCommand : IRequest<ResponseDto>
{
    public Guid Id { get; set; }
}