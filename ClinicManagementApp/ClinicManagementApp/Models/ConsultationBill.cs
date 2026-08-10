// ClinicManagementApp/Models/ConsultationBill.cs
using Microsoft.EntityFrameworkCore;

namespace ClinicManagementApp.Models
{
    public class ConsultationBill
    {
        public int Id { get; set; }

        public int AppointmentId { get; set; }
        public Appointment? Appointment { get; set; }

        [Precision(18, 2)]
        public decimal ConsultationCharge { get; set; }

        public bool IsPaid { get; set; } = false;

        public DateTime BillDate { get; set; } = DateTime.UtcNow;
    }
}