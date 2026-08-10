// ClinicManagement.Shared/DTOs/ConsultationBillDto.cs
namespace ClinicManagement.Shared.DTOs
{
    public class ConsultationBillDto
    {
        public int Id { get; set; }
        public int AppointmentId { get; set; }

        public decimal ConsultationCharge { get; set; }
        public bool IsPaid { get; set; }
        public DateTime BillDate { get; set; }

        // Read-only display properties for the UI
        public string PatientName { get; set; } = string.Empty;
        public string DoctorName { get; set; } = string.Empty;
        public string DoctorSpeciality { get; set; } = string.Empty;
    }
}