// ClinicManagementApp/Services/IPatientRecordService.cs
using ClinicManagement.Shared.DTOs;

namespace ClinicManagementApp.Services
{
    public interface IPatientRecordService
    {
        Task<List<PatientRecordDto>> GetAllRecordsAsync();
        Task<PatientRecordDto> GetRecordByIdAsync(int id);
        Task<List<PatientRecordDto>> GetRecordsByPatientIdAsync(int patientId); // Useful for viewing a patient's history
        Task<PatientRecordDto> CreateRecordAsync(PatientRecordDto recordDto);
        Task UpdateRecordAsync(int id, PatientRecordDto recordDto);
        Task DeleteRecordAsync(int id);
        Task CompleteConsultationAsync(ConsultationSaveDto dto);
    }
}