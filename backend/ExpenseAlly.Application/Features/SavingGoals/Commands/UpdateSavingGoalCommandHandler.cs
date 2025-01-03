using ExpenseAlly.Application.Common.Interfaces;
using ExpenseAlly.Application.Common.Models;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace ExpenseAlly.Application.Features.SavingGoals.Commands;

public class UpdateSavingGoalCommandHandler : IRequestHandler<UpdateSavingGoalCommand, ResponseDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IValidator<UpdateSavingGoalCommand> _validator;
    private readonly ILogger<UpdateSavingGoalCommandHandler> _logger;

    public UpdateSavingGoalCommandHandler(
        IApplicationDbContext context,
        IValidator<UpdateSavingGoalCommand> validator,
        ILogger<UpdateSavingGoalCommandHandler> logger)
    {
        _context = context;
        _validator = validator;
        _logger = logger;
    }

    public async Task<ResponseDto> Handle(UpdateSavingGoalCommand request, CancellationToken cancellationToken)
    {
        // Validate the request
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
            // Fetch the existing saving goal
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

            // Update properties
            savingGoal.Name = request.Name;
            savingGoal.TargetAmount = request.TargetAmount;
            savingGoal.CurrentAmount = request.CurrentAmount;
            savingGoal.Deadline = request.Deadline;
            savingGoal.IsCompleted = request.IsCompleted;
            savingGoal.Notes = request.Notes;

            _context.SavingGoals.Update(savingGoal);
            await _context.SaveChangesAsync(cancellationToken);

            return new ResponseDto { Success = true };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating the saving goal.");
            return new ResponseDto
            {
                Errors = new List<ErrorDto>
                {
                    new ErrorDto { Code = "InternalServerError", Message = "Error occurred while updating the saving goal." }
                }
            };
        }
    }
}