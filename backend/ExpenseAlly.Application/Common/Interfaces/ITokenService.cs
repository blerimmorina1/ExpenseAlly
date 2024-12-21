using ExpenseAlly.Application.Features.Account.Dtos;

namespace ExpenseAlly.Application.Common.Interfaces;

public interface ITokenService
{
    Task<TokenResponseDto> GetTokensAsync(string email);
    Task<TokenResponseDto> RefreshTokenAsync(string token, string refreshToken);
}
