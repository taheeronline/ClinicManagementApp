using System.ComponentModel.DataAnnotations;

namespace ClinicManagement.Shared.DTOs
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Username is required.")]
        public string LoginName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; } = string.Empty;
    }
}