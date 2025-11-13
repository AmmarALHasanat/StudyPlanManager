using backend.Data;
using backend.Helpers;
using backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly JwtService _jwtService;

        public AuthController(ApplicationDbContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] UserRegisterDto request)
        {
            if (_context.Users.Any(u => u.Username == request.Username))
                return BadRequest("Username already exists");

            using var hmac = new HMACSHA512();
            byte[] salt = hmac.Key;
            byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(request.Password));

            var user = new User
            {
                Username = request.Username,
                PasswordHash = hash,
                PasswordSalt = salt,
                Role = "User"
            };

            _context.Users.Add(user);
            _context.SaveChanges();
            return Ok("User registered successfully");
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLoginDto request)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == request.Username);
            if (user == null) return Unauthorized("User not found");
            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computed = hmac.ComputeHash(Encoding.UTF8.GetBytes(request.Password));
            bool result = computed.SequenceEqual(user.PasswordHash);
            if (!result)
                return Unauthorized("Invalid password");

            var token = _jwtService.GenerateToken(user);
            return Ok(new { token });
        }

    }
    public class UserLoginDto
    {
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
    }
    public class UserRegisterDto
    {
        [EmailAddress]
        public string Email { get; set; } = "";
        [Required]
        public string Name { get; set; } = "";
        [Required]
        public string Username { get; set; } = "";
        [Required] 
        public string Password { get; set; } = "";
        [Required]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string PasswordConfirm { get; set; } = "";
    }
}
