using ExpenseAlly.Application.Common.Models;

namespace ExpenseAlly.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<ResponseDto> RegisterUserAsync(string email, string password, string firstName, string lastName);
    Task<ResponseDto> SigninUserAsync(string email, string password);
}
