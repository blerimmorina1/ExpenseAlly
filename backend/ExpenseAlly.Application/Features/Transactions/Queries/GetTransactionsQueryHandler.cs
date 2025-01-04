using ExpenseAlly.Application.Common.Interfaces;
using ExpenseAlly.Application.Features.Transactions.Dtos;

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
                CategoryName = x.Category.Name,
                CategoryType = x.Category.Type
            })
            .ToListAsync(cancellationToken);
    }
}
