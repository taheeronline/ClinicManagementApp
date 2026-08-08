// ClinicManagementApp/Controllers/PatientRecordsController.cs
using ClinicManagement.Shared.DTOs;
using ClinicManagementApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagementApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientRecordsController : ControllerBase
    {
        private readonly IPatientRecordService _recordService;

        public PatientRecordsController(IPatientRecordService recordService)
        {
            _recordService = recordService;
        }

        [HttpGet]
        public async Task<ActionResult<List<PatientRecordDto>>> GetRecords() => Ok(await _recordService.GetAllRecordsAsync());

        [HttpGet("{id}")]
        public async Task<ActionResult<PatientRecordDto>> GetRecord(int id) => Ok(await _recordService.GetRecordByIdAsync(id));

        [HttpGet("patient/{patientId}")]
        public async Task<ActionResult<List<PatientRecordDto>>> GetRecordsByPatient(int patientId) =>
            Ok(await _recordService.GetRecordsByPatientIdAsync(patientId));

        [HttpPost]
        public async Task<ActionResult<PatientRecordDto>> CreateRecord(PatientRecordDto recordDto)
        {
            var created = await _recordService.CreateRecordAsync(recordDto);
            return CreatedAtAction(nameof(GetRecord), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRecord(int id, PatientRecordDto recordDto)
        {
            await _recordService.UpdateRecordAsync(id, recordDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecord(int id)
        {
            await _recordService.DeleteRecordAsync(id);
            return NoContent();
        }

        [HttpPost("complete-consultation")]
        public async Task<IActionResult> CompleteConsultation(ConsultationSaveDto dto)
        {
            await _recordService.CompleteConsultationAsync(dto);
            return Ok();
        }
    }
}