using ExpenseAlly.Application.Common.Models;
using ExpenseAlly.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace ExpenseAlly.Infrastructure.Services;

public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    public IdentityService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<ResponseDto> RegisterUserAsync(string email, string password, string firstName, string lastName)
    {
        var dbUser = await _userManager.FindByEmailAsync(email);

        if (dbUser != null)
        {
            return new ResponseDto
            {
                Errors = new List<ErrorDto>{ new ErrorDto
                {
                    Code = "UserExists",
                    Message = "User with this email address already exists."
                } }
            };
        }

        var user = new ApplicationUser
        {
            UserName = email,
            Email = email,
            FirstName = firstName,
            LastName = lastName
        };

        var result = await _userManager.CreateAsync(user, password);

        if (result.Succeeded)
        {
            return new ResponseDto
            {
                Success = true
            };
        }

        return new ResponseDto
        {
            Errors = result.Errors.Select(error => new ErrorDto
            {
                Code = error.Code,
                Message = error.Description
            })
        };
    }

    public async Task<ResponseDto> SigninUserAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user == null)
        {
            return new ResponseDto
            {
                Errors = new List<ErrorDto>{ new ErrorDto
                {
                    Code = "InvalidCredentials",
                    Message = "Invalid username or password."
                } }
            };
        }

        var result = await _signInManager.PasswordSignInAsync(email, password, true, false);

        if (result.Succeeded)
        {
            return new ResponseDto
            {
                Success = true
            };
        }

        return new ResponseDto
        {
            Errors = new List<ErrorDto>{ new ErrorDto
            {
                Code = "InvalidCredentials",
                Message = "Invalid email or password."
            } }
        };
    }
}