// ClinicManagementApp/Exceptions/BillNotFoundException.cs
namespace ClinicManagementApp.Exceptions
{
    public class BillNotFoundException : Exception
    {
        public BillNotFoundException(int billId)
            : base($"Bill with ID {billId} was not found.") { }
    }
}