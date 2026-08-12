using CitasMedicas.Application.DTOs.Medicos;
using CitasMedicas.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace CitasMedicas.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MedicosController : ControllerBase
{
    private readonly IMedicoService _medicoService;

    public MedicosController(IMedicoService medicoService)
    {
        _medicoService = medicoService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MedicoDto>>> GetAll()
    {
        var medicos =
            await _medicoService.GetAllAsync();

        return Ok(medicos);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<MedicoDto>> GetById(int id)
    {
        var medico =
            await _medicoService.GetByIdAsync(id);

        if (medico is null)
            return NotFound();

        return Ok(medico);
    }

    [HttpPost]
    public async Task<ActionResult<MedicoDto>> Create(
        MedicoCreateDto dto)
    {
        try
        {
            var medico =
                await _medicoService.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = medico.Id },
                medico);
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
        MedicoUpdateDto dto)
    {
        try
        {
            var updated =
                await _medicoService.UpdateAsync(id, dto);

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
            await _medicoService.DeleteAsync(id);

        if (!deleted)
            return NotFound();

        return NoContent();
    }
}