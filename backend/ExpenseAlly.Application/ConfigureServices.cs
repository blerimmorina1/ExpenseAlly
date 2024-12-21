
using ExpenseAlly.Application.Features.Account.Commands;
using ExpenseAlly.Application.Features.Account.Validators;
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

       services.AddTransient<IValidator<RegisterUserCommand>, RegisterUserCommandValidator>();

        return services;
    }
}
