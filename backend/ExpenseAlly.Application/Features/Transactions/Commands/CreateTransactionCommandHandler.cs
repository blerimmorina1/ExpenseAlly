using ExpenseAlly.Application.Common.Interfaces;
using ExpenseAlly.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseAlly.Application.Features.Transactions.Commands
{
    public class CreateTransactionCommandHandler : IRequestHandler<CreateTransactionCommand, Guid>
    {
        private readonly IApplicationDbContext _context;

        public CreateTransactionCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
        {
            if (request.Transaction.CategoryId == null || request.Transaction.CategoryId == Guid.Empty)
            {
                throw new ArgumentException("Category ID cannot be null or empty.");
            }

            // Find the category without applying query filters
            var category = await _context.TransactionCategories
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(c => c.Id == request.Transaction.CategoryId, cancellationToken);

            if (category == null)
            {
                throw new ArgumentException($"Category with ID {request.Transaction.CategoryId} not found.");
            }

            // Validate transaction type against category type
            if (category.Type != request.Transaction.Type)
            {
                throw new ArgumentException($"Transaction type '{request.Transaction.Type}' does not match category type '{category.Type}'.");
            }

            // Create and add the transaction
            var transaction = new Transaction
            {
                Type = request.Transaction.Type,
                CategoryId = request.Transaction.CategoryId,
                Amount = request.Transaction.Amount,
                Date = request.Transaction.Date,
                Notes = request.Transaction.Notes,
                Category = category
            };

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync(cancellationToken);

            return transaction.Id;
        }
    }
}
