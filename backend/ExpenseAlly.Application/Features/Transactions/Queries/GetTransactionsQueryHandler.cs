using ExpenseAlly.Application.Common.Interfaces;
using ExpenseAlly.Application.Features.Transactions.Dtos;
using ExpenseAlly.Application.Features.TransactionCategories.Dtos;
using Microsoft.EntityFrameworkCore;
using ExpenseAlly.Domain.Enums;

namespace ExpenseAlly.Application.Features.Transactions.Queries;

public class GetTransactionsQueryHandler : IRequestHandler<GetTransactionsQuery, List<TransactionDto>>
{
    private readonly IApplicationDbContext _context;

    public GetTransactionsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<TransactionDto>> Handle(GetTransactionsQuery request, CancellationToken cancellationToken)
    {
        return await _context.Transactions
            .AsNoTracking()
            .Include(x => x.Category)
            .Select(x => new TransactionDto
            {
                Id = x.Id,
                Type = x.Type,
                Amount = x.Amount,
                Date = x.Date,
                Notes = x.Notes,
                // Map Category to TransactionCategoryDto, handle null case
                Category = x.Category != null
                    ? new TransactionCategoryDto
                    {
                        Id = x.Category.Id,
                        Name = x.Category.Name,
                        Description = x.Category.Description,
                        Type = x.Category.Type.ToString() // Convert enum to string for TransactionCategoryDto
                    }
                    : null, // Null if no category is associated
                CategoryType = x.Category != null ? x.Category.Type : default(TransactionType) // Assign default value if null
            })
            .ToListAsync(cancellationToken);
    }
}
