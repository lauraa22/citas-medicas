using CitasMedicas.Api.DTOs;
using CitasMedicas.Api.Mappings;
using CitasMedicas.Application.UseCases.Medicos;
using Microsoft.AspNetCore.Mvc;

namespace CitasMedicas.Api.Controllers;

/// <summary>
/// Controlador encargado de la gestión de médicos.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class MedicosController : ControllerBase
{
    private readonly GetMedicosUseCase _getMedicosUseCase;
    private readonly GetMedicoUseCase _getMedicoUseCase;
    private readonly CreateMedicoUseCase _createMedicoUseCase;
    private readonly UpdateMedicoUseCase _updateMedicoUseCase;
    private readonly DeleteMedicoUseCase _deleteMedicoUseCase;

    /// <summary>
    /// Inicializa una nueva instancia del controlador de médicos.
    /// </summary>
    /// <param name="getMedicosUseCase">
    /// Caso de uso para obtener todos los médicos.
    /// </param>
    /// <param name="getMedicoUseCase">
    /// Caso de uso para obtener un médico.
    /// </param>
    /// <param name="createMedicoUseCase">
    /// Caso de uso para crear un médico.
    /// </param>
    /// <param name="updateMedicoUseCase">
    /// Caso de uso para actualizar un médico.
    /// </param>
    /// <param name="deleteMedicoUseCase">
    /// Caso de uso para eliminar un médico.
    /// </param>
    public MedicosController(
        GetMedicosUseCase getMedicosUseCase,
        GetMedicoUseCase getMedicoUseCase,
        CreateMedicoUseCase createMedicoUseCase,
        UpdateMedicoUseCase updateMedicoUseCase,
        DeleteMedicoUseCase deleteMedicoUseCase)
    {
        _getMedicosUseCase = getMedicosUseCase;
        _getMedicoUseCase = getMedicoUseCase;
        _createMedicoUseCase = createMedicoUseCase;
        _updateMedicoUseCase = updateMedicoUseCase;
        _deleteMedicoUseCase = deleteMedicoUseCase;
    }

    /// <summary>
    /// Obtiene todos los médicos registrados.
    /// </summary>
    /// <returns>
    /// Listado de médicos registrados.
    /// </returns>
    [HttpGet]
    [ProducesResponseType(
        typeof(IEnumerable<MedicoDto>),
        StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<MedicoDto>>> GetAll()
    {
        var medicos =
            await _getMedicosUseCase.ExecuteAsync();

        return Ok(
            medicos.Select(
                medico => medico.ToDto()));
    }

    /// <summary>
    /// Obtiene un médico por su identificador.
    /// </summary>
    /// <param name="id">
    /// Identificador del médico.
    /// </param>
    /// <returns>
    /// Médico solicitado.
    /// </returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(
        typeof(MedicoDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(
        StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MedicoDto>> GetById(
        int id)
    {
        var medico =
            await _getMedicoUseCase.ExecuteAsync(id);

        if (medico is null)
            return NotFound();

        return Ok(medico.ToDto());
    }

    /// <summary>
    /// Crea un nuevo médico.
    /// </summary>
    /// <param name="dto">
    /// Datos del médico.
    /// </param>
    /// <returns>
    /// Médico creado.
    /// </returns>
    [HttpPost]
    [ProducesResponseType(
        typeof(MedicoDto),
        StatusCodes.Status201Created)]
    [ProducesResponseType(
        StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<MedicoDto>> Create(
        MedicoDto dto)
    {
        var medico =
            await _createMedicoUseCase
                .ExecuteAsync(dto.ToModel());

        return CreatedAtAction(
            nameof(GetById),
            new { id = medico.Id },
            medico.ToDto());
    }

    /// <summary>
    /// Actualiza un médico existente.
    /// </summary>
    /// <param name="id">
    /// Identificador del médico.
    /// </param>
    /// <param name="dto">
    /// Nuevos datos del médico.
    /// </param>
    [HttpPut("{id:int}")]
    [ProducesResponseType(
        StatusCodes.Status204NoContent)]
    [ProducesResponseType(
        StatusCodes.Status400BadRequest)]
    [ProducesResponseType(
        StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        int id,
        MedicoDto dto)
    {
        var updated =
            await _updateMedicoUseCase
                .ExecuteAsync(
                    id,
                    dto.ToModel());

        if (!updated)
            return NotFound();

        return NoContent();
    }

    /// <summary>
    /// Elimina un médico.
    /// </summary>
    /// <param name="id">
    /// Identificador del médico.
    /// </param>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(
        StatusCodes.Status204NoContent)]
    [ProducesResponseType(
        StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted =
            await _deleteMedicoUseCase.ExecuteAsync(id);

        if (!deleted)
            return NotFound();

        return NoContent();
    }
}