using ExpenseAlly.Application.Common.Interfaces;
using ExpenseAlly.Application.Common.Models;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace ExpenseAlly.Application.Features.Account.Commands;

public class RegisterUserCommand : IRequest<ResponseDto>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
}

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, ResponseDto>
{
    private readonly IIdentityService _identityService;
    private readonly IValidator<RegisterUserCommand> _validator;
    private readonly ILogger<RegisterUserCommandHandler> _logger;

    public RegisterUserCommandHandler(
        IIdentityService identityService,
        IValidator<RegisterUserCommand> validator,
        ILogger<RegisterUserCommandHandler> logger)
    {
        _identityService = identityService;
        _validator = validator;
        _logger = logger;
    }

    public async Task<ResponseDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
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
            var response = await _identityService.RegisterUserAsync(
                request.Email, request.Password, request.FirstName, request.LastName);

            return new ResponseDto
            {
                Success = response.Success,
                Errors = response.Errors
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while registering the user.");
            throw;
        }
    }
}

