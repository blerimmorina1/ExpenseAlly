using ExpenseAlly.Application.Common.Interfaces;
using ExpenseAlly.Application.Features.Transactions.Commands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseAlly.Application.Features.Transactions.Validators
{
    public class CreateTransactionCommandValidator : AbstractValidator<CreateTransactionCommand>
    {
        private readonly IApplicationDbContext _context;

        public CreateTransactionCommandValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleFor(x => x.Transaction.CategoryId)
                .NotEmpty()
                .WithMessage("Category ID is required.");

            RuleFor(x => x.Transaction.CategoryId)
                .MustAsync(async (categoryId, cancellationToken) =>
                {
                    return await _context.TransactionCategories
                    .IgnoreQueryFilters()
                    .AnyAsync(c => c.Id == categoryId, cancellationToken);
                })
                .WithName("CategoryId")
                .WithMessage("Invalid Transaction Category. Please select a valid category.");
            RuleFor(x => x)
              .MustAsync(async (request, cancellationToken) =>
              {
                  var category = await _context.TransactionCategories
                    .IgnoreQueryFilters()
                    .FirstOrDefaultAsync(c => c.Id == request.Transaction.CategoryId, cancellationToken);

                  return category != null && category.Type == request.Transaction.Type;
              })
              .WithMessage("Transaction type must match the category type.");
        }
    }
}
