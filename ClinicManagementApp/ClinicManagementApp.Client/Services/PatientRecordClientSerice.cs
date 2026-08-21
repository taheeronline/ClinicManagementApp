using ClinicManagement.Shared.DTOs;
using System.Net.Http.Json;
using System.Text.Json;

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

        public async Task<PatientRecordDetailsDto> GetRecordDetailsAsync(int id) =>
            await _httpClient.GetFromJsonAsync<PatientRecordDetailsDto>($"api/patientrecords/{id}/details") ?? new PatientRecordDetailsDto();

        public async Task CreateRecordAsync(PatientRecordDto recordDto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/patientrecords", recordDto);
            await HandleErrorsAsync(response);
        }

        public async Task UpdateRecordAsync(int id, PatientRecordDto recordDto)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/patientrecords/{id}", recordDto);
            await HandleErrorsAsync(response);
        }

        public async Task DeleteRecordAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/patientrecords/{id}");
            await HandleErrorsAsync(response);
        }

        public async Task CompleteConsultationAsync(ConsultationSaveDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/patientrecords/complete-consultation", dto);
            await HandleErrorsAsync(response);
        }

        // Upgraded Bulletproof Error Handler
        private async Task HandleErrorsAsync(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                string finalErrorMessage = "An error occurred while processing your request.";

                if (!string.IsNullOrWhiteSpace(errorContent))
                {
                    try
                    {
                        // Attempt to parse as JSON
                        using var doc = JsonDocument.Parse(errorContent);
                        var root = doc.RootElement;

                        if (root.TryGetProperty("detail", out var detail) && detail.ValueKind == JsonValueKind.String)
                            finalErrorMessage = detail.GetString()!;
                        else if (root.TryGetProperty("Detail", out var detailUpper) && detailUpper.ValueKind == JsonValueKind.String)
                            finalErrorMessage = detailUpper.GetString()!;
                        else if (root.TryGetProperty("message", out var message) && message.ValueKind == JsonValueKind.String)
                            finalErrorMessage = message.GetString()!;
                        else if (root.TryGetProperty("title", out var title) && title.ValueKind == JsonValueKind.String)
                            finalErrorMessage = title.GetString()!;
                    }
                    catch
                    {
                        // If it fails to parse as JSON, the API likely returned plain text.
                        // As long as it's not a massive HTML error page, we can just display it!
                        if (!errorContent.Trim().StartsWith("<", StringComparison.OrdinalIgnoreCase))
                        {
                            finalErrorMessage = errorContent;
                        }
                    }
                }

                // Throw the exception OUTSIDE the try/catch block so it actually reaches the UI
                throw new Exception(finalErrorMessage);
            }
        }
    }
}