namespace CW_9_S262535.Controllers;
using DTOs;
using Exceptions;
using Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class PrescriptionsController : ControllerBase
{
    private readonly IDbService service;

    public PrescriptionsController(IDbService service)
    {
        this.service = service;
    }

    [HttpPost]
    public async Task<IActionResult> AddPrescription([FromBody] PrescriptionCreateDto dto)
    {
        try
        {
            var result = await service.AddPrescriptionAsync(dto);
            return Created("/prescriptions/" + result.IdPrescription, result);
        }
        catch (BadRequestException e)
        {
            return BadRequest(e.Message);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpGet("{idPatient}")]
    public async Task<IActionResult> GetPatientDetails([FromRoute] int idPatient)
    {
        try
        {
            var result = await service.GetPatientByIdAsync(idPatient);
            return Ok(result);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
}