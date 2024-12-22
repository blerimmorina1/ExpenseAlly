using Azure.Core;
using ExpenseAlly.Application.Common.Exceptions;
using ExpenseAlly.Application.Features.Account.Dtos;
using ExpenseAlly.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ExpenseAlly.Infrastructure.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;
    private readonly UserManager<ApplicationUser> _userManager;

    public TokenService(IConfiguration configuration, UserManager<ApplicationUser> userManager)
    {
        _configuration = configuration;
        _userManager = userManager;
    }

    public async Task<TokenResponseDto> GetTokensAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email); 
        return await GenerateTokensAndUpdateUser(user, false);
    }

    private async Task<string> GenerateToken(ApplicationUser user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new(ClaimTypes.Name, user.UserName ?? string.Empty),
                new(ClaimTypes.Email, user.Email ?? string.Empty)
        }; 

        var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
        claims,
            expires: DateTime.Now.AddMinutes(15),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<TokenResponseDto> GenerateRefreshToken()
    {
        byte[] randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        var refreshToken = Convert.ToBase64String(randomNumber);

        return new TokenResponseDto
        {
            RefreshToken = refreshToken,
            Expiry = DateTime.UtcNow.AddDays(7)
        };
    }

    public async Task<TokenResponseDto> RefreshTokenAsync(string token, string refreshToken)
    {
        var userPrincipal = GetPrincipalFromExpiredToken(token);

        var identity = userPrincipal.Identity as ClaimsIdentity;
        IEnumerable<Claim> claims = identity.Claims;
        var userId = claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;

        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
        {
            throw new UnauthorizedException("Authentication failed.");
        }


        if (user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            throw new UnauthorizedException("Invalid refresh token.");
        }

        return await GenerateTokensAndUpdateUser(user, true);
    }

    private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = securityKey,
            ValidateIssuer = false,
            ValidateAudience = false,
            RoleClaimType = ClaimTypes.Role,
            ClockSkew = TimeSpan.Zero,
            ValidateLifetime = false
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        try
        {
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(
                SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
            {
                throw new UnauthorizedException("Invalid token.");
            }

            return principal;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    private async Task<TokenResponseDto> GenerateTokensAndUpdateUser(ApplicationUser user, bool refresh)
    {
        try
        {
            string token = await GenerateToken(user);
            var refreshToken = await GenerateRefreshToken();

            user.UpdateRefreshToken(refreshToken.RefreshToken, !refresh ? refreshToken.Expiry : null);

            await _userManager.UpdateAsync(user);

            return new TokenResponseDto()
            {
                AccessToken = token,
                RefreshToken = refreshToken.RefreshToken,
                Expiry = user.RefreshTokenExpiryTime.Value
            };
        }
        catch (Exception e)
        {
            throw;
        }
    }
}
