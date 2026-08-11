// ClinicManagementApp/Controllers/AppointmentsController.cs
using ClinicManagement.Shared.DTOs;
using ClinicManagementApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagementApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentsController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpGet]
        public async Task<ActionResult<List<AppointmentDto>>> GetAppointments() => Ok(await _appointmentService.GetAllAppointmentsAsync());

        [HttpGet("{id}")]
        public async Task<ActionResult<AppointmentDto>> GetAppointment(int id) => Ok(await _appointmentService.GetAppointmentByIdAsync(id));

        [HttpPost]
        public async Task<ActionResult<AppointmentDto>> CreateAppointment(AppointmentDto appointmentDto)
        {
            var createdAppointment = await _appointmentService.CreateAppointmentAsync(appointmentDto);
            return CreatedAtAction(nameof(GetAppointment), new { id = createdAppointment.Id }, createdAppointment);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAppointment(int id, AppointmentDto appointmentDto)
        {
            await _appointmentService.UpdateAppointmentAsync(id, appointmentDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            await _appointmentService.DeleteAppointmentAsync(id);
            return NoContent();
        }

        [HttpPost("mark-noshows")]
        public async Task<ActionResult<int>> MarkNoShows()
        {
            var count = await _appointmentService.MarkOverdueAsNoShowAsync();
            return Ok(count);
        }
    }
}