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
            if (request.CategoryId == null || request.CategoryId == Guid.Empty)
            {
                throw new ArgumentException("Category ID cannot be null or empty.");
            }

            Console.WriteLine($"Finding category with ID: {request.CategoryId}");

            var category = await _context.TransactionCategories.FindAsync(new object[] { request.CategoryId }, cancellationToken);

            if (category == null)
            {
                throw new ArgumentException($"Category with ID {request.CategoryId} not found.");
            }

            if (category.Type != request.Type)
            {
                throw new ArgumentException($"Transaction type '{request.Type}' does not match category type '{category.Type}'.");
            }

            var transaction = new Transaction
            {
                Type = request.Type,
                CategoryId = request.CategoryId,
                Amount = request.Amount,
                Date = request.Date,
                Notes = request.Notes,
                Category = category
            };

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync(cancellationToken);

            return transaction.Id;
        }
    }
}
