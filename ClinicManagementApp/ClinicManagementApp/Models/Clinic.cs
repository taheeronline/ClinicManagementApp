// ClinicManagementApp/Models/Clinic.cs
namespace ClinicManagementApp.Models
{
    public class Clinic
    {
        public int Id { get; set; }
        public string Name { get; set; } = "My Clinic";
        public string Address { get; set; } = string.Empty;
        public string LandlineNumber { get; set; } = string.Empty;
        public string MobileNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string GoogleMapLocation { get; set; } = string.Empty;
    }
}