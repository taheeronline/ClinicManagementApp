// ClinicManagementApp/Models/Prescription.cs
namespace ClinicManagementApp.Models
{
    public class Prescription
    {
        public int Id { get; set; }

        public int AppointmentId { get; set; }
        public Appointment? Appointment { get; set; }

        public string MedicineName { get; set; } = string.Empty;
        public string Dosage { get; set; } = string.Empty; // e.g., 500mg
        public string Duration { get; set; } = string.Empty; // e.g., 5 Days
        public string Instructions { get; set; } = string.Empty; // e.g., After meals
    }
}