using ExpenseAlly.Application.Common.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseAlly.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ApiControllerBase : ControllerBase
{
    private ISender? mediator;

    protected ISender _mediator => mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();

    private ICurrentUserService? _currentUserService;
    protected ICurrentUserService CurrentUserService => _currentUserService ??= HttpContext.RequestServices.GetRequiredService<ICurrentUserService>();
}
