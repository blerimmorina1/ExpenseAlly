using ExpenseAlly.Application.Features.Account.Commands;
using ExpenseAlly.Application.Features.Account.Validators;
using ExpenseAlly.Application.Features.Budgets.Commands;
using ExpenseAlly.Application.Features.Budgets.Validators;
using ExpenseAlly.Application.Features.TransactionCategories.Commands;
using ExpenseAlly.Application.Features.TransactionCategories.Validators;
using ExpenseAlly.Application.Features.Transactions.Commands;
using ExpenseAlly.Application.Features.Transactions.Validators;
using FluentValidation;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });

        // Validators
        services.AddTransient<IValidator<RegisterUserCommand>, RegisterUserCommandValidator>();
        services.AddTransient<IValidator<CreateTransactionCommand>, CreateTransactionCommandValidator>();
        services.AddTransient<IValidator<CreateCategoryCommand>, CreateCategoryCommandValidator>();
        services.AddTransient<IValidator<CreateBudgetCommand>, CreateBudgetCommandValidator>();
        services.AddTransient<IValidator<EditBudgetCommand>, EditBudgetCommandValidator>();

        return services;
    }
}
