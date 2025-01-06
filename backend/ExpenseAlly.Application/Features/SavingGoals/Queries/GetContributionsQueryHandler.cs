using ExpenseAlly.Application.Features.SavingGoals.Dtos;
using ExpenseAlly.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ExpenseAlly.Application.Features.SavingGoals.Queries
{
    public class GetContributionsQueryHandler : IRequestHandler<GetContributionsQuery, List<ContributionDto>>
    {
        private readonly IApplicationDbContext _context;

        public GetContributionsQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ContributionDto>> Handle(GetContributionsQuery request, CancellationToken cancellationToken)
        {
            // Query contributions with related saving goals
            var contributionsQuery = _context.Contributions
                .Include(c => c.SavingGoal) // Include SavingGoal relationship
                .AsQueryable();

            // Filter by SavingGoalId if provided
            if (request.SavingGoalId.HasValue)
            {
                contributionsQuery = contributionsQuery.Where(c => c.SavingGoalId == request.SavingGoalId.Value);
            }


            if (request.Year.HasValue)
            {
                contributionsQuery = contributionsQuery.Where(c => c.Date.Year == request.Year.Value);
            }

            if (request.Month.HasValue)
            {
                contributionsQuery = contributionsQuery.Where(c => c.Date.Month == request.Month.Value);
            }

            if (request.Day.HasValue)
            {
                contributionsQuery = contributionsQuery.Where(c => c.Date.Day == request.Day.Value);
            }

            var contributions = await contributionsQuery
                .OrderBy(c => c.Date)
                .Select(c => new ContributionDto
                {
                    Id = c.Id,
                    Amount = c.Amount,
                    Date = c.Date,
                    SavingGoalId = c.SavingGoal.Id,
                    SavingGoalName = c.SavingGoal.Name
                })
                .ToListAsync(cancellationToken);

            return contributions;
        }
    }
}
