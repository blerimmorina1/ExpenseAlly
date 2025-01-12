using ExpenseAlly.Application.Common.Interfaces;
using ExpenseAlly.Domain.Entities;
using FluentValidation;
using ExpenseAlly.Application.Common.Models;
using Microsoft.Extensions.Logging;
using ExpenseAlly.Domain.Enums;

namespace ExpenseAlly.Application.Features.Transactions.Commands;

public class CreateTransactionCommandHandler : IRequestHandler<CreateTransactionCommand, ResponseDto>
{
    private readonly IApplicationDbContext _context;
    private readonly INotificationService _notificationService;
    private readonly IValidator<CreateTransactionCommand> _validator;
    private readonly ILogger<CreateTransactionCommandHandler> _logger;

    public CreateTransactionCommandHandler(IApplicationDbContext context, IValidator<CreateTransactionCommand> validator, ILogger<CreateTransactionCommandHandler> logger, INotificationService notificationService)
    {
        _context = context;
        _validator = validator;
        _logger = logger;
        _notificationService = notificationService;
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
            var transaction = new Transaction
            {
                Type = request.Transaction.Type,
                CategoryId = request.Transaction.CategoryId,
                Amount = request.Transaction.Amount,
                Date = request.Transaction.Date,
                Notes = request.Transaction.Notes,
            };

            _context.Transactions.Add(transaction);

            // Update the budget
            var budget = await _context.Budgets
                .FirstOrDefaultAsync(b => b.StartDate <= request.Transaction.Date && b.EndDate >= request.Transaction.Date, cancellationToken);

            BudgetDetail? categoryDetail = null;
            if (budget != null)
            {
                categoryDetail = _context.BudgetDetails.Include(x => x.Budget).Include(x => x.Category).FirstOrDefault(cd => cd.CategoryId == request.Transaction.CategoryId && cd.BudgetId == budget.Id);

                if (categoryDetail != null)
                {
                    categoryDetail.Spent += request.Transaction.Amount;
                    budget.TotalSpent += request.Transaction.Amount;
                }
            }

            await _context.SaveChangesAsync(cancellationToken);

            if (budget != null)
            {
                await _notificationService.SendNotificationAsync(NotificationType.Budget, budget, cancellationToken);

                if (categoryDetail != null)
                {
                    await _notificationService.SendNotificationAsync(NotificationType.BudgetDetail, categoryDetail, cancellationToken);
                }
            }

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