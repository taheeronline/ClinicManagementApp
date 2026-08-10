// ClinicManagement.Shared/DTOs/ClinicDto.cs
using System.ComponentModel.DataAnnotations;

namespace ClinicManagement.Shared.DTOs
{
    public class ClinicDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Clinic Name is required.")]
        public string Name { get; set; } = "My Clinic";

        public string Address { get; set; } = string.Empty;
        public string LandlineNumber { get; set; } = string.Empty;
        public string MobileNumber { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; } = string.Empty;

        public string GoogleMapLocation { get; set; } = string.Empty;
    }
}