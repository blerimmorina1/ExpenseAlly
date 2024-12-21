using ExpenseAlly.Application.Common.Interfaces;
using ExpenseAlly.Application.Common.Models;
using ExpenseAlly.Application.Features.Account.Dtos;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace ExpenseAlly.Application.Features.Account.Commands;

public class RefreshTokenCommand : IRequest<TokenResponseDto>
{
    public string Token { get; set; }
    public string RefreshToken { get; set; }
}

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, TokenResponseDto>
{
    private readonly ITokenService _tokenService;

    private readonly ILogger<RegisterUserCommandHandler> _logger;

    public RefreshTokenCommandHandler(ITokenService tokenService, ILogger<RegisterUserCommandHandler> logger)
    {
        _tokenService = tokenService;
        _logger = logger;
    }

    public async Task<TokenResponseDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var tokenResponse = await _tokenService.RefreshTokenAsync(request.Token, request.RefreshToken);
            return tokenResponse;
        }
        catch (Exception ex)
        {
            throw;
        }
}
}

