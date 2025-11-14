using backend.Data;
using backend.DTO;
using backend.Helpers;
using backend.Models;
using backend.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtService _jwtService;
        public AuthController(IUserRepository userRepository, JwtService jwtService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] UserRegisterDto request)
        {
            if (_userRepository.Exists(request.Username))
                return BadRequest("Username already exists");
            _userRepository.Register(request);

           
            return Ok("User registered successfully");
        } 

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLoginDto request)
        {
            var user = _userRepository.GetByUsername(request.Username);
            if (user == null) return Unauthorized("User not found");
            
            if (!_userRepository.Login(user,request.Password))
                return Unauthorized("Invalid password");

            var token = _jwtService.GenerateToken(user);
            return Ok(new { token });
        }

    }
}
