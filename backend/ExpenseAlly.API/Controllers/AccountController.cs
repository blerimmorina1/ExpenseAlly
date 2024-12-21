using ExpenseAlly.Application.Common.Exceptions;
using ExpenseAlly.Application.Common.Models;
using ExpenseAlly.Application.Features.Account.Commands;
using ExpenseAlly.Application.Features.Account.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseAlly.API.Controllers;

public class AccountController : ApiControllerBase
{
    [HttpPost("register")]
    public async Task<ResponseDto> Register(RegisterUserCommand request)
    {
        return await Mediator.Send(request);
    }

    [HttpPost("login")]
    public async Task<TokenResponseDto> Login(LoginUserCommand request)
    {
        return await Mediator.Send(request);
    }

    [HttpPost("refreshToken")]
    public async Task<TokenResponseDto> RefreshToken(RefreshTokenCommand request)
    {
        return await Mediator.Send(request);
    }
}