// ClinicManagementApp/Services/IBillService.cs
using ClinicManagement.Shared.DTOs;

namespace ClinicManagementApp.Services
{
    public interface IBillService
    {
        Task<List<ConsultationBillDto>> GetAllBillsAsync();
        Task<ConsultationBillDto> GenerateBillAsync(int appointmentId);
        Task MarkAsPaidAsync(int billId);
    }
}