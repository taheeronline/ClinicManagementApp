// ClinicManagement.Client/Services/PatientClientService.cs
using ClinicManagement.Shared.DTOs;
using System.Net.Http.Json;
using System.Text.Json;

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

        public async Task DeletePatientAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/patients/{id}");
            await HandleErrorsAsync(response);
        }

        private async Task HandleErrorsAsync(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                string finalErrorMessage = "An error occurred while processing your request.";

                try
                {
                    // Parse the JSON document safely
                    using var doc = JsonDocument.Parse(errorContent);
                    var root = doc.RootElement;

                    // Check for the "detail" property (case-insensitive check)
                    if (root.TryGetProperty("detail", out var detail) || root.TryGetProperty("Detail", out detail))
                    {
                        var extractedMessage = detail.GetString();
                        if (!string.IsNullOrWhiteSpace(extractedMessage))
                        {
                            finalErrorMessage = extractedMessage;
                        }
                    }
                }
                catch
                {
                    // If the server returns something that isn't JSON, we just ignore the parse failure
                }

                // Throw the exception OUTSIDE the try/catch block so it actually reaches the UI!
                throw new Exception(finalErrorMessage);
            }
        }
    }
}