using CitasMedicas.Application.DTOs.Citas;
using CitasMedicas.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace CitasMedicas.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CitasController : ControllerBase
{
    private readonly ICitaService _citaService;

    public CitasController(ICitaService citaService)
    {
        _citaService = citaService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CitaDto>>> GetAll()
    {
        var citas =
            await _citaService.GetAllAsync();

        return Ok(citas);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CitaDto>> GetById(int id)
    {
        var cita =
            await _citaService.GetByIdAsync(id);

        if (cita is null)
            return NotFound();

        return Ok(cita);
    }

    [HttpPost]
    public async Task<ActionResult<CitaDto>> Create(
        CitaCreateDto dto)
    {
        try
        {
            var cita =
                await _citaService.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = cita.Id },
                cita);
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
        CitaUpdateDto dto)
    {
        try
        {
            var updated =
                await _citaService.UpdateAsync(id, dto);

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
            await _citaService.DeleteAsync(id);

        if (!deleted)
            return NotFound();

        return NoContent();
    }
}