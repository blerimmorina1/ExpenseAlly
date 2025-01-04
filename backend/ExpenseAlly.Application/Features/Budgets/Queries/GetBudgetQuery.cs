using ExpenseAlly.Application.Common.Interfaces;
using ExpenseAlly.Application.Features.Budgets.Dtos;
using Microsoft.Extensions.Logging;

namespace ExpenseAlly.Application.Features.Budgets.Queries;

public class GetBudgetQuery : IRequest<BudgetDto>
{
    public GetBudgetQuery(DateTime date)
    {
        Date = date;
    }

    public DateTime Date { get; set; }
}

public class GetBudgetQueryHandler : IRequestHandler<GetBudgetQuery, BudgetDto>
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<GetBudgetQueryHandler> _logger;

    public GetBudgetQueryHandler(IApplicationDbContext context, ILogger<GetBudgetQueryHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<BudgetDto> Handle(GetBudgetQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var response = new BudgetDto();

            var budget = await _context.Budgets.Where(x => x.StartDate.Month == request.Date.Month && x.StartDate.Year == request.Date.Year).Include(x => x.BudgetDetails).ThenInclude(x => x.Category).FirstOrDefaultAsync();

            if (budget != null)
            {
                response.Success = true;
                response.Id = budget.Id;
                response.Name = budget.Name;
                response.TotalSpent = budget.TotalSpent;
                response.TotalSpent = budget.TotalSpent;
                response.TotalLimit = budget.TotalLimit;
                response.BudgetDetails = budget.BudgetDetails?.Select(detail => new BudgetDetailDto
                {
                    Id = detail.Id,
                    CategoryName = detail.Category.Name,
                    CategoryId = detail.CategoryId,
                    Limit = detail.Limit,
                    Spent = detail.Spent,
                }).ToList(); 
            }

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred fetching budget.");
            throw;
        }
    }
}