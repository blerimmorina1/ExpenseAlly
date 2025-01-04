using ExpenseAlly.Application.Common.Interfaces;
using ExpenseAlly.Application.Features.Budgets.Dtos;
using ExpenseAlly.Application.Features.TransactionCategories.Dtos;
using ExpenseAlly.Application.Features.TransactionCategories.Queries;

namespace ExpenseAlly.Application.Features.Budgets.Queries;

public class GetExpenseCategoriesQuery : IRequest<BudgetCategoriesDto>
{
}

public class GetExpenseCategoriesQueryHandler : IRequestHandler<GetExpenseCategoriesQuery, BudgetCategoriesDto>
{
    private readonly IApplicationDbContext _context;

    public GetExpenseCategoriesQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<BudgetCategoriesDto> Handle(GetExpenseCategoriesQuery request, CancellationToken cancellationToken)
    {
        var response = new BudgetCategoriesDto();

        var categories = await _context.TransactionCategories.Where(x => x.Type == Domain.Enums.TransactionType.Expense).ToListAsync(cancellationToken);

        if (categories != null && categories.Count() > 0)
        {
            response.Success = true;
            response.Categories = categories.Select(c => new BudgetCategoryDto
            {
                CategoryId = c.Id,
                CategoryName = c.Name, 
                Limit = 0
            }).ToList();
        }

        return response; 
    }
}
