using Blazored.LocalStorage;
using ClinicManagement.Client.Auth;
using ClinicManagement.Client.AuthHelper;
using ClinicManagement.Client.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddScoped<AuthHeaderHandler>();

// Replace the existing HttpClient registration with this:
builder.Services.AddScoped(sp =>
{
    var handler = sp.GetRequiredService<AuthHeaderHandler>();
    handler.InnerHandler = new HttpClientHandler();
    return new HttpClient(handler) { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) };
});

builder.Services.AddScoped<AuthClientService>();
builder.Services.AddScoped<ClinicClientService>();
builder.Services.AddScoped<DoctorClientService>();
builder.Services.AddScoped<PatientClientService>();
builder.Services.AddScoped<AppointmentClientService>();
builder.Services.AddScoped<PatientRecordClientService>();
builder.Services.AddScoped<BillClientService>();
builder.Services.AddSingleton<LoadingService>();
builder.Services.AddScoped<UserClientService>();

await builder.Build().RunAsync();
