using CitasMedicas.Application.DTOs.Diagnosticos;
using CitasMedicas.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace CitasMedicas.Api.Controllers;

/// <summary>
/// Controlador encargado de la gestión de diagnósticos.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class DiagnosticosController : ControllerBase
{
    private readonly IDiagnosticoService _diagnosticoService;

    /// <summary>
    /// Inicializa el controlador de diagnósticos.
    /// </summary>
    public DiagnosticosController(
        IDiagnosticoService diagnosticoService)
    {
        _diagnosticoService = diagnosticoService;
    }

    /// <summary>
    /// Obtiene todos los diagnósticos registrados.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<DiagnosticoDto>),
        StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<DiagnosticoDto>>> GetAll()
    {
        var diagnosticos =
            await _diagnosticoService.GetAllAsync();

        return Ok(diagnosticos);
    }

    /// <summary>
    /// Obtiene un diagnóstico por su identificador.
    /// </summary>
    /// <param name="id">Identificador del diagnóstico.</param>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(DiagnosticoDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DiagnosticoDto>> GetById(int id)
    {
        var diagnostico =
            await _diagnosticoService.GetByIdAsync(id);

        if (diagnostico is null)
            return NotFound();

        return Ok(diagnostico);
    }

    /// <summary>
    /// Crea un nuevo diagnóstico.
    /// </summary>
    /// <param name="dto">
    /// Datos necesarios para crear el diagnóstico.
    /// </param>
    [HttpPost]
    [ProducesResponseType(typeof(DiagnosticoDto),
        StatusCodes.Status201Created)]
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

    /// <summary>
    /// Actualiza un diagnóstico existente.
    /// </summary>
    /// <param name="id">Identificador del diagnóstico.</param>
    /// <param name="dto">Nuevos datos del diagnóstico.</param>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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

    /// <summary>
    /// Elimina un diagnóstico.
    /// </summary>
    /// <param name="id">Identificador del diagnóstico.</param>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted =
            await _diagnosticoService.DeleteAsync(id);

        if (!deleted)
            return NotFound();

        return NoContent();
    }
}