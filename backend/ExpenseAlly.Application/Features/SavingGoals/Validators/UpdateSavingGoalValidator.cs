using FluentValidation;
using ExpenseAlly.Application.Features.SavingGoals.Commands;

public class UpdateSavingGoalValidator : AbstractValidator<UpdateSavingGoalCommand>
{
    public UpdateSavingGoalValidator()
    {
        RuleFor(x => x.Id).
            NotEmpty().WithMessage("Id is required.");
        RuleFor(x => x.Name).
            NotEmpty().WithMessage("Name is required.");
        RuleFor(x => x.TargetAmount).
            GreaterThan(0).WithMessage("Target amount must be greater than zero.");
    }
}