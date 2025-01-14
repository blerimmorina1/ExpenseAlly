using ExpenseAlly.Application.Common.Interfaces;
using ExpenseAlly.Application.Common.Models;
using ExpenseAlly.Domain.Entities;
using ExpenseAlly.Domain.Enums;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ExpenseAlly.Application.Features.SavingGoals.Commands;

public class ContributeSavingGoalCommand : IRequest<ResponseDto>
{
    public Guid SavingGoalId { get; set; }
    public decimal Amount { get; set; }
}

public class ContributeSavingGoalCommandHandler : IRequestHandler<ContributeSavingGoalCommand, ResponseDto>
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<ContributeSavingGoalCommandHandler> _logger;
    private readonly INotificationService _notificationService;

    public ContributeSavingGoalCommandHandler(IApplicationDbContext context, ILogger<ContributeSavingGoalCommandHandler> logger, INotificationService notificationService)
    {
        _context = context;
        _logger = logger;
        _notificationService = notificationService;
    }

    public async Task<ResponseDto> Handle(ContributeSavingGoalCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var savingGoal = await _context.SavingGoals.FirstOrDefaultAsync(g => g.Id == request.SavingGoalId, cancellationToken);

            if (savingGoal == null)
            {
                return new ResponseDto
                {
                    Errors = new List<ErrorDto>
                    {
                        new ErrorDto
                        {
                            Code = "NotFound", 
                            Message = "Saving goal not found."
                        }
                    }
                };
            }

            savingGoal.CurrentAmount += request.Amount;
            if (savingGoal.CurrentAmount >= savingGoal.TargetAmount)
            {
                savingGoal.IsCompleted = true;
            }
            
            var contribution = new Contribution
            {
                SavingGoalId = request.SavingGoalId,
                Amount = request.Amount,
                Date = DateTime.UtcNow
            };
            _context.Contributions.Add(contribution);
            
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
            _logger.LogError(ex, "Error contributing to saving goal.");
            return new ResponseDto
            {
                Errors = new List<ErrorDto>
                {
                    new ErrorDto
                    {
                        Code = "InternalServerError", 
                        Message = "Error contributing to saving goal."
                    }
                }
            };
        }
    }
}