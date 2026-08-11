// ClinicManagementApp/Services/DoctorService.cs
using ClinicManagementApp.Data;
using ClinicManagementApp.Exceptions;
using ClinicManagementApp.Models;
using ClinicManagement.Shared.DTOs;
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
                    PhoneNumber = d.PhoneNumber
                })
                .ToListAsync();
        }

        public async Task<DoctorDto> GetDoctorByIdAsync(int id)
        {
            var doctor = await _context.Doctors.FindAsync(id);

            if (doctor == null)
            {
                throw new DoctorNotFoundException(id); // Triggers our custom exception
            }

            return new DoctorDto
            {
                Id = doctor.Id,
                FirstName = doctor.FirstName,
                LastName = doctor.LastName,
                Speciality = doctor.Speciality,
                ConsultationFee = doctor.ConsultationFee,
                PhoneNumber = doctor.PhoneNumber
            };
        }

        public async Task<DoctorDto> CreateDoctorAsync(DoctorDto doctorDto)
        {
            var doctor = new Doctor
            {
                FirstName = doctorDto.FirstName,
                LastName = doctorDto.LastName,
                Speciality = doctorDto.Speciality,
                ConsultationFee = doctorDto.ConsultationFee,
                PhoneNumber = doctorDto.PhoneNumber,
                IsActive = true
            };

            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync();

            doctorDto.Id = doctor.Id;
            return doctorDto;
        }

        public async Task UpdateDoctorAsync(int id, DoctorDto doctorDto)
        {
            var doctor = await _context.Doctors.FindAsync(id);

            if (doctor == null)
            {
                throw new DoctorNotFoundException(id);
            }

            doctor.FirstName = doctorDto.FirstName;
            doctor.LastName = doctorDto.LastName;
            doctor.Speciality = doctorDto.Speciality;
            doctor.ConsultationFee = doctorDto.ConsultationFee;
            doctor.PhoneNumber = doctorDto.PhoneNumber;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteDoctorAsync(int id)
        {
            // --- BUSINESS RULE #3: REFERENTIAL INTEGRITY ---
            bool hasAppointments = await _context.Appointments.AnyAsync(a => a.DoctorId == id);
            if (hasAppointments)
            {
                throw new InvalidOperationException("Cannot delete this doctor because they have appointment history attached to them.");
            }
            // -----------------------------------------------

            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor == null) throw new DoctorNotFoundException(id);

            _context.Doctors.Remove(doctor);
            await _context.SaveChangesAsync();
        }
    }
}