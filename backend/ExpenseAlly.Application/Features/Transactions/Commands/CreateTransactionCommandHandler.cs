using ExpenseAlly.Application.Common.Interfaces;
using ExpenseAlly.Domain.Entities;
using FluentValidation;
using ExpenseAlly.Application.Common.Models;
using Microsoft.Extensions.Logging;

namespace ExpenseAlly.Application.Features.Transactions.Commands;

public class CreateTransactionCommandHandler : IRequestHandler<CreateTransactionCommand, ResponseDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IValidator<CreateTransactionCommand> _validator;
    private readonly ILogger<CreateTransactionCommandHandler> _logger;

    public CreateTransactionCommandHandler(IApplicationDbContext context, IValidator<CreateTransactionCommand> validator, ILogger<CreateTransactionCommandHandler> logger)
    {
        _context = context;
        _validator = validator;
        _logger = logger;
    }

    public async Task<ResponseDto> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
    {
        // Validate the command
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            return new ResponseDto
            {
                Errors = validationResult.Errors.Select(e => new ErrorDto
                {
                    Code = "ValidationError",
                    Message = e.ErrorMessage
                })
            };
        }

        try
        {
            var category = await _context.TransactionCategories
                .FirstOrDefaultAsync(c => c.Id == request.Transaction.CategoryId, cancellationToken);

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

            return new ResponseDto
            {
                Success = true
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating the transaction.");
            return new ResponseDto
            {
                Errors = new List<ErrorDto>
                {
                    new ErrorDto
                    {
                        Code = "InternalServerError",
                        Message = "An error occurred while creating the transaction."
                    }
                }
            };
        }
    }
}