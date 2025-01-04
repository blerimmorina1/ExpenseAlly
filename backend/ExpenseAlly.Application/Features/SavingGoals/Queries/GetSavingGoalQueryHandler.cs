using ExpenseAlly.Application.Common.Interfaces;
using ExpenseAlly.Application.Features.SavingGoals.Dtos;

namespace ExpenseAlly.Application.Features.SavingGoals.Queries;

public class GetSavingGoalQueryHandler : IRequestHandler<GetSavingGoalQuery, List<SavingGoalDto>>
{
    private readonly IApplicationDbContext _context;

    public GetSavingGoalQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<SavingGoalDto>> Handle(GetSavingGoalQuery request, CancellationToken cancellationToken)
    {
        return await _context.SavingGoals
            .AsNoTracking()
            .Select(s => new SavingGoalDto
            {
                Id = s.Id,
                Name = s.Name,
                TargetAmount = s.TargetAmount,
                CurrentAmount = s.CurrentAmount,
                Deadline = s.Deadline,
                IsCompleted = s.IsCompleted,
                Notes = s.Notes
            })
            .ToListAsync(cancellationToken);
    }
}