using ExpenseAlly.Application.Common.Interfaces;
using ExpenseAlly.Application.Features.Account.Dtos;
using Microsoft.Extensions.Logging;

namespace ExpenseAlly.Application.Features.Account.Commands;

public class LoginUserCommand : IRequest<TokenResponseDto>
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, TokenResponseDto>
{
    private readonly IIdentityService _identityService;
    private readonly ITokenService _tokenService;
    private readonly ILogger<LoginUserCommandHandler> _logger;

    public LoginUserCommandHandler(
        IIdentityService identityService,
        ILogger<LoginUserCommandHandler> logger,
        ITokenService tokenService)
    {
        _identityService = identityService;
        _logger = logger;
        _tokenService = tokenService;
    }

    public async Task<TokenResponseDto> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _identityService.SigninUserAsync(request.Email, request.Password);

            var tokenResponse = new TokenResponseDto(); 

            if (response.Success)
            {
                 tokenResponse = await _tokenService.GetTokensAsync(request.Email); 
            }

            return new TokenResponseDto
            {
                Success = response.Success,
                Errors = response.Errors,
                AccessToken = tokenResponse.AccessToken,
                RefreshToken = tokenResponse.RefreshToken,
                Expiry = tokenResponse.Expiry,
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred during authentication.");
            throw;
        }
    }
}