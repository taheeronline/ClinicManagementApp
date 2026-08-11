// ClinicManagement.Shared/DTOs/PatientRecordDetailsDto.cs
namespace ClinicManagement.Shared.DTOs
{
    public class PatientRecordDetailsDto
    {
        public int Id { get; set; }
        public DateTime DateAdded { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public string DoctorName { get; set; } = string.Empty;
        public string Symptoms { get; set; } = string.Empty;
        public string Diagnosis { get; set; } = string.Empty;
        public string AdditionalNotes { get; set; } = string.Empty;

        // Include the locked prescriptions
        public List<PrescriptionItemDto> Prescriptions { get; set; } = new();
    }
}