// ClinicManagement.Client/Services/BillClientService.cs
using ClinicManagement.Shared.DTOs;
using System.Net.Http.Json;
using System.Text.Json;

namespace ClinicManagement.Client.Services
{
    public class BillClientService
    {
        private readonly HttpClient _httpClient;

        public BillClientService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ConsultationBillDto>> GetBillsAsync() =>
            await _httpClient.GetFromJsonAsync<List<ConsultationBillDto>>("api/bills") ?? new List<ConsultationBillDto>();

        public async Task GenerateBillAsync(int appointmentId) =>
            await _httpClient.PostAsync($"api/bills/generate/{appointmentId}", null);

        public async Task MarkAsPaidAsync(int id)
        {
            var response = await _httpClient.PutAsync($"api/bills/{id}/pay", null);
            await HandleErrorsAsync(response);
        }

        public async Task DeleteBillAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/bills/{id}");
            await HandleErrorsAsync(response);
        }

        // Helper method to catch the Business Rule Exceptions from the global handler
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