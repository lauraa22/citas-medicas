using CitasMedicas.Application.DTOs.Pacientes;
using CitasMedicas.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace CitasMedicas.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PacientesController : ControllerBase
{
    private readonly IPacienteService _pacienteService;

    public PacientesController(IPacienteService pacienteService)
    {
        _pacienteService = pacienteService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PacienteDto>>> GetAll()
    {
        var pacientes = await _pacienteService.GetAllAsync();

        return Ok(pacientes);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PacienteDto>> GetById(int id)
    {
        var paciente = await _pacienteService.GetByIdAsync(id);

        if (paciente is null)
            return NotFound();

        return Ok(paciente);
    }

    [HttpPost]
    public async Task<ActionResult<PacienteDto>> Create(
        PacienteCreateDto dto)
    {
        try
        {
            var paciente =
                await _pacienteService.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = paciente.Id },
                paciente);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        PacienteUpdateDto dto)
    {
        try
        {
            var updated =
                await _pacienteService.UpdateAsync(id, dto);

            if (!updated)
                return NotFound();

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted =
            await _pacienteService.DeleteAsync(id);

        if (!deleted)
            return NotFound();

        return NoContent();
    }
}