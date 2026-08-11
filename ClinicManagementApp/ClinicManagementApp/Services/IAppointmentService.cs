using ClinicManagement.Shared.DTOs;

namespace ClinicManagementApp.Services
{
    public interface IAppointmentService
    {
        Task<List<AppointmentDto>> GetAllAppointmentsAsync();
        Task<AppointmentDto> GetAppointmentByIdAsync(int id);
        Task<AppointmentDto> CreateAppointmentAsync(AppointmentDto appointmentDto);
        Task UpdateAppointmentAsync(int id, AppointmentDto appointmentDto);
        Task DeleteAppointmentAsync(int id);
        Task<int> MarkOverdueAsNoShowAsync();
    }
}