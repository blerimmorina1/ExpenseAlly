using ExpenseAlly.Application.Common.Exceptions;
using ExpenseAlly.Application.Common.Models;
using ExpenseAlly.Application.Features.Account.Commands;
using ExpenseAlly.Application.Features.Account.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseAlly.API.Controllers;

public class AccountController : ApiControllerBase
{
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ResponseDto> Register(RegisterUserCommand request)
    {
        return await _mediator.Send(request);
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<TokenResponseDto> Login(LoginUserCommand request)
    {
        return await _mediator.Send(request);
    }

    [AllowAnonymous]
    [HttpPost("refreshToken")]
    public async Task<TokenResponseDto> RefreshToken(RefreshTokenCommand request)
    {
        return await _mediator.Send(request);
    }
}