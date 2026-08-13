using ClinicManagement.Client.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<ClinicClientService>();
builder.Services.AddScoped<DoctorClientService>();
builder.Services.AddScoped<PatientClientService>();
builder.Services.AddScoped<AppointmentClientService>();
builder.Services.AddScoped<PatientRecordClientService>();
builder.Services.AddScoped<BillClientService>();
builder.Services.AddSingleton<LoadingService>();

await builder.Build().RunAsync();
