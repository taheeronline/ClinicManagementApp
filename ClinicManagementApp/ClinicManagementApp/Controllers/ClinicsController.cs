// ClinicManagementApp/Controllers/ClinicController.cs
using ClinicManagement.Shared.DTOs;
using ClinicManagementApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagementApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClinicController : ControllerBase
    {
        private readonly IClinicService _clinicService;

        public ClinicController(IClinicService clinicService)
        {
            _clinicService = clinicService;
        }

        [HttpGet]
        public async Task<ActionResult<ClinicDto>> GetClinic() => Ok(await _clinicService.GetClinicDetailsAsync());

        [HttpPut]
        public async Task<IActionResult> UpdateClinic(ClinicDto clinicDto)
        {
            await _clinicService.UpdateClinicDetailsAsync(clinicDto);
            return NoContent();
        }
    }
}