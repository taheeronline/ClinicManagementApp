using ClinicManagement.Shared.DTOs;
using ClinicManagementApp.Data;
using ClinicManagementApp.Exceptions;
using ClinicManagementApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagementApp.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly ClinicDbContext _context;

        public DoctorService(ClinicDbContext context)
        {
            _context = context;
        }

        public async Task<List<DoctorDto>> GetAllDoctorsAsync()
        {
            return await _context.Doctors
                .Select(d => new DoctorDto
                {
                    Id = d.Id,
                    FirstName = d.FirstName,
                    LastName = d.LastName,
                    Speciality = d.Speciality,
                    ConsultationFee = d.ConsultationFee,
                    PhoneNumber = d.PhoneNumber,
                    LoginName = d.LoginName ?? string.Empty
                })
                .ToListAsync();
        }

        public async Task<DoctorDto> GetDoctorByIdAsync(int id)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor == null) throw new DoctorNotFoundException(id);

            return new DoctorDto
            {
                Id = doctor.Id,
                FirstName = doctor.FirstName,
                LastName = doctor.LastName,
                Speciality = doctor.Speciality,
                ConsultationFee = doctor.ConsultationFee,
                PhoneNumber = doctor.PhoneNumber,
                LoginName = doctor.LoginName ?? string.Empty
                // Notice we do NOT send the PasswordHash back to the client
            };
        }

        public async Task<DoctorDto> CreateDoctorAsync(DoctorDto doctorDto)
        {
            // 1. Check for duplicate usernames
            if (await _context.Doctors.AnyAsync(d => d.LoginName == doctorDto.LoginName))
                throw new InvalidOperationException("This Login Username is already taken.");

            // 2. Ensure password is provided for new doctors
            if (string.IsNullOrWhiteSpace(doctorDto.Password))
                throw new InvalidOperationException("Password is required when creating a new doctor.");

            var doctor = new Doctor
            {
                FirstName = doctorDto.FirstName,
                LastName = doctorDto.LastName,
                Speciality = doctorDto.Speciality,
                ConsultationFee = doctorDto.ConsultationFee,
                PhoneNumber = doctorDto.PhoneNumber,
                IsActive = true,
                LoginName = doctorDto.LoginName,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(doctorDto.Password) // HASH IT!
            };

            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync();

            doctorDto.Id = doctor.Id;
            return doctorDto;
        }

        public async Task UpdateDoctorAsync(int id, DoctorDto doctorDto)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor == null) throw new DoctorNotFoundException(id);

            // 1. Check for duplicate usernames (excluding this specific doctor)
            if (await _context.Doctors.AnyAsync(d => d.LoginName == doctorDto.LoginName && d.Id != id))
                throw new InvalidOperationException("This Login Username is already taken by another doctor.");

            doctor.FirstName = doctorDto.FirstName;
            doctor.LastName = doctorDto.LastName;
            doctor.Speciality = doctorDto.Speciality;
            doctor.ConsultationFee = doctorDto.ConsultationFee;
            doctor.PhoneNumber = doctorDto.PhoneNumber;
            doctor.LoginName = doctorDto.LoginName;

            // 2. Only update the password if a new one was typed into the UI
            if (!string.IsNullOrWhiteSpace(doctorDto.Password))
            {
                doctor.PasswordHash = BCrypt.Net.BCrypt.HashPassword(doctorDto.Password);
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteDoctorAsync(int id)
        {
            bool hasAppointments = await _context.Appointments.AnyAsync(a => a.DoctorId == id);
            if (hasAppointments)
                throw new InvalidOperationException("Cannot delete this doctor because they have appointment history attached to them.");

            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor == null) throw new DoctorNotFoundException(id);

            _context.Doctors.Remove(doctor);
            await _context.SaveChangesAsync();
        }
    }
}