using CitasMedicas.Application.DTOs.Diagnosticos;
using CitasMedicas.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace CitasMedicas.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DiagnosticosController : ControllerBase
{
    private readonly IDiagnosticoService _diagnosticoService;

    public DiagnosticosController(
        IDiagnosticoService diagnosticoService)
    {
        _diagnosticoService = diagnosticoService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DiagnosticoDto>>> GetAll()
    {
        var diagnosticos =
            await _diagnosticoService.GetAllAsync();

        return Ok(diagnosticos);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<DiagnosticoDto>> GetById(int id)
    {
        var diagnostico =
            await _diagnosticoService.GetByIdAsync(id);

        if (diagnostico is null)
            return NotFound();

        return Ok(diagnostico);
    }

    [HttpPost]
    public async Task<ActionResult<DiagnosticoDto>> Create(
        DiagnosticoCreateDto dto)
    {
        var diagnostico =
            await _diagnosticoService.CreateAsync(dto);

        return CreatedAtAction(
            nameof(GetById),
            new { id = diagnostico.Id },
            diagnostico);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        DiagnosticoUpdateDto dto)
    {
        var updated =
            await _diagnosticoService.UpdateAsync(id, dto);

        if (!updated)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted =
            await _diagnosticoService.DeleteAsync(id);

        if (!deleted)
            return NotFound();

        return NoContent();
    }
}