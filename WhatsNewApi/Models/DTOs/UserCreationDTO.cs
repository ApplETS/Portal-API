using WhatsNewApi.Models.Enums;

namespace WhatsNewApi.Models.DTOs
{
    public class UserCreationDTO
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? PasswordConfirmation { get; set; }
        public string Role { get; set; } = "Member"; // Default role is member
    }
}
