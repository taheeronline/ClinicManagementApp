// ClinicManagementApp/Services/BillService.cs
using ClinicManagementApp.Data;
using ClinicManagementApp.Exceptions;
using ClinicManagementApp.Models;
using ClinicManagement.Shared.DTOs;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagementApp.Services
{
    public class BillService : IBillService
    {
        private readonly ClinicDbContext _context;

        public BillService(ClinicDbContext context)
        {
            _context = context;
        }

        public async Task<List<ConsultationBillDto>> GetAllBillsAsync()
        {
            return await _context.ConsultationBills
                .Include(b => b.Appointment)
                    .ThenInclude(a => a!.Patient)
                .Include(b => b.Appointment)
                    .ThenInclude(a => a!.Doctor)
                .Select(b => new ConsultationBillDto
                {
                    Id = b.Id,
                    AppointmentId = b.AppointmentId,
                    ConsultationCharge = b.ConsultationCharge,
                    IsPaid = b.IsPaid,
                    BillDate = b.BillDate,
                    PatientName = $"{b.Appointment!.Patient!.FirstName} {b.Appointment.Patient.LastName}",
                    DoctorName = $"{b.Appointment.Doctor!.FirstName} {b.Appointment.Doctor.LastName}",
                    DoctorSpeciality = b.Appointment.Doctor.Speciality
                })
                .OrderByDescending(b => b.BillDate)
                .ToListAsync();
        }

        public async Task<ConsultationBillDto> GenerateBillAsync(int appointmentId)
        {
            // 1. Find the appointment and the associated doctor
            var appointment = await _context.Appointments
                .Include(a => a.Doctor)
                .FirstOrDefaultAsync(a => a.Id == appointmentId);

            if (appointment == null) throw new AppointmentNotFoundException(appointmentId);

            // 2. Check if a bill already exists to prevent duplicates
            var existingBill = await _context.ConsultationBills.AnyAsync(b => b.AppointmentId == appointmentId);
            if (existingBill) throw new Exception("A bill already exists for this appointment.");

            // 3. Create the bill, locking in the doctor's current fee
            var bill = new ConsultationBill
            {
                AppointmentId = appointmentId,
                ConsultationCharge = appointment.Doctor!.ConsultationFee, // Auto-pulled based on speciality/doctor
                IsPaid = false,
                BillDate = DateTime.UtcNow
            };

            _context.ConsultationBills.Add(bill);
            await _context.SaveChangesAsync();

            return await GetBillDtoByIdAsync(bill.Id); // Helper method (below) to return full display data
        }

        public async Task MarkAsPaidAsync(int billId)
        {
            var bill = await _context.ConsultationBills
                .Include(b => b.Appointment)
                .FirstOrDefaultAsync(b => b.Id == billId);

            if (bill == null) throw new BillNotFoundException(billId);

            // 1. Mark bill as paid
            bill.IsPaid = true;

            // 2. Close the appointment! 
            if (bill.Appointment != null)
            {
                bill.Appointment.Status = ClinicManagement.Shared.Enums.AppointmentStatus.Closed;
            }

            await _context.SaveChangesAsync();
        }

        // Private helper to format the return object
        private async Task<ConsultationBillDto> GetBillDtoByIdAsync(int id)
        {
            var b = await _context.ConsultationBills
                .Include(x => x.Appointment).ThenInclude(a => a!.Patient)
                .Include(x => x.Appointment).ThenInclude(a => a!.Doctor)
                .FirstAsync(x => x.Id == id);

            return new ConsultationBillDto
            {
                Id = b.Id,
                AppointmentId = b.AppointmentId,
                ConsultationCharge = b.ConsultationCharge,
                IsPaid = b.IsPaid,
                BillDate = b.BillDate,
                PatientName = $"{b.Appointment!.Patient!.FirstName} {b.Appointment.Patient.LastName}",
                DoctorName = $"{b.Appointment.Doctor!.FirstName} {b.Appointment.Doctor.LastName}",
                DoctorSpeciality = b.Appointment.Doctor.Speciality
            };
        }
    }
}