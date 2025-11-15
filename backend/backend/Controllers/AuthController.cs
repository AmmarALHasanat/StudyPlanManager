using backend.DTO;
using backend.Helpers;
using backend.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtService _jwtService;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;

        public AuthController(IUserRepository userRepository, JwtService jwtService, IEmailService emailService, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _emailService = emailService;
            _configuration = configuration;
        }


        [HttpPost("register")]
        public IActionResult Register([FromBody] UserRegisterDto request)
        {
            if (_userRepository.Exists(request.Username))
                return BadRequest("Username already exists");
            var token = _userRepository.Register(request);
            var urlToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            var clientUrl = _configuration["AppSettings:ClientUrl"] ?? "http://localhost:5173";
            var confirmEndpoint = $"{clientUrl}/api/auth/confirm-email?username={Uri.EscapeDataString(request.Username)}&token={urlToken}";
            var emailBody = $"Please confirm your email by clicking the link: {confirmEndpoint}";
            _emailService.Send(request.Email, "Confirm your email", emailBody);



            return Ok("User registered successfully, please check your email to confirm.");
        }
        [HttpGet("confirm-email")]
        public IActionResult ConfirmEmail(string username, string token)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(token))
                return BadRequest("Invalid request");

            var user = _userRepository.GetByUsername(username);
            if (user == null) return NotFound("User not found");

            if (user.EmailConfirmed) return Ok("Email already confirmed");

            if (user.EmailConfirmationTokenHash == null 
                || user.EmailConfirmationTokenHash.Length == 0 
                || user.EmailConfirmationTokenExpiry == null)
                return BadRequest("No confirmation token found");

            if (user.EmailConfirmationTokenExpiry < DateTime.UtcNow)
                return BadRequest("Token expired");

            // decode token
            var tokenBytes = WebEncoders.Base64UrlDecode(token);
            var tokenText = Encoding.UTF8.GetString(tokenBytes);

            var tokenHash = TokenHelper.ComputeSha256HashBytes(tokenText);

            if (!tokenHash.SequenceEqual(user.EmailConfirmationTokenHash))
                return BadRequest("Invalid token");

            // confirm email
            user.EmailConfirmed = true;
            user.EmailConfirmationTokenHash = null;
            user.EmailConfirmationTokenExpiry = null;

            _userRepository.Update(user);
            return Ok("Email confirmed successfully");
        }


        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLoginDto request)
        {
            var user = _userRepository.GetByUsername(request.Username);
            if (user == null) return Unauthorized("User not found");
            
            if (!_userRepository.Login(user,request.Password))
                return Unauthorized("Invalid password");

            if (!user.EmailConfirmed)
                return Unauthorized("Email not confirmed. Please check your email.");

            var token = _jwtService.GenerateToken(user);
            return Ok(new { token });
        }


    }
}
