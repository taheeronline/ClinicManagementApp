// ClinicManagement.Shared/DTOs/PatientDto.cs
using System.ComponentModel.DataAnnotations;

namespace ClinicManagement.Shared.DTOs
{
    public class PatientDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Date of Birth is required")]
        public DateTime DateOfBirth { get; set; } = DateTime.Today;

        [Required]
        public string Gender { get; set; } = string.Empty;

        [Required]
        [Phone]
        public string ContactNumber { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public string MedicalHistory { get; set; } = string.Empty;

        public string FullName => $"{FirstName} {LastName}";

        // Calculate age on the fly for the UI
        public int Age => DateTime.Today.Year - DateOfBirth.Year -
            (DateTime.Today.DayOfYear < DateOfBirth.DayOfYear ? 1 : 0);
    }
}