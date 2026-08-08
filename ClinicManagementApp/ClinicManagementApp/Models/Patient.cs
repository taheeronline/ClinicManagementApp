// ClinicManagementApp/Models/Patient.cs
namespace ClinicManagementApp.Models
{
    public class Patient
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; } = string.Empty;
        public string ContactNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string MedicalHistory { get; set; } = string.Empty; // Brief notes on allergies, past conditions, etc.
        public DateTime RegisteredDate { get; set; } = DateTime.UtcNow;
    }
}