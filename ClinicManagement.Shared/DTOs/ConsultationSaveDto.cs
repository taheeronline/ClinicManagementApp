// ClinicManagement.Shared/DTOs/ConsultationSaveDto.cs
using System.ComponentModel.DataAnnotations;

namespace ClinicManagement.Shared.DTOs
{
    public class PrescriptionItemDto
    {
        [Required(ErrorMessage = "Medicine name is required")]
        public string MedicineName { get; set; } = string.Empty;

        [Required]
        public string Dosage { get; set; } = string.Empty;

        [Required]
        public string Duration { get; set; } = string.Empty;

        public string Instructions { get; set; } = string.Empty;
    }

    public class ConsultationSaveDto
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please select an appointment.")]
        public int AppointmentId { get; set; }

        public int PatientId { get; set; }

        [Required(ErrorMessage = "Symptoms are required")]
        public string Symptoms { get; set; } = string.Empty;

        [Required(ErrorMessage = "Diagnosis is required")]
        public string Diagnosis { get; set; } = string.Empty;

        public string AdditionalNotes { get; set; } = string.Empty;

        // The dynamic list of medicines
        public List<PrescriptionItemDto> Prescriptions { get; set; } = new();
    }
}