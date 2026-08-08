// ClinicManagement.Client/Services/DoctorClientService.cs
using ClinicManagement.Shared.DTOs;
using System.Net.Http.Json;

namespace ClinicManagement.Client.Services
{
    public class DoctorClientService
    {
        private readonly HttpClient _httpClient;

        public DoctorClientService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<DoctorDto>> GetDoctorsAsync() =>
            await _httpClient.GetFromJsonAsync<List<DoctorDto>>("api/doctors") ?? new List<DoctorDto>();

        public async Task<DoctorDto> GetDoctorByIdAsync(int id) =>
            await _httpClient.GetFromJsonAsync<DoctorDto>($"api/doctors/{id}");

        public async Task CreateDoctorAsync(DoctorDto doctorDto) =>
            await _httpClient.PostAsJsonAsync("api/doctors", doctorDto);

        public async Task UpdateDoctorAsync(int id, DoctorDto doctorDto) =>
            await _httpClient.PutAsJsonAsync($"api/doctors/{id}", doctorDto);

        public async Task DeleteDoctorAsync(int id) =>
            await _httpClient.DeleteAsync($"api/doctors/{id}");
    }
}