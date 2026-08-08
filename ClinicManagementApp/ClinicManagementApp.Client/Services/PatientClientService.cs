// ClinicManagement.Client/Services/PatientClientService.cs
using ClinicManagement.Shared.DTOs;
using System.Net.Http.Json;

namespace ClinicManagement.Client.Services
{
    public class PatientClientService
    {
        private readonly HttpClient _httpClient;

        public PatientClientService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<PatientDto>> GetPatientsAsync() =>
            await _httpClient.GetFromJsonAsync<List<PatientDto>>("api/patients") ?? new List<PatientDto>();

        public async Task<PatientDto> GetPatientByIdAsync(int id) =>
            await _httpClient.GetFromJsonAsync<PatientDto>($"api/patients/{id}");

        public async Task CreatePatientAsync(PatientDto patientDto) =>
            await _httpClient.PostAsJsonAsync("api/patients", patientDto);

        public async Task UpdatePatientAsync(int id, PatientDto patientDto) =>
            await _httpClient.PutAsJsonAsync($"api/patients/{id}", patientDto);

        public async Task DeletePatientAsync(int id) =>
            await _httpClient.DeleteAsync($"api/patients/{id}");
    }
}