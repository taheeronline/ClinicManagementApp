// ClinicManagement.Shared/DTOs/DoctorDto.cs
using System.ComponentModel.DataAnnotations;

namespace ClinicManagement.Shared.DTOs
{
    public class DoctorDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        [StringLength(100, ErrorMessage = "First Name cannot exceed 100 characters")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last Name is required")]
        [StringLength(100, ErrorMessage = "Last Name cannot exceed 100 characters")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Speciality is required")]
        [StringLength(100, ErrorMessage = "Speciality cannot exceed 100 characters")]
        public string Speciality { get; set; } = string.Empty;

        [Required(ErrorMessage = "Consultation fee is required")]
        [Range(0, 100000, ErrorMessage = "Fee must be a positive value")]
        public decimal ConsultationFee { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [StringLength(20, ErrorMessage = "Phone number cannot exceed 20 characters")]
        [Phone(ErrorMessage = "Invalid phone number format")]
        public string PhoneNumber { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public string FullName => $"{FirstName} {LastName}";
    }
}