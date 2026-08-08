using ClinicManagement.Shared.Enums;

namespace ClinicManagementApp.Models
{
    public class Appointment
    {
        public int Id { get; set; }

        // Foreign Key to Patient
        public int PatientId { get; set; }
        public Patient? Patient { get; set; }

        // Foreign Key to Doctor
        public int DoctorId { get; set; }
        public Doctor? Doctor { get; set; }

        public DateTime AppointmentDate { get; set; }

        public AppointmentStatus Status { get; set; } = AppointmentStatus.Scheduled;

        public string ReasonForVisit { get; set; } = string.Empty;
    }
}