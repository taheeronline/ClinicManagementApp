// ClinicManagement.Client/Services/ClinicClientService.cs
using ClinicManagement.Shared.DTOs;
using System.Net.Http.Json;

namespace ClinicManagement.Client.Services
{
    public class ClinicClientService
    {
        private readonly HttpClient _httpClient;

        public ClinicClientService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ClinicDto> GetClinicDetailsAsync() =>
            await _httpClient.GetFromJsonAsync<ClinicDto>("api/clinic") ?? new ClinicDto();

        public async Task UpdateClinicDetailsAsync(ClinicDto clinicDto) =>
            await _httpClient.PutAsJsonAsync("api/clinic", clinicDto);
    }
}