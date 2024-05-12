using BasicBoilerplate.Interfaces;
using BasicBoilerplate.Models.Database;
using Microsoft.AspNetCore.Mvc;

namespace BasicBoilerplate.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IGeneralService<User> _userService;
        public UserController(AppDbContext context, IGeneralService<User> userService)
        {
            _context = context;
            _userService = userService;
        }

        [HttpGet]
        [Route("all")]
        public IActionResult GetAllUsers()
        {
            var users = _context.Users.ToList();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public IActionResult GetUser(string id)
        {
            var user = _userService.GetEntity(id);
            return Ok(user);
        }

        /// <summary>
        /// Burayı services katmanına çekmemiz gerekiyor.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddUser(User user)
        {
            user.Status = Models.Global.UserStatus.Active;
            user.CreatedAt = DateTime.Now;

            _context.Users.Add(user);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetAllUsers), new { id = user.Id }, user);
        }
    }
}
