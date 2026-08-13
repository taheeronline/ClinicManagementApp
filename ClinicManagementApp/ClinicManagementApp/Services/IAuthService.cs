// IAuthService.cs
using ClinicManagement.Shared.DTOs;

namespace ClinicManagementApp.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
    }
}