// ClinicManagementApp/Services/ClinicService.cs
using ClinicManagementApp.Data;
using ClinicManagementApp.Models;
using ClinicManagement.Shared.DTOs;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagementApp.Services
{
    public class ClinicService : IClinicService
    {
        private readonly ClinicDbContext _context;

        public ClinicService(ClinicDbContext context)
        {
            _context = context;
        }

        public async Task<ClinicDto> GetClinicDetailsAsync()
        {
            // Always get the first record (since we seeded Id = 1)
            var clinic = await _context.Clinics.FirstOrDefaultAsync();

            if (clinic == null) return new ClinicDto(); // Fallback safety

            return new ClinicDto
            {
                Id = clinic.Id,
                Name = clinic.Name,
                Address = clinic.Address,
                LandlineNumber = clinic.LandlineNumber,
                MobileNumber = clinic.MobileNumber,
                Email = clinic.Email,
                GoogleMapLocation = clinic.GoogleMapLocation
            };
        }

        public async Task UpdateClinicDetailsAsync(ClinicDto clinicDto)
        {
            var clinic = await _context.Clinics.FirstOrDefaultAsync();

            if (clinic != null)
            {
                clinic.Name = clinicDto.Name;
                clinic.Address = clinicDto.Address;
                clinic.LandlineNumber = clinicDto.LandlineNumber;
                clinic.MobileNumber = clinicDto.MobileNumber;
                clinic.Email = clinicDto.Email;
                clinic.GoogleMapLocation = clinicDto.GoogleMapLocation;

                await _context.SaveChangesAsync();
            }
        }
    }
}