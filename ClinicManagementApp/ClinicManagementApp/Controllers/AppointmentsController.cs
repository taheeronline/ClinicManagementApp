// ClinicManagementApp/Controllers/AppointmentsController.cs
using ClinicManagement.Shared.DTOs;
using ClinicManagementApp.Services;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize(Roles = "Admin,Manager,Receptionist,Staff")]
        [HttpPost]
        public async Task<ActionResult<AppointmentDto>> CreateAppointment(AppointmentDto appointmentDto)
        {
            try
            {
                var createdAppointment = await _appointmentService.CreateAppointmentAsync(appointmentDto);
                return CreatedAtAction(nameof(GetAppointment), new { id = createdAppointment.Id }, createdAppointment);
            }
            catch (Exception ex)
            {

                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "Admin,Manager,Receptionist,Staff")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAppointment(int id, AppointmentDto appointmentDto)
        {
            try
            {
                await _appointmentService.UpdateAppointmentAsync(id, appointmentDto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "Admin,Manager,Receptionist,Staff")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            try
            {
                await _appointmentService.DeleteAppointmentAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "Admin,Manager,Receptionist,Staff")]
        [HttpPost("mark-noshows")]
        public async Task<ActionResult<int>> MarkNoShows()
        {
            var count = await _appointmentService.MarkOverdueAsNoShowAsync();
            return Ok(count);
        }

        [Authorize(Roles = "Admin,Manager,Receptionist,Staff")]
        [HttpPut("{id}/cancel")]
        public async Task<IActionResult> CancelAppointment(int id)
        {
            try
            {
                await _appointmentService.CancelAppointmentAsync(id);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            return NoContent();
        }
    }
}