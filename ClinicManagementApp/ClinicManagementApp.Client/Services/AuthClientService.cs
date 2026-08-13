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
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", loginDto);
            var authResponse = await response.Content.ReadFromJsonAsync<AuthResponseDto>();

            if (response.IsSuccessStatusCode && authResponse!.IsSuccessful)
            {
                await _localStorage.SetItemAsync("authToken", authResponse.Token);
                ((CustomAuthStateProvider)_authStateProvider).NotifyUserLoggedIn(authResponse.Token);
            }
            return authResponse!;
        }

        public async Task LogoutAsync()
        {
            await _localStorage.RemoveItemAsync("authToken");
            ((CustomAuthStateProvider)_authStateProvider).NotifyUserLoggedOut();
        }
    }
}