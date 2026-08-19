using AutoMapper;
using CitasMedicas.Api.DTOs;
using CitasMedicas.Application.Models;
using CitasMedicas.Application.UseCases.Pacientes;
using Microsoft.AspNetCore.Mvc;

namespace CitasMedicas.Api.Controllers;

/// <summary>
/// Controlador encargado de la gestión de pacientes.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class PacientesController : ControllerBase
{
    private readonly GetPacientesUseCase _getPacientesUseCase;
    private readonly GetPacienteUseCase _getPacienteUseCase;
    private readonly CreatePacienteUseCase _createPacienteUseCase;
    private readonly UpdatePacienteUseCase _updatePacienteUseCase;
    private readonly DeletePacienteUseCase _deletePacienteUseCase;
    private readonly IMapper _mapper;

    /// <summary>
    /// Inicializa una nueva instancia del controlador de pacientes.
    /// </summary>
    /// <param name="getPacientesUseCase">
    /// Caso de uso encargado de obtener todos los pacientes.
    /// </param>
    /// <param name="getPacienteUseCase">
    /// Caso de uso encargado de obtener un paciente.
    /// </param>
    /// <param name="createPacienteUseCase">
    /// Caso de uso encargado de crear un paciente.
    /// </param>
    /// <param name="updatePacienteUseCase">
    /// Caso de uso encargado de actualizar un paciente.
    /// </param>
    /// <param name="deletePacienteUseCase">
    /// Caso de uso encargado de eliminar un paciente.
    /// </param>
    /// <param name="mapper">
    /// Mapper utilizado para convertir DTOs y modelos de aplicación.
    /// </param>
    public PacientesController(
        GetPacientesUseCase getPacientesUseCase,
        GetPacienteUseCase getPacienteUseCase,
        CreatePacienteUseCase createPacienteUseCase,
        UpdatePacienteUseCase updatePacienteUseCase,
        DeletePacienteUseCase deletePacienteUseCase,
        IMapper mapper)
    {
        _getPacientesUseCase = getPacientesUseCase;
        _getPacienteUseCase = getPacienteUseCase;
        _createPacienteUseCase = createPacienteUseCase;
        _updatePacienteUseCase = updatePacienteUseCase;
        _deletePacienteUseCase = deletePacienteUseCase;
        _mapper = mapper;
    }

    /// <summary>
    /// Obtiene todos los pacientes registrados.
    /// </summary>
    /// <returns>
    /// Listado de pacientes registrados.
    /// </returns>
    [HttpGet]
    [ProducesResponseType(
        typeof(IEnumerable<PacienteDto>),
        StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PacienteDto>>> GetAll()
    {
        var pacientes =
            await _getPacientesUseCase.ExecuteAsync();

        return Ok(
            _mapper.Map<IEnumerable<PacienteDto>>(pacientes));
    }

    /// <summary>
    /// Obtiene un paciente por su identificador.
    /// </summary>
    /// <param name="id">
    /// Identificador del paciente.
    /// </param>
    /// <returns>
    /// Paciente solicitado.
    /// </returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(
        typeof(PacienteDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(
        StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PacienteDto>> GetById(
        int id)
    {
        var paciente =
            await _getPacienteUseCase.ExecuteAsync(id);

        if (paciente is null)
            return NotFound();

        return Ok(
            _mapper.Map<PacienteDto>(paciente));
    }

    /// <summary>
    /// Crea un nuevo paciente.
    /// </summary>
    /// <param name="dto">
    /// Datos del paciente.
    /// </param>
    /// <returns>
    /// Paciente creado.
    /// </returns>
    [HttpPost]
    [ProducesResponseType(
        typeof(PacienteDto),
        StatusCodes.Status201Created)]
    [ProducesResponseType(
        StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PacienteDto>> Create(
        PacienteDto dto)
    {
        var model =
            _mapper.Map<PacienteModel>(dto);

        var paciente =
            await _createPacienteUseCase
                .ExecuteAsync(model);

        return CreatedAtAction(
            nameof(GetById),
            new { id = paciente.Id },
            _mapper.Map<PacienteDto>(paciente));
    }

    /// <summary>
    /// Actualiza un paciente existente.
    /// </summary>
    /// <param name="id">
    /// Identificador del paciente.
    /// </param>
    /// <param name="dto">
    /// Nuevos datos del paciente.
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
        PacienteDto dto)
    {
        var model =
            _mapper.Map<PacienteModel>(dto);

        var updated =
            await _updatePacienteUseCase
                .ExecuteAsync(
                    id,
                    model);

        if (!updated)
            return NotFound();

        return NoContent();
    }

    /// <summary>
    /// Elimina un paciente.
    /// </summary>
    /// <param name="id">
    /// Identificador del paciente.
    /// </param>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(
        StatusCodes.Status204NoContent)]
    [ProducesResponseType(
        StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(
        int id)
    {
        var deleted =
            await _deletePacienteUseCase
                .ExecuteAsync(id);

        if (!deleted)
            return NotFound();

        return NoContent();
    }
}