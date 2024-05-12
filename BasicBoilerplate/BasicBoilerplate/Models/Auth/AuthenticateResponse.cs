using BasicBoilerplate.Models.Database;

namespace BasicBoilerplate.Models.Auth
{
    public class AuthenticateResponse
    {
        public AuthenticateResponse(User user, string jwtToken, string refreshToken)
        {
            Id = Utilities.HashCode.EncodeId(Convert.ToInt32(user.Id));
            JwtToken = jwtToken;
            RefreshToken = refreshToken;
        }

        public string Id { get; set; }
        public string JwtToken { get; set; }
        public string RefreshToken { get; set; }
    }
}

