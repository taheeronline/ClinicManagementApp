// ClinicManagementApp/Models/PatientRecord.cs
namespace ClinicManagementApp.Models
{
    public class PatientRecord
    {
        public int Id { get; set; }

        // Link to the Patient
        public int PatientId { get; set; }
        public Patient? Patient { get; set; }

        // Link to the specific Appointment
        public int AppointmentId { get; set; }
        public Appointment? Appointment { get; set; }

        public string Symptoms { get; set; } = string.Empty;
        public string Diagnosis { get; set; } = string.Empty;
        public string AdditionalNotes { get; set; } = string.Empty;

        public DateTime DateAdded { get; set; } = DateTime.UtcNow;
    }
}