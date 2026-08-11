// ClinicManagementApp/Services/PatientService.cs
using ClinicManagementApp.Data;
using ClinicManagementApp.Exceptions;
using ClinicManagementApp.Models;
using ClinicManagement.Shared.DTOs;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagementApp.Services
{
    public class PatientService : IPatientService
    {
        private readonly ClinicDbContext _context;

        public PatientService(ClinicDbContext context)
        {
            _context = context;
        }

        public async Task<List<PatientDto>> GetAllPatientsAsync()
        {
            return await _context.Patients
                .Select(p => new PatientDto
                {
                    Id = p.Id,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    DateOfBirth = p.DateOfBirth,
                    Gender = p.Gender,
                    ContactNumber = p.ContactNumber,
                    Address = p.Address,
                    MedicalHistory = p.MedicalHistory
                })
                .ToListAsync();
        }

        public async Task<PatientDto> GetPatientByIdAsync(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null) throw new PatientNotFoundException(id);

            return new PatientDto
            {
                Id = patient.Id,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                DateOfBirth = patient.DateOfBirth,
                Gender = patient.Gender,
                ContactNumber = patient.ContactNumber,
                Address = patient.Address,
                MedicalHistory = patient.MedicalHistory
            };
        }

        public async Task<PatientDto> CreatePatientAsync(PatientDto patientDto)
        {
            var patient = new Patient
            {
                FirstName = patientDto.FirstName,
                LastName = patientDto.LastName,
                DateOfBirth = patientDto.DateOfBirth,
                Gender = patientDto.Gender,
                ContactNumber = patientDto.ContactNumber,
                Address = patientDto.Address,
                MedicalHistory = patientDto.MedicalHistory,
                RegisteredDate = DateTime.UtcNow
            };

            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();

            patientDto.Id = patient.Id;
            return patientDto;
        }

        public async Task UpdatePatientAsync(int id, PatientDto patientDto)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null) throw new PatientNotFoundException(id);

            patient.FirstName = patientDto.FirstName;
            patient.LastName = patientDto.LastName;
            patient.DateOfBirth = patientDto.DateOfBirth;
            patient.Gender = patientDto.Gender;
            patient.ContactNumber = patientDto.ContactNumber;
            patient.Address = patientDto.Address;
            patient.MedicalHistory = patientDto.MedicalHistory;

            await _context.SaveChangesAsync();
        }

        public async Task DeletePatientAsync(int id)
        {
            // --- BUSINESS RULE #3: REFERENTIAL INTEGRITY ---
            // Check if they have appointments OR medical records
            bool hasAppointments = await _context.Appointments.AnyAsync(a => a.PatientId == id);
            bool hasRecords = await _context.PatientRecords.AnyAsync(pr => pr.PatientId == id);

            if (hasAppointments || hasRecords)
            {
                throw new InvalidOperationException("Cannot delete this patient. Data retention laws require keeping their medical history.");
            }
            // -----------------------------------------------

            var patient = await _context.Patients.FindAsync(id);
            if (patient == null) throw new PatientNotFoundException(id);

            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();
        }
    }
}