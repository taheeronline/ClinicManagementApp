// ClinicManagement.Shared/DTOs/UserDto.cs
using System.ComponentModel.DataAnnotations;

namespace ClinicManagement.Shared.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Login Username is required")]
        [StringLength(50)]
        public string LoginName { get; set; } = string.Empty;

        [StringLength(200)]
        public string? Address { get; set; }

        [StringLength(20)]
        public string? Phone { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email format")]
        [StringLength(100)]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Role is required")]
        public string Role { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; }
        public DateTime? LastLogin { get; set; }

        // Used only for UI input to transfer the password to the server.
        public string? Password { get; set; }
    }
}