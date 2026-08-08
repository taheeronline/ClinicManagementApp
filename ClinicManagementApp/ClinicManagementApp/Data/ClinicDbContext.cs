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
        public DbSet<PatientRecord> PatientRecords { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }

        // ADD THIS METHOD:
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Prevent cascade delete from Patient to PatientRecord
            modelBuilder.Entity<PatientRecord>()
                .HasOne(pr => pr.Patient)
                .WithMany()
                .HasForeignKey(pr => pr.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            // Prevent cascade delete from Appointment to PatientRecord
            modelBuilder.Entity<PatientRecord>()
                .HasOne(pr => pr.Appointment)
                .WithMany()
                .HasForeignKey(pr => pr.AppointmentId)
                .OnDelete(DeleteBehavior.Restrict);

            // NEW RULE: Prevent cascade delete for Prescriptions
            modelBuilder.Entity<Prescription>()
                .HasOne(p => p.Appointment)
                .WithMany()
                .HasForeignKey(p => p.AppointmentId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}