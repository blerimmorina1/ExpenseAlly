using FluentValidation;
using ExpenseAlly.Application.Features.SavingGoals.Commands;

namespace ExpenseAlly.Application.Features.SavingGoals.Validators;

public class ContributeSavingGoalCommandValidator : AbstractValidator<ContributeSavingGoalCommand>
{
    public ContributeSavingGoalCommandValidator()
    {
        RuleFor(x => x.SavingGoalId)
            .NotEmpty().WithMessage("Saving Goal ID is required.");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Amount must be greater than zero.");
    }
}