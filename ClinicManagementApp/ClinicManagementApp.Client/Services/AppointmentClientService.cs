using ClinicManagement.Shared.DTOs;
using System.Net.Http.Json;
using System.Text.Json;

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

        public async Task CreateAppointmentAsync(AppointmentDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/appointments", dto);
            await HandleErrorsAsync(response);
        }

        public async Task UpdateAppointmentAsync(int id, AppointmentDto dto)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/appointments/{id}", dto);
            await HandleErrorsAsync(response);
        }

        public async Task DeleteAppointmentAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/appointments/{id}");
            await HandleErrorsAsync(response);
        }
        public async Task<int> MarkOverdueAsNoShowAsync()
        {
            var response = await _httpClient.PostAsync("api/appointments/mark-noshows", null);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<int>();
            }
            return 0;
        }

        // Helper method to extract the ProblemDetails string
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