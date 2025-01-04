using ExpenseAlly.Application.Common.Interfaces;
using ExpenseAlly.Application.Common.Models;
using ExpenseAlly.Application.Features.Budgets.Dtos;
using ExpenseAlly.Domain.Entities;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace ExpenseAlly.Application.Features.Budgets.Commands;

public class CreateBudgetCommand : IRequest<ResponseDto>
{
    public string Name { get; set; }
    public decimal TotalLimit { get; set; }
    public DateTime StartDate { get; set; }
    public List<BudgetDetailDto> BudgetDetails { get; set; }
}

public class CreateBudgetCommandHandler : IRequestHandler<CreateBudgetCommand, ResponseDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IValidator<CreateBudgetCommand> _validator;
    private readonly ILogger<CreateBudgetCommandHandler> _logger;

    public CreateBudgetCommandHandler(IApplicationDbContext context, IValidator<CreateBudgetCommand> validator, ILogger<CreateBudgetCommandHandler> logger)
    {
        _context = context;
        _validator = validator;
        _logger = logger;
    }

    public async Task<ResponseDto> Handle(CreateBudgetCommand request, CancellationToken cancellationToken)
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
                })
            };
        }

        try
        {
            var budget = new Budget
            {
                Name = request.Name,
                TotalLimit = request.TotalLimit,
                StartDate = request.StartDate,
                EndDate = request.StartDate.AddMonths(1),
                BudgetDetails = request.BudgetDetails.Select(detail => new BudgetDetail
                {
                    CategoryId = detail.CategoryId,
                    Limit = detail.Limit
                }).ToList()
            };

            _context.Budgets.Add(budget);
            await _context.SaveChangesAsync(cancellationToken);

            return new ResponseDto
            {
                Success = true
            }; 
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating a budget.");
            throw; 
        }
    }
}