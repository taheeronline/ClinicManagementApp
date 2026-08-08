// ClinicManagement.Shared/DTOs/PatientRecordDto.cs
using System.ComponentModel.DataAnnotations;

namespace ClinicManagement.Shared.DTOs
{
    public class PatientRecordDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Patient is required.")]
        public int PatientId { get; set; }

        [Required(ErrorMessage = "Appointment is required.")]
        public int AppointmentId { get; set; }

        [Required(ErrorMessage = "Please list the symptoms.")]
        public string Symptoms { get; set; } = string.Empty;

        [Required(ErrorMessage = "Diagnosis is required.")]
        public string Diagnosis { get; set; } = string.Empty;

        public string AdditionalNotes { get; set; } = string.Empty;
        public DateTime DateAdded { get; set; } = DateTime.Now;

        // Display properties for the UI
        public string PatientName { get; set; } = string.Empty;
        public DateTime AppointmentDate { get; set; }
    }
}