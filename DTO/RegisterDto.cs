namespace HotelAPI.DTOs
{
    public class RegisterDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty; // plain text lozinka
        public string Role { get; set; } = "User"; // default role
    }
}
