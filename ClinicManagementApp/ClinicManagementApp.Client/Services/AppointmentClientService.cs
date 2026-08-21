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

                // Throw the exception OUTSIDE the try/catch block so it actually reaches the UI!
                throw new Exception(finalErrorMessage);
            }
        }

        // Add this to AppointmentClientService.cs
        public async Task CancelAppointmentAsync(int id)
        {
            var response = await _httpClient.PutAsync($"api/appointments/{id}/cancel", null);
            await HandleErrorsAsync(response);
        }
    }
}