// ClinicManagement.Client/Services/UserClientService.cs
using ClinicManagement.Shared.DTOs;
using System.Net.Http.Json;
using System.Text.Json;

namespace ClinicManagement.Client.Services
{
    public class UserClientService
    {
        private readonly HttpClient _httpClient;

        public UserClientService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<UserDto>> GetUsersAsync() =>
            await _httpClient.GetFromJsonAsync<List<UserDto>>("api/users") ?? new List<UserDto>();

        public async Task<UserDto> GetUserByIdAsync(int id) =>
            await _httpClient.GetFromJsonAsync<UserDto>($"api/users/{id}");

        public async Task CreateUserAsync(UserDto userDto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/users", userDto);
            await HandleErrorsAsync(response);
        }

        public async Task UpdateUserAsync(int id, UserDto userDto)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/users/{id}", userDto);
            await HandleErrorsAsync(response);
        }

        public async Task DeleteUserAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/users/{id}");
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
                    using var doc = JsonDocument.Parse(errorContent);
                    if (doc.RootElement.TryGetProperty("detail", out var detail) || doc.RootElement.TryGetProperty("Detail", out detail))
                    {
                        var extractedMessage = detail.GetString();
                        if (!string.IsNullOrWhiteSpace(extractedMessage)) finalErrorMessage = extractedMessage;
                    }
                }
                catch { }

                throw new Exception(finalErrorMessage);
            }
        }
    }
}