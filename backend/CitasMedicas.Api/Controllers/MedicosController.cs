using AutoMapper;
using CitasMedicas.Api.DTOs;
using CitasMedicas.Application.Models;
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
    private readonly IMapper _mapper;

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
    /// <param name="mapper">
    /// Mapper utilizado para convertir DTOs y modelos de aplicación.
    /// </param>
    public MedicosController(
        GetMedicosUseCase getMedicosUseCase,
        GetMedicoUseCase getMedicoUseCase,
        CreateMedicoUseCase createMedicoUseCase,
        UpdateMedicoUseCase updateMedicoUseCase,
        DeleteMedicoUseCase deleteMedicoUseCase,
        IMapper mapper)
    {
        _getMedicosUseCase = getMedicosUseCase;
        _getMedicoUseCase = getMedicoUseCase;
        _createMedicoUseCase = createMedicoUseCase;
        _updateMedicoUseCase = updateMedicoUseCase;
        _deleteMedicoUseCase = deleteMedicoUseCase;
        _mapper = mapper;
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
            _mapper.Map<IEnumerable<MedicoDto>>(medicos));
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

        return Ok(
            _mapper.Map<MedicoDto>(medico));
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
        var model =
            _mapper.Map<MedicoModel>(dto);

        var medico =
            await _createMedicoUseCase
                .ExecuteAsync(model);

        return CreatedAtAction(
            nameof(GetById),
            new { id = medico.Id },
            _mapper.Map<MedicoDto>(medico));
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
        var model =
            _mapper.Map<MedicoModel>(dto);

        var updated =
            await _updateMedicoUseCase
                .ExecuteAsync(
                    id,
                    model);

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