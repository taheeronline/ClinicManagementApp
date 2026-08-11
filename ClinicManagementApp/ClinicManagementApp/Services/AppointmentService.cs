using ClinicManagementApp.Data;
using ClinicManagementApp.Exceptions;
using ClinicManagementApp.Models;
using ClinicManagement.Shared.DTOs;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagementApp.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly ClinicDbContext _context;

        public AppointmentService(ClinicDbContext context)
        {
            _context = context;
        }

        public async Task<List<AppointmentDto>> GetAllAppointmentsAsync()
        {
            // Note the .Include() to fetch related Doctor and Patient data
            return await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Select(a => new AppointmentDto
                {
                    Id = a.Id,
                    PatientId = a.PatientId,
                    DoctorId = a.DoctorId,
                    AppointmentDate = a.AppointmentDate,
                    Status = a.Status,
                    ReasonForVisit = a.ReasonForVisit,
                    PatientName = $"{a.Patient!.FirstName} {a.Patient.LastName}",
                    DoctorName = $"{a.Doctor!.FirstName} {a.Doctor.LastName}"
                })
                .OrderByDescending(a => a.AppointmentDate) // Show newest/upcoming first
                .ToListAsync();
        }

        public async Task<AppointmentDto> GetAppointmentByIdAsync(int id)
        {
            var appointment = await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (appointment == null) throw new AppointmentNotFoundException(id);

            return new AppointmentDto
            {
                Id = appointment.Id,
                PatientId = appointment.PatientId,
                DoctorId = appointment.DoctorId,
                AppointmentDate = appointment.AppointmentDate,
                Status = appointment.Status,
                ReasonForVisit = appointment.ReasonForVisit,
                PatientName = $"{appointment.Patient!.FirstName} {appointment.Patient.LastName}",
                DoctorName = $"{appointment.Doctor!.FirstName} {appointment.Doctor.LastName}"
            };
        }

        public async Task<AppointmentDto> CreateAppointmentAsync(AppointmentDto appointmentDto)
        {
            await ValidateDoubleBookingAsync(appointmentDto.DoctorId, appointmentDto.AppointmentDate, 0); // 0 means new appointment

            var appointment = new Appointment
            {
                PatientId = appointmentDto.PatientId,
                DoctorId = appointmentDto.DoctorId,
                AppointmentDate = appointmentDto.AppointmentDate,
                Status = ClinicManagement.Shared.Enums.AppointmentStatus.Scheduled,
                ReasonForVisit = appointmentDto.ReasonForVisit
            };

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            // Populate the generated ID and Status back into the DTO to return it
            appointmentDto.Id = appointment.Id;
            appointmentDto.Status = appointment.Status;

            return appointmentDto;
        }

        // Inside AppointmentService.cs

        public async Task CancelAppointmentAsync(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null) throw new AppointmentNotFoundException(id);

            if (appointment.Status != ClinicManagement.Shared.Enums.AppointmentStatus.Scheduled)
            {
                throw new InvalidOperationException("Only pending (Scheduled) appointments can be cancelled.");
            }

            appointment.Status = ClinicManagement.Shared.Enums.AppointmentStatus.Cancelled;
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAppointmentAsync(int id, AppointmentDto appointmentDto)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null) throw new AppointmentNotFoundException(id);

            // --- UPDATED BUSINESS RULE #1: IMMUTABILITY ---
            // Now it blocks anything that IS NOT Scheduled
            if (appointment.Status != ClinicManagement.Shared.Enums.AppointmentStatus.Scheduled)
            {
                throw new InvalidOperationException("Cannot modify this appointment. Cancelled, No-Show, Completed, and Closed appointments are permanently locked.");
            }
            // ----------------------------------------------

            await ValidateDoubleBookingAsync(appointmentDto.DoctorId, appointmentDto.AppointmentDate, id);

            appointment.PatientId = appointmentDto.PatientId;
            appointment.DoctorId = appointmentDto.DoctorId;
            appointment.AppointmentDate = appointmentDto.AppointmentDate;
            // Note: We don't update Status here to prevent bypassing the lifecycle. Status changes should happen via specific actions (Cancel, Complete, etc.)
            appointment.ReasonForVisit = appointmentDto.ReasonForVisit;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAppointmentAsync(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null) throw new AppointmentNotFoundException(id);

            // --- UPDATED BUSINESS RULE #1: IMMUTABILITY ---
            if (appointment.Status != ClinicManagement.Shared.Enums.AppointmentStatus.Scheduled)
            {
                throw new InvalidOperationException("Cannot delete this appointment. Cancelled, No-Show, Completed, and Closed appointments are permanently locked.");
            }
            // ----------------------------------------------

            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();
        }

        // --- ADD THIS HELPER METHOD TO THE CLASS ---
        private async Task ValidateDoubleBookingAsync(int doctorId, DateTime requestedTime, int currentAppointmentId)
        {
            // Define the blocked 30-minute window
            var bufferStart = requestedTime.AddMinutes(-30);
            var bufferEnd = requestedTime.AddMinutes(30);

            bool isDoubleBooked = await _context.Appointments
                .Where(a => a.DoctorId == doctorId
                         && a.Id != currentAppointmentId // Ignore itself during an update
                         && a.Status != ClinicManagement.Shared.Enums.AppointmentStatus.Cancelled
                         && a.Status != ClinicManagement.Shared.Enums.AppointmentStatus.NoShow)
                .AnyAsync(a => a.AppointmentDate > bufferStart && a.AppointmentDate < bufferEnd);

            if (isDoubleBooked)
            {
                throw new InvalidOperationException("This doctor is already booked during that time slot. Please select a time at least 30 minutes before or after.");
            }
        }

        public async Task<int> MarkOverdueAsNoShowAsync()
        {
            // Define the threshold (e.g., 2 hours in the past)
            var overdueTime = DateTime.Now.AddHours(-2);

            var overdueAppointments = await _context.Appointments
                .Where(a => a.Status == ClinicManagement.Shared.Enums.AppointmentStatus.Scheduled
                         && a.AppointmentDate < overdueTime)
                .ToListAsync();

            if (!overdueAppointments.Any()) return 0;

            foreach (var appt in overdueAppointments)
            {
                appt.Status = ClinicManagement.Shared.Enums.AppointmentStatus.NoShow;
            }

            await _context.SaveChangesAsync();

            // Return the number of appointments that were updated
            return overdueAppointments.Count;
        }
    }
}