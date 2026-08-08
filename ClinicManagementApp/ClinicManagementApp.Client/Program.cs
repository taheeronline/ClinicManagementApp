using ClinicManagement.Client.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<DoctorClientService>();
builder.Services.AddScoped<PatientClientService>();
builder.Services.AddScoped<AppointmentClientService>();

await builder.Build().RunAsync();
