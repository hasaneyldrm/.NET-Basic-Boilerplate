using BasicBoilerplate.Interfaces;
using BasicBoilerplate.Models.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using HashCode = BasicBoilerplate.Utilities.HashCode;

namespace BasicBoilerplate.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthenticationController : Controller
    {
        private readonly IUserService _userService;

        public AuthenticationController(IUserService userService, IMemoryCache memoryCache)
        {
            _userService = userService;
        }

        ///Header'e channelkey koyup agenti de userin authu ile koyulmalı.

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Authenticate([FromBody] AuthenticateRequest model/*[FromHeader] string channelKey*/)
        {
            var response = _userService.Authenticate(model, IpAddress());

            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });


            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("refresh")]
        public IActionResult RefreshToken([FromHeader] string refreshToken)
        {
            var response = _userService.RefreshToken(refreshToken, IpAddress());

            if (response == null)
                return Unauthorized(new { message = "Invalid token" });

            return Ok(response);
        }

        [HttpGet("SifreOlustur")]
        public string EncodeId(int id)
        {
            return HashCode.EncodeId(id);
        }

        [HttpGet("SifreKir")]
        public int DecodeId(string id)
        {
            return HashCode.DecodeId(id);
        }

        private string IpAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }
    }
}
