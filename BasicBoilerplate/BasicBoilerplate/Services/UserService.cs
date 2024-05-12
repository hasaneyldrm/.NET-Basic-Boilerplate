using BasicBoilerplate.Interfaces;
using BasicBoilerplate.Models.Database;
using BasicBoilerplate.Models.Global;

namespace BasicBoilerplate.Services
{
    public class UserService : IGeneralService<User>
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Sadece create işlemi bitti. Diğer işlemlerin de yapılması gerekiyor.
        /// </summary>
        /// <param name="context"></param>
        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public string CreateEntity(User entity)
        {
            throw new NotImplementedException();
        }

        public void DisableEntity(string id)
        {
            throw new NotImplementedException();
        }

        public List<User> GetAll()
        {
            throw new NotImplementedException();
        }

        public User GetEntity(string id)
        {
            var user = _context.Users.FirstOrDefault(predicate: x =>
                x.Id == x.Id && x.Status == UserStatus.Active);


            if (user == null)
            {
                throw new Exception("User can't be found");
            }
            else
            {
                return user;
            }

        }

        public void UpdateEntity(User entity)
        {
            throw new NotImplementedException();
        }

    }
}
