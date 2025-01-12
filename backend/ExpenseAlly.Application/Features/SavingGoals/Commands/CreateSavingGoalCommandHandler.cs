using ExpenseAlly.Application.Common.Interfaces;
using ExpenseAlly.Domain.Entities;
using FluentValidation;
using ExpenseAlly.Application.Common.Models;
using Microsoft.Extensions.Logging;
using ExpenseAlly.Domain.Enums;

namespace ExpenseAlly.Application.Features.SavingGoals.Commands;

public class CreateSavingGoalCommandHandler : IRequestHandler<CreateSavingGoalCommand, ResponseDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IValidator<CreateSavingGoalCommand> _validator;
    private readonly ILogger<CreateSavingGoalCommandHandler> _logger;
    private readonly INotificationService _notificationService;

    public CreateSavingGoalCommandHandler(IApplicationDbContext context, IValidator<CreateSavingGoalCommand> validator, ILogger<CreateSavingGoalCommandHandler> logger, INotificationService notificationService)
    {
        _context = context;
        _validator = validator;
        _logger = logger;
        _notificationService = notificationService;
    }

    public async Task<ResponseDto> Handle(CreateSavingGoalCommand request, CancellationToken cancellationToken)
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
            var savingGoal = new SavingGoal
            {
                Name = request.Name,
                TargetAmount = request.TargetAmount,
                CurrentAmount = request.CurrentAmount,
                Deadline = request.Deadline,
                IsCompleted = request.CurrentAmount >= request.TargetAmount? true: false,
                Notes = request.Notes
            };

            _context.SavingGoals.Add(savingGoal);

            if (request.CurrentAmount > 0)
            {
                var contribution = new Contribution
                {
                    SavingGoalId = savingGoal.Id,
                    Amount = request.CurrentAmount,
                    Date = DateTime.UtcNow
                };
                _context.Contributions.Add(contribution);
            }

            await _context.SaveChangesAsync(cancellationToken);

            if (savingGoal.IsCompleted)
            {
                await _notificationService.SendNotificationAsync(NotificationType.SavingGoal, savingGoal, cancellationToken);
            }

            return new ResponseDto
            {
                Success = true,
                Data = new
                {
                    savingGoal.Id,
                    savingGoal.Name,
                    savingGoal.TargetAmount,
                    savingGoal.CurrentAmount,
                    savingGoal.Deadline,
                    savingGoal.IsCompleted,
                    savingGoal.Notes
                }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating the saving goal.");
            return new ResponseDto
            {
                Errors = new List<ErrorDto>
                {
                    new ErrorDto
                    {
                        Code = "InternalServerError",
                        Message = "An error occurred while creating the saving goal."
                    }
                }
            };
        }
    }
}