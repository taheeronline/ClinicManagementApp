// ClinicManagementApp/Controllers/BillsController.cs
using ClinicManagement.Shared.DTOs;
using ClinicManagementApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagementApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BillsController : ControllerBase
    {
        private readonly IBillService _billService;

        public BillsController(IBillService billService)
        {
            _billService = billService;
        }

        [HttpGet]
        public async Task<ActionResult<List<ConsultationBillDto>>> GetBills() => Ok(await _billService.GetAllBillsAsync());

        [HttpPost("generate/{appointmentId}")]
        public async Task<ActionResult<ConsultationBillDto>> GenerateBill(int appointmentId)
        {
            try
            {
                var bill = await _billService.GenerateBillAsync(appointmentId);
                return Ok(bill);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}/pay")]
        public async Task<IActionResult> MarkAsPaid(int id)
        {
            await _billService.MarkAsPaidAsync(id);
            return NoContent();
        }
    }
}