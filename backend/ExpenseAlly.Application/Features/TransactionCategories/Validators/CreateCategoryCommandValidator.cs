using FluentValidation;
using ExpenseAlly.Application.Features.TransactionCategories.Commands;

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Category name is required.")
            .MaximumLength(50).WithMessage("Category name must not exceed 50 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(250).WithMessage("Category description must not exceed 250 characters.")
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleFor(x => x.Type)
            .IsInEnum().WithMessage("Invalid category type.");
    }
}
