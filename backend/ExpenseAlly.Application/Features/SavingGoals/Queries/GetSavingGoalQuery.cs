using ExpenseAlly.Application.Common.Models;
using ExpenseAlly.Application.Features.SavingGoals.Dtos;
using MediatR;

namespace ExpenseAlly.Application.Features.SavingGoals.Queries
{
    public class GetSavingGoalQuery : IRequest<List<SavingGoalDto>>
    {
    }
}