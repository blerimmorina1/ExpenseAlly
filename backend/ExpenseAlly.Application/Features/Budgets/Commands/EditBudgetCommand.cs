using ExpenseAlly.Application.Common.Interfaces;
using ExpenseAlly.Application.Common.Models;
using ExpenseAlly.Application.Features.Budgets.Dtos;
using ExpenseAlly.Domain.Entities;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace ExpenseAlly.Application.Features.Budgets.Commands;

public class EditBudgetCommand : IRequest<ResponseDto>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal TotalLimit { get; set; }
    public DateTime StartDate { get; set; }
    public List<BudgetDetailDto> BudgetDetails { get; set; }
}

public class EditBudgetCommandHandler : IRequestHandler<EditBudgetCommand, ResponseDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IValidator<EditBudgetCommand> _validator;
    private readonly ILogger<EditBudgetCommandHandler> _logger;

    public EditBudgetCommandHandler(IApplicationDbContext context, IValidator<EditBudgetCommand> validator, ILogger<EditBudgetCommandHandler> logger)
    {
        _context = context;
        _validator = validator;
        _logger = logger;
    }

    public async Task<ResponseDto> Handle(EditBudgetCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            return new ResponseDto
            {
                Errors = validationResult.Errors.Select(e => new ErrorDto
                {
                    Code = "ValidationError",
                    Message = e.ErrorMessage
                }).ToList()
            };
        }

        try
        {
            var budget = await _context.Budgets
                .Include(b => b.BudgetDetails) 
                .FirstOrDefaultAsync(b => b.Id == request.Id, cancellationToken);

            if (budget == null)
            {
                return new ResponseDto
                {
                    Success = false,
                    Errors = new List<ErrorDto>
                    {
                        new ErrorDto { Code = "NotFound", Message = "Budget with the provided ID doesn't exist." }
                    }
                };
            }

            budget.Name = request.Name;
            budget.TotalLimit = request.TotalLimit;
            budget.StartDate = request.StartDate;
            budget.EndDate = request.StartDate.AddMonths(1);

            var updatedDetails = request.BudgetDetails;

            var detailsToRemove = budget.BudgetDetails
                .Where(existing => !updatedDetails.Any(updated => updated.Id == existing.Id))
                .ToList();

            _context.BudgetDetails.RemoveRange(detailsToRemove);

            foreach (var detailDto in updatedDetails)
            {
                var existingDetail = budget.BudgetDetails.FirstOrDefault(d => d.Id == detailDto.Id);
                if (existingDetail != null)
                {
                    existingDetail.CategoryId = detailDto.CategoryId;
                    existingDetail.Limit = detailDto.Limit;
                }
                else
                {
                    budget.BudgetDetails.Add(new BudgetDetail
                    {
                        CategoryId = detailDto.CategoryId,
                        Limit = detailDto.Limit
                    });
                }
            }

            await _context.SaveChangesAsync(cancellationToken);

            return new ResponseDto
            {
                Success = true
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while editing the budget.");
            throw;
        }
    }
}