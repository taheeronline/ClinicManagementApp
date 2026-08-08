// ClinicManagementApp/Exceptions/PatientRecordNotFoundException.cs
namespace ClinicManagementApp.Exceptions
{
    public class PatientRecordNotFoundException : Exception
    {
        public PatientRecordNotFoundException(int recordId)
            : base($"Patient Record with ID {recordId} was not found.")
        {
        }
    }
}