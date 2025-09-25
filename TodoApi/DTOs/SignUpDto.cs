using System.ComponentModel.DataAnnotations;

namespace TodoApi.DTOs
{
    public class SignUpDto
    {
        [Required(ErrorMessage = "Username is required.")]
        [MaxLength(100)]
        public string UserName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Email is required.")]
        [MaxLength(256)]
        public string Email { get; set; } = string.Empty;
        [Required(ErrorMessage = "Password is required")]
        [StringLength(8, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters")]
        public string Password { get; set; } = string.Empty;
    }

    public class SignUpResponseDto
    {
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }

    public class LoginDto
    {
        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; } = string.Empty;
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = string.Empty;
    }

    public class LoginResponseDto
    {
        public string Email { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }
}
