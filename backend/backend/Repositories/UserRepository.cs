using backend.Data;
using backend.DTO;
using backend.Models;
using backend.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
namespace backend.Repositories
{
    public class UserRepository : IUserRepository
    {
        private ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool Exists(string username)
        {
            return _context.Users.Any(u => u.Username == username);
        }

        public User? GetByUsername(string username)
        {
            return _context.Users.FirstOrDefault(u => u.Username == username);
        }

        public void Register(UserRegisterDto userDto)
        {
            using var hmac = new HMACSHA512();
            byte[] salt = hmac.Key;
            byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(userDto.Password));

            var user = new User
            {
                Username = userDto.Username,
                PasswordHash = hash,
                PasswordSalt = salt,
                Role = "User"
            };

            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public bool Login(User user, string password)
        {
            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computed = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return computed.SequenceEqual(user.PasswordHash); 
        }
    }
}
