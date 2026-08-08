// ClinicManagementApp/Controllers/DoctorsController.cs
using ClinicManagement.Shared.DTOs;
using ClinicManagementApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagementApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorsController : ControllerBase
    {
        private readonly IDoctorService _doctorService;

        public DoctorsController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        [HttpGet]
        public async Task<ActionResult<List<DoctorDto>>> GetDoctors()
        {
            return Ok(await _doctorService.GetAllDoctorsAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DoctorDto>> GetDoctor(int id)
        {
            return Ok(await _doctorService.GetDoctorByIdAsync(id));
        }

        [HttpPost]
        public async Task<ActionResult<DoctorDto>> CreateDoctor(DoctorDto doctorDto)
        {
            var createdDoctor = await _doctorService.CreateDoctorAsync(doctorDto);
            return CreatedAtAction(nameof(GetDoctor), new { id = createdDoctor.Id }, createdDoctor);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDoctor(int id, DoctorDto doctorDto)
        {
            await _doctorService.UpdateDoctorAsync(id, doctorDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDoctor(int id)
        {
            await _doctorService.DeleteDoctorAsync(id);
            return NoContent();
        }
    }
}