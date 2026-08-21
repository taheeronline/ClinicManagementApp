using ClinicManagement.Shared.DTOs;
using System.Net.Http.Json;
using System.Text.Json;

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

        public async Task CreateDoctorAsync(DoctorDto doctorDto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/doctors", doctorDto);
            await HandleErrorsAsync(response);
        }

        public async Task UpdateDoctorAsync(int id, DoctorDto doctorDto)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/doctors/{id}", doctorDto);
            await HandleErrorsAsync(response);
        }

        public async Task DeleteDoctorAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/doctors/{id}");
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