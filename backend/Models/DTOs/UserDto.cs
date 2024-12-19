namespace backendCRM.Models.DTOs
{
    public class RegisterDto
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
    }

    public class LoginDto
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }

    public class UserDto
    {
        public long Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
    }
}
