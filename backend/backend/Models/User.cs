using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class User
    {
        public int Id { get; set; }
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public byte[] PasswordHash { get; set; } = Array.Empty<byte>();
        public byte[] PasswordSalt { get; set; } = Array.Empty<byte>();
        public string Role { get; set; } = "User"; // User / Admin
        public virtual List<Plan>? Plans { get; set; }
    }
}
