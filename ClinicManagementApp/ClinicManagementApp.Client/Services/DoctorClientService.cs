// ClinicManagement.Client/Services/DoctorClientService.cs
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
            await HandleErrorsAsync(response); // <-- Added error handling
        }

        public async Task UpdateDoctorAsync(int id, DoctorDto doctorDto)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/doctors/{id}", doctorDto);
            await HandleErrorsAsync(response); // <-- Added error handling
        }
        public async Task DeleteDoctorAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/doctors/{id}");
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