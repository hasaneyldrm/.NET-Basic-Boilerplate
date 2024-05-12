using BasicBoilerplate.Interfaces;
using BasicBoilerplate.Models.Auth;
using BasicBoilerplate.Models.Database;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using HashCode = BasicBoilerplate.Utilities.HashCode;


namespace BasicBoilerplate.Services
{
    public class AuthService : IUserService
    {
        private readonly AppSettings _appSettings;
        private readonly AppDbContext _context;
        private readonly IMemoryCache _memCache;
        public IConfiguration Configuration { get; }

        public AuthService(
         AppDbContext context,
         IOptions<AppSettings> appSettings, IMemoryCache memoryCache, IConfiguration configuration)
        {
            _context = context;
            _appSettings = appSettings.Value;
            _memCache = memoryCache;
            Configuration = configuration;
        }

        public AuthenticateResponse Authenticate(AuthenticateRequest model, string ipAddress /*string channelKey*/)
        {
            var user = GetUser(model);

            if (user == null) return null;

            var ipExist = _memCache.TryGetValue(user.Id, out List<string> userIpList);
            if (!ipExist)
            {
                userIpList = new List<string> { ipAddress };
            }
            else
            {
                var isIpUsed = userIpList.FirstOrDefault(a => a == ipAddress);
                if (string.IsNullOrEmpty(isIpUsed) && userIpList.Count == 3)
                    throw new ArgumentException(
                        userIpList.Count +
                        " adetten fazla IP adresi ile giriş yapıldı. Lütfen yöneticiniz ile görüşün.");
                userIpList.Add(ipAddress);
            }

            _memCache.Set(user.Id, userIpList, TimeSpan.FromHours(10));
            _memCache.Set(user.Id, TimeSpan.FromHours(10));

            var refreshToken = GenerateRefreshToken(ipAddress, user.Id.ToString());

            var jwtToken = GenerateJwtToken(user);
            // save refresh token
            _memCache.Set(refreshToken.Token, user, TimeSpan.FromHours(10));
            return new AuthenticateResponse(user, jwtToken, refreshToken.Token);
        }

        public AuthenticateResponse RefreshToken(string token, string ipAddress)
        {
            var exist = _memCache.TryGetValue(token, out User user);
            if (exist)
            {
                var newRefreshToken = GenerateRefreshToken(ipAddress, user.Id.ToString());

                // generate new jwt
                var jwtToken = GenerateJwtToken(user);

                _memCache.Remove(token);
                _memCache.Set(newRefreshToken.Token, user, TimeSpan.FromHours(10));

                return new AuthenticateResponse(user, jwtToken, newRefreshToken.Token);
            }

            throw new Exception("Token is not valid");
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(24),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private RefreshToken GenerateRefreshToken(string ipAddress, string publicId)
        {
            using (new RNGCryptoServiceProvider())
            {
                var hashCode = HashCode.RandomString(12);
                var encrypt = HashCode.EncryptMd5(ipAddress + "|" + publicId + "|" + hashCode);

                return new RefreshToken
                {
                    Token = encrypt,
                    Expires = DateTime.UtcNow.AddHours(24),
                    Created = DateTime.UtcNow,
                    CreatedByIp = ipAddress
                };
            }
        }
        /// <summary>
        /// Burayı services katmanına koymamız gerekiyor.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private User GetUser(AuthenticateRequest model)
        {
            var Authuser = _context.Users.SingleOrDefault(x =>
                                                   x.Email == model.EMail && x.Password == model.Password && x.Status == Models.Global.UserStatus.Active);
            if (Authuser != null)
            {
                var user = new User
                {
                    Id = Authuser.Id,
                    Email = Authuser.Email,
                    Password = Authuser.Password
                };


                return user;
            }
            else
            {
                throw new Exception("User can't found");
            }
        }
    }
}

