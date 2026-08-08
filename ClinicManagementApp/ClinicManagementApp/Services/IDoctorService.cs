// ClinicManagementApp/Services/IDoctorService.cs
using ClinicManagement.Shared.DTOs;

namespace ClinicManagementApp.Services
{
    public interface IDoctorService
    {
        Task<List<DoctorDto>> GetAllDoctorsAsync();
        Task<DoctorDto> GetDoctorByIdAsync(int id);
        Task<DoctorDto> CreateDoctorAsync(DoctorDto doctorDto);
        Task UpdateDoctorAsync(int id, DoctorDto doctorDto);
        Task DeleteDoctorAsync(int id);
    }
}