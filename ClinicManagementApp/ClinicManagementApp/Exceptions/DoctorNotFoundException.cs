// ClinicManagementApp/Exceptions/DoctorNotFoundException.cs
namespace ClinicManagementApp.Exceptions
{
    public class DoctorNotFoundException : Exception
    {
        public DoctorNotFoundException(int doctorId)
            : base($"Doctor with ID {doctorId} was not found in the database.")
        {
        }
    }
}