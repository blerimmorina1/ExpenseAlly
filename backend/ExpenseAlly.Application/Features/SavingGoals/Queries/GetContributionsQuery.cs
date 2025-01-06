using MediatR;
using System;
using ExpenseAlly.Application.Features.SavingGoals.Dtos;

namespace ExpenseAlly.Application.Features.SavingGoals.Queries
{
    public class GetContributionsQuery : IRequest<List<ContributionDto>>
    {
        public Guid? SavingGoalId { get; set; }
        public int? Year { get; set; }
        public int? Month { get; set; }
        public int? Day { get; set; }
    }
}
