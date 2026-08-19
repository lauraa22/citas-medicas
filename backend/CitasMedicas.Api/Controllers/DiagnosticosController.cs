using AutoMapper;
using CitasMedicas.Api.DTOs;
using CitasMedicas.Application.Models;
using CitasMedicas.Application.UseCases.Diagnosticos;
using Microsoft.AspNetCore.Mvc;

namespace CitasMedicas.Api.Controllers;

/// <summary>
/// Controlador encargado de la gestión de diagnósticos.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class DiagnosticosController : ControllerBase
{
    private readonly GetDiagnosticosUseCase _getDiagnosticosUseCase;
    private readonly GetDiagnosticoUseCase _getDiagnosticoUseCase;
    private readonly CreateDiagnosticoUseCase _createDiagnosticoUseCase;
    private readonly UpdateDiagnosticoUseCase _updateDiagnosticoUseCase;
    private readonly DeleteDiagnosticoUseCase _deleteDiagnosticoUseCase;
    private readonly IMapper _mapper;

    /// <summary>
    /// Inicializa una nueva instancia del controlador de diagnósticos.
    /// </summary>
    /// <param name="getDiagnosticosUseCase">
    /// Caso de uso encargado de obtener todos los diagnósticos.
    /// </param>
    /// <param name="getDiagnosticoUseCase">
    /// Caso de uso encargado de obtener un diagnóstico.
    /// </param>
    /// <param name="createDiagnosticoUseCase">
    /// Caso de uso encargado de crear un diagnóstico.
    /// </param>
    /// <param name="updateDiagnosticoUseCase">
    /// Caso de uso encargado de actualizar un diagnóstico.
    /// </param>
    /// <param name="deleteDiagnosticoUseCase">
    /// Caso de uso encargado de eliminar un diagnóstico.
    /// </param>
    /// <param name="mapper">
    /// Mapper utilizado para convertir DTOs y modelos de aplicación.
    /// </param>
    public DiagnosticosController(
        GetDiagnosticosUseCase getDiagnosticosUseCase,
        GetDiagnosticoUseCase getDiagnosticoUseCase,
        CreateDiagnosticoUseCase createDiagnosticoUseCase,
        UpdateDiagnosticoUseCase updateDiagnosticoUseCase,
        DeleteDiagnosticoUseCase deleteDiagnosticoUseCase,
        IMapper mapper)
    {
        _getDiagnosticosUseCase = getDiagnosticosUseCase;
        _getDiagnosticoUseCase = getDiagnosticoUseCase;
        _createDiagnosticoUseCase = createDiagnosticoUseCase;
        _updateDiagnosticoUseCase = updateDiagnosticoUseCase;
        _deleteDiagnosticoUseCase = deleteDiagnosticoUseCase;
        _mapper = mapper;
    }

    /// <summary>
    /// Obtiene todos los diagnósticos registrados.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(
        typeof(IEnumerable<DiagnosticoDto>),
        StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<DiagnosticoDto>>> GetAll()
    {
        var diagnosticos =
            await _getDiagnosticosUseCase.ExecuteAsync();

        return Ok(
            _mapper.Map<IEnumerable<DiagnosticoDto>>(diagnosticos));
    }

    /// <summary>
    /// Obtiene un diagnóstico por su identificador.
    /// </summary>
    /// <param name="id">
    /// Identificador del diagnóstico.
    /// </param>
    [HttpGet("{id:int}")]
    [ProducesResponseType(
        typeof(DiagnosticoDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(
        StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DiagnosticoDto>> GetById(
        int id)
    {
        var diagnostico =
            await _getDiagnosticoUseCase.ExecuteAsync(id);

        if (diagnostico is null)
            return NotFound();

        return Ok(
            _mapper.Map<DiagnosticoDto>(diagnostico));
    }

    /// <summary>
    /// Crea un nuevo diagnóstico.
    /// </summary>
    /// <param name="dto">
    /// Datos del diagnóstico.
    /// </param>
    [HttpPost]
    [ProducesResponseType(
        typeof(DiagnosticoDto),
        StatusCodes.Status201Created)]
    public async Task<ActionResult<DiagnosticoDto>> Create(
        DiagnosticoDto dto)
    {
        var model =
            _mapper.Map<DiagnosticoModel>(dto);

        var diagnostico =
            await _createDiagnosticoUseCase
                .ExecuteAsync(model);

        return CreatedAtAction(
            nameof(GetById),
            new { id = diagnostico.Id },
            _mapper.Map<DiagnosticoDto>(diagnostico));
    }

    /// <summary>
    /// Actualiza un diagnóstico existente.
    /// </summary>
    /// <param name="id">
    /// Identificador del diagnóstico.
    /// </param>
    /// <param name="dto">
    /// Nuevos datos del diagnóstico.
    /// </param>
    [HttpPut("{id:int}")]
    [ProducesResponseType(
        StatusCodes.Status204NoContent)]
    [ProducesResponseType(
        StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        int id,
        DiagnosticoDto dto)
    {
        var model =
            _mapper.Map<DiagnosticoModel>(dto);

        var updated =
            await _updateDiagnosticoUseCase
                .ExecuteAsync(
                    id,
                    model);

        if (!updated)
            return NotFound();

        return NoContent();
    }

    /// <summary>
    /// Elimina un diagnóstico.
    /// </summary>
    /// <param name="id">
    /// Identificador del diagnóstico.
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
            await _deleteDiagnosticoUseCase
                .ExecuteAsync(id);

        if (!deleted)
            return NotFound();

        return NoContent();
    }
}