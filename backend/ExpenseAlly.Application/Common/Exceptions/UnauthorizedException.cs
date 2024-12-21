using System.Net;

namespace ExpenseAlly.Application.Common.Exceptions;

public class UnauthorizedException : BaseException
{
    public UnauthorizedException(string message) : base(message, HttpStatusCode.Unauthorized)
    {
    }
}