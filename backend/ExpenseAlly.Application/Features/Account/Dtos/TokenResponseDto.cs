using ExpenseAlly.Application.Common.Models;

namespace ExpenseAlly.Application.Features.Account.Dtos
{
    public class TokenResponseDto : ResponseDto
    {
        public  string AccessToken { get; init; }
        public  DateTime Expiry { get; init; }
        public  string RefreshToken { get; init; }
    }
}
