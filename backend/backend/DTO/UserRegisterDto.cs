using System.ComponentModel.DataAnnotations;

namespace backend.DTO
{
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
