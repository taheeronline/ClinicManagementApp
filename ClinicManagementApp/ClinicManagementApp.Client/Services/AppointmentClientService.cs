    using ClinicManagement.Shared.DTOs;
using System.Net.Http.Json;

namespace ClinicManagement.Client.Services
{
    public class AppointmentClientService
    {
        private readonly HttpClient _httpClient;

        public AppointmentClientService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<AppointmentDto>> GetAppointmentsAsync() =>
            await _httpClient.GetFromJsonAsync<List<AppointmentDto>>("api/appointments") ?? new List<AppointmentDto>();

        public async Task<AppointmentDto> GetAppointmentByIdAsync(int id) =>
            await _httpClient.GetFromJsonAsync<AppointmentDto>($"api/appointments/{id}");

        public async Task CreateAppointmentAsync(AppointmentDto appointmentDto) =>
            await _httpClient.PostAsJsonAsync("api/appointments", appointmentDto);

        public async Task UpdateAppointmentAsync(int id, AppointmentDto appointmentDto) =>
            await _httpClient.PutAsJsonAsync($"api/appointments/{id}", appointmentDto);

        public async Task DeleteAppointmentAsync(int id) =>
            await _httpClient.DeleteAsync($"api/appointments/{id}");
    }
}