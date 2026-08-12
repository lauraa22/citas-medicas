using CitasMedicas.Application.DTOs.Medicos;
using CitasMedicas.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace CitasMedicas.Api.Controllers;

/// <summary>
/// Controlador encargado de la gestión de médicos.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class MedicosController : ControllerBase
{
    private readonly IMedicoService _medicoService;

    /// <summary>
    /// Inicializa el controlador de médicos.
    /// </summary>
    /// <param name="medicoService">
    /// Servicio encargado de la lógica de negocio de médicos.
    /// </param>
    public MedicosController(IMedicoService medicoService)
    {
        _medicoService = medicoService;
    }

    /// <summary>
    /// Obtiene todos los médicos registrados.
    /// </summary>
    /// <returns>Listado de médicos.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<MedicoDto>),
        StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<MedicoDto>>> GetAll()
    {
        var medicos =
            await _medicoService.GetAllAsync();

        return Ok(medicos);
    }

    /// <summary>
    /// Obtiene un médico por su identificador.
    /// </summary>
    /// <param name="id">Identificador del médico.</param>
    /// <returns>Médico solicitado.</returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(MedicoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MedicoDto>> GetById(int id)
    {
        var medico =
            await _medicoService.GetByIdAsync(id);

        if (medico is null)
            return NotFound();

        return Ok(medico);
    }

    /// <summary>
    /// Crea un nuevo médico.
    /// </summary>
    /// <param name="dto">Datos necesarios para crear el médico.</param>
    /// <returns>Médico creado.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(MedicoDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

    /// <summary>
    /// Actualiza los datos de un médico.
    /// </summary>
    /// <param name="id">Identificador del médico.</param>
    /// <param name="dto">Nuevos datos del médico.</param>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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

    /// <summary>
    /// Elimina un médico.
    /// </summary>
    /// <param name="id">Identificador del médico.</param>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted =
            await _medicoService.DeleteAsync(id);

        if (!deleted)
            return NotFound();

        return NoContent();
    }
}