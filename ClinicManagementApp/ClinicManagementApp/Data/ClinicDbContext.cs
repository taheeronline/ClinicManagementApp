// ClinicManagementApp/Data/ClinicDbContext.cs
using ClinicManagementApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagementApp.Data
{
    public class ClinicDbContext : DbContext
    {
        public ClinicDbContext(DbContextOptions<ClinicDbContext> options) : base(options)
        {
        }

        // DbSets represent the actual tables in your database
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
    }
}