using backend.DTO;
using backend.Models;

namespace backend.Repositories
{
    public interface IUserRepository
    {
        public User? GetByUsername(string username);

        public bool Exists(string username);

        public string Register(UserRegisterDto user);

        public bool Login(User user, string password);

        public void Update(User user);
    }
}
