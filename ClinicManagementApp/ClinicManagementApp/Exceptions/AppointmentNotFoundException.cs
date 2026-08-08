namespace ClinicManagementApp.Exceptions
{
    public class AppointmentNotFoundException : Exception
    {
        public AppointmentNotFoundException(int appointmentId)
            : base($"Appointment with ID {appointmentId} was not found in the database.")
        {
        }
    }
}