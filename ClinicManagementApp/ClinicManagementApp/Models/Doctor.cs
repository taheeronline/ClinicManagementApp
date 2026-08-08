using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ClinicManagementApp.Models
{
    public class Doctor
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;
        [Required]
        [MaxLength(100)]
        public string Speciality { get; set; } = string.Empty;
        [Required]
        [Precision(18, 2)] // Add this line
        public decimal ConsultationFee { get; set; }

        [Required]
        [MaxLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }
}