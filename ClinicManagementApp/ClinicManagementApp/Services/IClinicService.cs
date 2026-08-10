// ClinicManagementApp/Services/IClinicService.cs
using ClinicManagement.Shared.DTOs;

namespace ClinicManagementApp.Services
{
    public interface IClinicService
    {
        Task<ClinicDto> GetClinicDetailsAsync();
        Task UpdateClinicDetailsAsync(ClinicDto clinicDto);
    }
}