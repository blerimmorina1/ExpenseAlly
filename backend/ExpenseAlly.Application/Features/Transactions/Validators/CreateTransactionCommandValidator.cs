using ExpenseAlly.Application.Common.Interfaces;
using ExpenseAlly.Application.Features.Transactions.Commands;
using FluentValidation;

namespace ExpenseAlly.Application.Features.Transactions.Validators;

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
                .AnyAsync(c => c.Id == categoryId, cancellationToken);
            })
            .WithName("CategoryId")
            .WithMessage("Invalid Transaction Category. Please select a valid category.");
    }
}
