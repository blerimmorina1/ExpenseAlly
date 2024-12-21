using Microsoft.AspNetCore.Identity;

namespace ExpenseAlly.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }


        public ApplicationUser  UpdateRefreshToken(string refreshToken, DateTime? refreshTokenExpiryTime)
        {
            RefreshToken = refreshToken;

            if (refreshTokenExpiryTime.HasValue)
            {
                RefreshTokenExpiryTime = refreshTokenExpiryTime.Value;
            }
            return this;
        }
    }
}
