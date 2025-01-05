using ExpenseAlly.Application.Common.Interfaces;
using ExpenseAlly.Application.Common.Models;
using ExpenseAlly.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace ExpenseAlly.Application.Features.Budgets.Commands;

public class SaveCategoryBudgetCommand : IRequest<ResponseDto>
{
    public Guid Id { get; set; }
    public decimal Limit { get; set; }
}

public class SaveCategoryBudgetCommandHandler : IRequestHandler<SaveCategoryBudgetCommand, ResponseDto>
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<SaveCategoryBudgetCommandHandler> _logger;
    private readonly INotificationService _notificationService;

    public SaveCategoryBudgetCommandHandler(IApplicationDbContext context, ILogger<SaveCategoryBudgetCommandHandler> logger, INotificationService notificationService)
    {
        _context = context;
        _logger = logger;
        _notificationService = notificationService;
    }

    public async Task<ResponseDto> Handle(SaveCategoryBudgetCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var budgetDetail = await _context.BudgetDetails.Include(x=> x.Category)
                .FirstOrDefaultAsync(b => b.Id == request.Id, cancellationToken);

            if (budgetDetail == null)
            {
                return new ResponseDto
                {
                    Success = false,
                    Errors = new List<ErrorDto>
                    {
                        new ErrorDto { Code = "NotFound", Message = "Budget for category with the provided ID doesn't exist." }
                    }
                };
            }

            decimal oldLimit = budgetDetail.Limit;
            budgetDetail.Limit = request.Limit;
            _context.BudgetDetails.Update(budgetDetail);

            var budget = await _context.Budgets.FirstOrDefaultAsync(b => b.Id == budgetDetail.BudgetId, cancellationToken);

            if (budget != null)
            {
                budget.TotalLimit += request.Limit - oldLimit;
                _context.Budgets.Update(budget);
            }

            await _context.SaveChangesAsync(cancellationToken);

            if (budget != null)
            {
                //await _notificationService.SendNotificationAsync(NotificationType.Budget, budget, cancellationToken);

                await _notificationService.SendNotificationAsync(NotificationType.BudgetDetail, budgetDetail, cancellationToken);
            }

            return new ResponseDto
            {
                Success = true
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while saving the category budget.");
            throw;
        }
    }
}