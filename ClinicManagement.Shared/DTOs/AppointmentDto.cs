using ClinicManagement.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace ClinicManagement.Shared.DTOs
{
    public class AppointmentDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please select a patient.")]
        public int PatientId { get; set; }

        [Required(ErrorMessage = "Please select a doctor.")]
        public int DoctorId { get; set; }

        [Required(ErrorMessage = "Appointment date and time is required.")]
        public DateTime AppointmentDate { get; set; } = DateTime.Now.AddDays(1); // Default to tomorrow

        public AppointmentStatus Status { get; set; } = AppointmentStatus.Scheduled;

        [StringLength(500, ErrorMessage = "Reason cannot exceed 500 characters.")]
        public string ReasonForVisit { get; set; } = string.Empty;

        // Display properties for the UI List view (Read-Only)
        public string PatientName { get; set; } = string.Empty;
        public string DoctorName { get; set; } = string.Empty;
    }
}