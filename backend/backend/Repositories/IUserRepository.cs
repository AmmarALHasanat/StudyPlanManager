using backend.DTO;
using backend.Models;

namespace backend.Repositories
{
    public interface IUserRepository
    {
        //public bool UserExists(string username, string email);
        public User? GetByUsername(string username);

        public bool Exists(string username);

        public void Register(UserRegisterDto user);

        public bool Login(User user, string password);
    }
}
