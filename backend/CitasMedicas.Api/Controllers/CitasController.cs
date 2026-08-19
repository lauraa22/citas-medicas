using AutoMapper;
using CitasMedicas.Api.DTOs;
using CitasMedicas.Application.Models;
using CitasMedicas.Application.UseCases.Citas;
using Microsoft.AspNetCore.Mvc;

namespace CitasMedicas.Api.Controllers;

/// <summary>
/// Controlador encargado de la gestión de citas médicas.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CitasController : ControllerBase
{
    private readonly GetCitasUseCase _getCitasUseCase;
    private readonly GetCitaUseCase _getCitaUseCase;
    private readonly CreateCitaUseCase _createCitaUseCase;
    private readonly UpdateCitaUseCase _updateCitaUseCase;
    private readonly DeleteCitaUseCase _deleteCitaUseCase;
    private readonly IMapper _mapper;

    /// <summary>
    /// Inicializa una nueva instancia del controlador de citas.
    /// </summary>
    /// <param name="getCitasUseCase">
    /// Caso de uso para obtener todas las citas.
    /// </param>
    /// <param name="getCitaUseCase">
    /// Caso de uso para obtener una cita.
    /// </param>
    /// <param name="createCitaUseCase">
    /// Caso de uso para crear una cita.
    /// </param>
    /// <param name="updateCitaUseCase">
    /// Caso de uso para actualizar una cita.
    /// </param>
    /// <param name="deleteCitaUseCase">
    /// Caso de uso para eliminar una cita.
    /// </param>
    /// <param name="mapper">
    /// Mapper utilizado para convertir DTOs y modelos de aplicación.
    /// </param>
    public CitasController(
        GetCitasUseCase getCitasUseCase,
        GetCitaUseCase getCitaUseCase,
        CreateCitaUseCase createCitaUseCase,
        UpdateCitaUseCase updateCitaUseCase,
        DeleteCitaUseCase deleteCitaUseCase,
        IMapper mapper)
    {
        _getCitasUseCase = getCitasUseCase;
        _getCitaUseCase = getCitaUseCase;
        _createCitaUseCase = createCitaUseCase;
        _updateCitaUseCase = updateCitaUseCase;
        _deleteCitaUseCase = deleteCitaUseCase;
        _mapper = mapper;
    }

    /// <summary>
    /// Obtiene todas las citas registradas.
    /// </summary>
    /// <returns>
    /// Listado de citas.
    /// </returns>
    [HttpGet]
    [ProducesResponseType(
        typeof(IEnumerable<CitaDto>),
        StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<CitaDto>>> GetAll()
    {
        var citas =
            await _getCitasUseCase.ExecuteAsync();

        return Ok(
            _mapper.Map<IEnumerable<CitaDto>>(citas));
    }

    /// <summary>
    /// Obtiene una cita por su identificador.
    /// </summary>
    /// <param name="id">
    /// Identificador de la cita.
    /// </param>
    /// <returns>
    /// Cita solicitada.
    /// </returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(
        typeof(CitaDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(
        StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CitaDto>> GetById(int id)
    {
        var cita =
            await _getCitaUseCase.ExecuteAsync(id);

        if (cita is null)
            return NotFound();

        return Ok(
            _mapper.Map<CitaDto>(cita));
    }

    /// <summary>
    /// Crea una nueva cita médica.
    /// </summary>
    /// <param name="dto">
    /// Datos de la cita.
    /// </param>
    /// <returns>
    /// Cita creada.
    /// </returns>
    [HttpPost]
    [ProducesResponseType(
        typeof(CitaDto),
        StatusCodes.Status201Created)]
    [ProducesResponseType(
        StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CitaDto>> Create(
        CitaDto dto)
    {
        var model =
            _mapper.Map<CitaModel>(dto);

        var cita =
            await _createCitaUseCase
                .ExecuteAsync(model);

        return CreatedAtAction(
            nameof(GetById),
            new { id = cita.Id },
            _mapper.Map<CitaDto>(cita));
    }

    /// <summary>
    /// Actualiza una cita existente.
    /// </summary>
    /// <param name="id">
    /// Identificador de la cita.
    /// </param>
    /// <param name="dto">
    /// Nuevos datos de la cita.
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
        CitaDto dto)
    {
        var model =
            _mapper.Map<CitaModel>(dto);

        var updated =
            await _updateCitaUseCase
                .ExecuteAsync(
                    id,
                    model);

        if (!updated)
            return NotFound();

        return NoContent();
    }

    /// <summary>
    /// Elimina una cita existente.
    /// </summary>
    /// <param name="id">
    /// Identificador de la cita.
    /// </param>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(
        StatusCodes.Status204NoContent)]
    [ProducesResponseType(
        StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted =
            await _deleteCitaUseCase.ExecuteAsync(id);

        if (!deleted)
            return NotFound();

        return NoContent();
    }
}