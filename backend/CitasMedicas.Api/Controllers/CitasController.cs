using CitasMedicas.Application.DTOs.Citas;
using CitasMedicas.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace CitasMedicas.Api.Controllers;

/// <summary>
/// Controlador encargado de la gestión de citas médicas.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CitasController : ControllerBase
{
    private readonly ICitaService _citaService;

    /// <summary>
    /// Inicializa el controlador de citas.
    /// </summary>
    /// <param name="citaService">
    /// Servicio encargado de la lógica de negocio de citas.
    /// </param>
    public CitasController(ICitaService citaService)
    {
        _citaService = citaService;
    }

    /// <summary>
    /// Obtiene todas las citas registradas.
    /// </summary>
    /// <returns>Listado de citas.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CitaDto>),
        StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<CitaDto>>> GetAll()
    {
        var citas =
            await _citaService.GetAllAsync();

        return Ok(citas);
    }

    /// <summary>
    /// Obtiene una cita por su identificador.
    /// </summary>
    /// <param name="id">Identificador de la cita.</param>
    /// <returns>Cita solicitada.</returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(CitaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CitaDto>> GetById(int id)
    {
        var cita =
            await _citaService.GetByIdAsync(id);

        if (cita is null)
            return NotFound();

        return Ok(cita);
    }

    /// <summary>
    /// Crea una nueva cita.
    /// </summary>
    /// <remarks>
    /// La cita debe estar asociada a un paciente y un médico.
    /// El diagnóstico es opcional en el momento de creación.
    /// </remarks>
    /// <param name="dto">Datos necesarios para crear la cita.</param>
    /// <returns>Cita creada.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(CitaDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

    /// <summary>
    /// Actualiza una cita existente.
    /// </summary>
    /// <param name="id">Identificador de la cita.</param>
    /// <param name="dto">Nuevos datos de la cita.</param>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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

    /// <summary>
    /// Elimina una cita.
    /// </summary>
    /// <param name="id">Identificador de la cita.</param>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted =
            await _citaService.DeleteAsync(id);

        if (!deleted)
            return NotFound();

        return NoContent();
    }
}