using CitasMedicas.Application.DTOs.Pacientes;
using CitasMedicas.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace CitasMedicas.Api.Controllers;

/// <summary>
/// Controlador para la gestión de pacientes.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class PacientesController : ControllerBase
{
    private readonly IPacienteService _pacienteService;

    /// <summary>
    /// Inicializa una nueva instancia del controlador de pacientes.
    /// </summary>
    /// <param name="pacienteService">
    /// Servicio encargado de gestionar las operaciones relacionadas con los pacientes.
    /// </param>
    public PacientesController(IPacienteService pacienteService)
    {
        _pacienteService = pacienteService;
    }

    /// <summary>
    /// Obtiene todos los pacientes.
    /// </summary>
    /// <returns>Listado de pacientes.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PacienteDto>>> GetAll()
    {
        var pacientes = await _pacienteService.GetAllAsync();

        return Ok(pacientes);
    }

    /// <summary>
    /// Obtiene un paciente por su identificador.
    /// </summary>
    /// <param name="id">Identificador del paciente.</param>
    /// <returns>Paciente encontrado.</returns>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<PacienteDto>> GetById(int id)
    {
        var paciente = await _pacienteService.GetByIdAsync(id);

        if (paciente is null)
            return NotFound();

        return Ok(paciente);
    }

    /// <summary>
    /// Crea un nuevo paciente.
    /// </summary>
    /// <param name="dto">Datos del paciente.</param>
    /// <returns>Paciente creado.</returns>
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

    /// <summary>
    /// Actualiza un paciente existente.
    /// </summary>
    /// <param name="id">Identificador del paciente.</param>
    /// <param name="dto">Nuevos datos del paciente.</param>
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

    /// <summary>
    /// Elimina un paciente.
    /// </summary>
    /// <param name="id">Identificador del paciente.</param>
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