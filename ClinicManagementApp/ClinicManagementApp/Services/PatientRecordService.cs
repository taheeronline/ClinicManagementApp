// ClinicManagementApp/Services/PatientRecordService.cs
using ClinicManagement.Shared.DTOs;
using ClinicManagementApp.Data;
using ClinicManagementApp.Exceptions;
using ClinicManagementApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagementApp.Services
{
    public class PatientRecordService : IPatientRecordService
    {
        private readonly ClinicDbContext _context;

        public PatientRecordService(ClinicDbContext context)
        {
            _context = context;
        }

        public async Task<List<PatientRecordDto>> GetAllRecordsAsync()
        {
            return await _context.PatientRecords
                .Include(pr => pr.Patient)
                .Include(pr => pr.Appointment)
                .Select(pr => new PatientRecordDto
                {
                    Id = pr.Id,
                    PatientId = pr.PatientId,
                    AppointmentId = pr.AppointmentId,
                    Symptoms = pr.Symptoms,
                    Diagnosis = pr.Diagnosis,
                    AdditionalNotes = pr.AdditionalNotes,
                    DateAdded = pr.DateAdded,
                    PatientName = $"{pr.Patient!.FirstName} {pr.Patient.LastName}",
                    AppointmentDate = pr.Appointment!.AppointmentDate
                })
                .OrderByDescending(pr => pr.DateAdded)
                .ToListAsync();
        }

        public async Task<PatientRecordDto> GetRecordByIdAsync(int id)
        {
            var record = await _context.PatientRecords
                .Include(pr => pr.Patient)
                .Include(pr => pr.Appointment)
                .FirstOrDefaultAsync(pr => pr.Id == id);

            if (record == null) throw new PatientRecordNotFoundException(id);

            return new PatientRecordDto
            {
                Id = record.Id,
                PatientId = record.PatientId,
                AppointmentId = record.AppointmentId,
                Symptoms = record.Symptoms,
                Diagnosis = record.Diagnosis,
                AdditionalNotes = record.AdditionalNotes,
                DateAdded = record.DateAdded,
                PatientName = $"{record.Patient!.FirstName} {record.Patient.LastName}",
                AppointmentDate = record.Appointment!.AppointmentDate
            };
        }

        public async Task<List<PatientRecordDto>> GetRecordsByPatientIdAsync(int patientId)
        {
            return await _context.PatientRecords
               .Include(pr => pr.Appointment)
               .Where(pr => pr.PatientId == patientId)
               .Select(pr => new PatientRecordDto
               {
                   Id = pr.Id,
                   PatientId = pr.PatientId,
                   AppointmentId = pr.AppointmentId,
                   Symptoms = pr.Symptoms,
                   Diagnosis = pr.Diagnosis,
                   AdditionalNotes = pr.AdditionalNotes,
                   DateAdded = pr.DateAdded,
                   AppointmentDate = pr.Appointment!.AppointmentDate
               })
               .OrderByDescending(pr => pr.DateAdded)
               .ToListAsync();
        }

        public async Task<PatientRecordDetailsDto> GetRecordDetailsAsync(int id)
        {
            var record = await _context.PatientRecords
                .Include(pr => pr.Patient)
                .Include(pr => pr.Appointment).ThenInclude(a => a!.Doctor)
                .FirstOrDefaultAsync(pr => pr.Id == id);

            if (record == null) throw new Exception("Record not found.");

            // Fetch the attached prescriptions based on the AppointmentId
            var prescriptions = await _context.Prescriptions
                .Where(p => p.AppointmentId == record.AppointmentId)
                .Select(p => new PrescriptionItemDto
                {
                    MedicineName = p.MedicineName,
                    Dosage = p.Dosage,
                    Duration = p.Duration,
                    Instructions = p.Instructions
                }).ToListAsync();

            return new PatientRecordDetailsDto
            {
                Id = record.Id,
                DateAdded = record.DateAdded,
                PatientName = $"{record.Patient!.FirstName} {record.Patient.LastName}",
                DoctorName = $"{record.Appointment!.Doctor!.FirstName} {record.Appointment.Doctor.LastName}",
                Symptoms = record.Symptoms,
                Diagnosis = record.Diagnosis,
                AdditionalNotes = record.AdditionalNotes,
                Prescriptions = prescriptions
            };
        }

        public async Task<PatientRecordDto> CreateRecordAsync(PatientRecordDto recordDto)
        {
            var record = new PatientRecord
            {
                PatientId = recordDto.PatientId,
                AppointmentId = recordDto.AppointmentId,
                Symptoms = recordDto.Symptoms,
                Diagnosis = recordDto.Diagnosis,
                AdditionalNotes = recordDto.AdditionalNotes,
                DateAdded = DateTime.UtcNow
            };

            _context.PatientRecords.Add(record);
            await _context.SaveChangesAsync();

            recordDto.Id = record.Id;
            return recordDto;
        }

        //public async Task UpdateRecordAsync(int id, PatientRecordDto recordDto)
        //{
        //    var record = await _context.PatientRecords.FindAsync(id);
        //    if (record == null) throw new PatientRecordNotFoundException(id);

        //    record.PatientId = recordDto.PatientId;
        //    record.AppointmentId = recordDto.AppointmentId;
        //    record.Symptoms = recordDto.Symptoms;
        //    record.Diagnosis = recordDto.Diagnosis;
        //    record.AdditionalNotes = recordDto.AdditionalNotes;

        //    await _context.SaveChangesAsync();
        //}

        //public async Task DeleteRecordAsync(int id)
        //{
        //    var record = await _context.PatientRecords.FindAsync(id);
        //    if (record == null) throw new PatientRecordNotFoundException(id);

        //    _context.PatientRecords.Remove(record);
        //    await _context.SaveChangesAsync();
        //}

        public async Task CompleteConsultationAsync(ConsultationSaveDto dto)
        {
            // 1. Save the Patient Record
            var record = new PatientRecord
            {
                AppointmentId = dto.AppointmentId,
                PatientId = dto.PatientId,
                Symptoms = dto.Symptoms,
                Diagnosis = dto.Diagnosis,
                AdditionalNotes = dto.AdditionalNotes,
                DateAdded = DateTime.UtcNow
            };
            _context.PatientRecords.Add(record);

            // 2. Save all Prescriptions
            foreach (var med in dto.Prescriptions)
            {
                _context.Prescriptions.Add(new Prescription
                {
                    AppointmentId = dto.AppointmentId,
                    MedicineName = med.MedicineName,
                    Dosage = med.Dosage,
                    Duration = med.Duration,
                    Instructions = med.Instructions
                });
            }

            // 3. Update the Appointment Status to Completed (Ready for Billing)
            var appointment = await _context.Appointments.FindAsync(dto.AppointmentId);
            if (appointment != null)
            {
                appointment.Status = ClinicManagement.Shared.Enums.AppointmentStatus.Completed;
            }

            // Save everything in one transaction!
            await _context.SaveChangesAsync();
        }
    }
}