using BasicBoilerplate.Models.Auth;

namespace BasicBoilerplate.Interfaces
{
    public interface IUserService
    {
        /// <summary>
        /// Daha sonrasında channelkey eklenecek.
        /// </summary>

        AuthenticateResponse Authenticate(AuthenticateRequest model, string ipAddress /*string channelKey*/);
        AuthenticateResponse RefreshToken(string token, string ipAddress);

        //Status GetChannelKey(string channelKey);
    }
}
