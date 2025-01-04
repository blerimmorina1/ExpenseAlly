using ExpenseAlly.Application.Common.Interfaces;
using ExpenseAlly.Application.Common.Models;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace ExpenseAlly.Application.Features.Budgets.Commands;

public class DeleteBudgetCommand : IRequest<ResponseDto>
{
    public Guid Id { get; set; }

    public DeleteBudgetCommand(Guid id)
    {
        Id = id;
    }
}

public class DeleteBudgetCommandHandler : IRequestHandler<DeleteBudgetCommand, ResponseDto>
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<DeleteBudgetCommandHandler> _logger;

    public DeleteBudgetCommandHandler(IApplicationDbContext context, ILogger<DeleteBudgetCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<ResponseDto> Handle(DeleteBudgetCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var budget = await _context.Budgets.Where(x => x.Id == request.Id).FirstOrDefaultAsync();

            if (budget != null)
            {
                _context.Budgets.Remove(budget);
                await _context.SaveChangesAsync(cancellationToken);

                return new ResponseDto
                {
                    Success = true
                };
            }
            else
            {
                return new ResponseDto
                {
                    Errors = new List<ErrorDto>{ new ErrorDto
                {
                    Code = "NotFound",
                    Message = "Budget with the provided ID doesn't exist."
                } }
                };
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while deleting budget.");
            throw;
        }
    }
}