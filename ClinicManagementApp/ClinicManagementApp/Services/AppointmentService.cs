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
            var appointment = new Appointment
            {
                PatientId = appointmentDto.PatientId,
                DoctorId = appointmentDto.DoctorId,
                AppointmentDate = appointmentDto.AppointmentDate,
                Status = appointmentDto.Status,
                ReasonForVisit = appointmentDto.ReasonForVisit
            };

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            appointmentDto.Id = appointment.Id;
            return appointmentDto;
        }

        public async Task UpdateAppointmentAsync(int id, AppointmentDto appointmentDto)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null) throw new AppointmentNotFoundException(id);

            appointment.PatientId = appointmentDto.PatientId;
            appointment.DoctorId = appointmentDto.DoctorId;
            appointment.AppointmentDate = appointmentDto.AppointmentDate;
            appointment.Status = appointmentDto.Status;
            appointment.ReasonForVisit = appointmentDto.ReasonForVisit;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAppointmentAsync(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null) throw new AppointmentNotFoundException(id);

            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();
        }
    }
}