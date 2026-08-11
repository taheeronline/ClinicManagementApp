using ClinicManagement.Shared.DTOs;

namespace ClinicManagementApp.Services
{
    public interface IPatientRecordService
    {
        Task<List<PatientRecordDto>> GetAllRecordsAsync();
        Task<PatientRecordDto> GetRecordByIdAsync(int id);
        Task<List<PatientRecordDto>> GetRecordsByPatientIdAsync(int patientId); 
        Task<PatientRecordDetailsDto> GetRecordDetailsAsync(int id);
        Task<PatientRecordDto> CreateRecordAsync(PatientRecordDto recordDto);
        //Task UpdateRecordAsync(int id, PatientRecordDto recordDto);
        //Task DeleteRecordAsync(int id);
        Task CompleteConsultationAsync(ConsultationSaveDto dto);
    }
}