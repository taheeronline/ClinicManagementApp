// ClinicManagement.Client/Services/PatientRecordClientService.cs
using ClinicManagement.Shared.DTOs;
using System.Net.Http.Json;

namespace ClinicManagement.Client.Services
{
    public class PatientRecordClientService
    {
        private readonly HttpClient _httpClient;

        public PatientRecordClientService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<PatientRecordDto>> GetRecordsAsync() =>
            await _httpClient.GetFromJsonAsync<List<PatientRecordDto>>("api/patientrecords") ?? new List<PatientRecordDto>();

        public async Task<PatientRecordDto> GetRecordByIdAsync(int id) =>
            await _httpClient.GetFromJsonAsync<PatientRecordDto>($"api/patientrecords/{id}");

        public async Task<List<PatientRecordDto>> GetRecordsByPatientIdAsync(int patientId) =>
            await _httpClient.GetFromJsonAsync<List<PatientRecordDto>>($"api/patientrecords/patient/{patientId}") ?? new List<PatientRecordDto>();

        public async Task CreateRecordAsync(PatientRecordDto recordDto) =>
            await _httpClient.PostAsJsonAsync("api/patientrecords", recordDto);

        public async Task UpdateRecordAsync(int id, PatientRecordDto recordDto) =>
            await _httpClient.PutAsJsonAsync($"api/patientrecords/{id}", recordDto);

        public async Task DeleteRecordAsync(int id) =>
            await _httpClient.DeleteAsync($"api/patientrecords/{id}");

        public async Task CompleteConsultationAsync(ConsultationSaveDto dto) =>
            await _httpClient.PostAsJsonAsync("api/patientrecords/complete-consultation", dto);
    }
}