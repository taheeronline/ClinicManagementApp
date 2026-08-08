// ClinicManagementApp/Controllers/PatientsController.cs
using ClinicManagement.Shared.DTOs;
using ClinicManagementApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagementApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientService _patientService;

        public PatientsController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        [HttpGet]
        public async Task<ActionResult<List<PatientDto>>> GetPatients() => Ok(await _patientService.GetAllPatientsAsync());

        [HttpGet("{id}")]
        public async Task<ActionResult<PatientDto>> GetPatient(int id) => Ok(await _patientService.GetPatientByIdAsync(id));

        [HttpPost]
        public async Task<ActionResult<PatientDto>> CreatePatient(PatientDto patientDto)
        {
            var createdPatient = await _patientService.CreatePatientAsync(patientDto);
            return CreatedAtAction(nameof(GetPatient), new { id = createdPatient.Id }, createdPatient);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePatient(int id, PatientDto patientDto)
        {
            await _patientService.UpdatePatientAsync(id, patientDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePatient(int id)
        {
            await _patientService.DeletePatientAsync(id);
            return NoContent();
        }
    }
}