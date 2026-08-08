// ClinicManagementApp/Exceptions/DoctorNotFoundException.cs
namespace ClinicManagementApp.Exceptions
{
    public class PatientNotFoundException : Exception
    {
        public PatientNotFoundException(int patientId)
            : base($"Patient with ID {patientId} was not found in the database.")
        {
        }
    }
}