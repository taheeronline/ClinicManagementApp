using Blazored.LocalStorage;
using ClinicManagement.Client.AuthHelper;
using ClinicManagement.Shared.DTOs;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;

namespace ClinicManagement.Client.Services
{
    public class AuthClientService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly ILocalStorageService _localStorage;

        public AuthClientService(HttpClient httpClient, AuthenticationStateProvider authStateProvider, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _authStateProvider = authStateProvider;
            _localStorage = localStorage;
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/auth/login", loginDto);

                // Try to safely parse the response into our AuthResponseDto. 
                // Our AuthController returns this DTO even on a 401 Unauthorized!
                try
                {
                    var authResponse = await response.Content.ReadFromJsonAsync<AuthResponseDto>();

                    if (authResponse != null)
                    {
                        // If login was successful, save the token and notify the app state
                        if (response.IsSuccessStatusCode && authResponse.IsSuccessful)
                        {
                            await _localStorage.SetItemAsync("authToken", authResponse.Token);
                            ((CustomAuthStateProvider)_authStateProvider).NotifyUserLoggedIn(authResponse.Token);
                        }

                        // Return the response (whether it's a success or a 401 Unauthorized with an error message)
                        return authResponse;
                    }
                }
                catch
                {
                    // If JSON parsing fails, the server sent a generic error (e.g., a 500 Server Crash).
                    // We just ignore the parse failure and drop down to the fallback below.
                }

                // Fallback for unexpected server responses (like 400 Bad Request or 500 Internal Server Error)
                return new AuthResponseDto
                {
                    IsSuccessful = false,
                    ErrorMessage = $"Server error or invalid request. (Status Code: {response.StatusCode})"
                };
            }
            catch (Exception ex)
            {
                // This catches scenarios where the API is completely offline or unreachable
                return new AuthResponseDto
                {
                    IsSuccessful = false,
                    ErrorMessage = "Could not connect to the server. Please check your connection."
                };
            }
        }

        public async Task LogoutAsync()
        {
            await _localStorage.RemoveItemAsync("authToken");
            ((CustomAuthStateProvider)_authStateProvider).NotifyUserLoggedOut();
        }
    }
}