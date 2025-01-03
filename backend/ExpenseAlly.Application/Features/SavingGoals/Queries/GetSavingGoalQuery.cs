using ExpenseAlly.Application.Common.Models;
using MediatR;

namespace ExpenseAlly.Application.Features.SavingGoals.Queries
{
    public class GetSavingGoalQuery : IRequest<ResponseDto>
    {
        public Guid Id { get; set; }

        public GetSavingGoalQuery(Guid id)
        {
            Id = id;
        }
    }
}