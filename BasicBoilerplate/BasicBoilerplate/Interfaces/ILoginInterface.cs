using BasicBoilerplate.Models.Auth;
using BasicBoilerplate.Models.Database;
using BasicBoilerplate.Models.Global;

namespace BasicBoilerplate.Interfaces
{
    public interface ILoginInterface
    {
        AuthenticateResponse Authenticate(AuthenticateRequest model, string ipAddress, string channelKey);
        AuthenticateResponse RefreshToken(string token, string ipAddress);
        IEnumerable<User> GetAll();
        User GetById(int id);
        int GetCompanyIdFromCache(int id);
        ChannelType GetChannelKey(string channelKey);
    }
}
