// ClinicManagementApp/Services/IPatientService.cs
using ClinicManagement.Shared.DTOs;

namespace ClinicManagementApp.Services
{
    public interface IPatientService
    {
        Task<List<PatientDto>> GetAllPatientsAsync();
        Task<PatientDto> GetPatientByIdAsync(int id);
        Task<PatientDto> CreatePatientAsync(PatientDto patientDto);
        Task UpdatePatientAsync(int id, PatientDto patientDto);
        Task DeletePatientAsync(int id);
    }
}