using ExpenseAlly.Application.Common.Interfaces;
using ExpenseAlly.Application.Common.Models;
using Microsoft.Extensions.Logging;

namespace ExpenseAlly.Application.Features.SavingGoals.Commands;

public class DeleteSavingGoalCommandHandler : IRequestHandler<DeleteSavingGoalCommand, ResponseDto>
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<DeleteSavingGoalCommandHandler> _logger;

    public DeleteSavingGoalCommandHandler(IApplicationDbContext context, ILogger<DeleteSavingGoalCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<ResponseDto> Handle(DeleteSavingGoalCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var savingGoal = await _context.SavingGoals.FirstOrDefaultAsync(g => g.Id == request.Id, cancellationToken);

            if (savingGoal == null)
            {
                return new ResponseDto
                {
                    Errors = new List<ErrorDto>
                    {
                        new ErrorDto { Code = "NotFound", Message = "Saving goal not found." }
                    }
                };
            }

            _context.SavingGoals.Remove(savingGoal);
            await _context.SaveChangesAsync(cancellationToken);

            return new ResponseDto { Success = true };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting the saving goal.");
            return new ResponseDto
            {
                Errors = new List<ErrorDto>
                {
                    new ErrorDto { Code = "InternalServerError", Message = "Error occurred while deleting the saving goal." }
                }
            };
        }
    }
}