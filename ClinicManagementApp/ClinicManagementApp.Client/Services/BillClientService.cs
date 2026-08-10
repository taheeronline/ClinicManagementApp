// ClinicManagement.Client/Services/BillClientService.cs
using ClinicManagement.Shared.DTOs;
using System.Net.Http.Json;

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

        public async Task MarkAsPaidAsync(int id) =>
            await _httpClient.PutAsync($"api/bills/{id}/pay", null);
    }
}