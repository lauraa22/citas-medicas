using AutoMapper;
using CitasMedicas.Api.DTOs;
using CitasMedicas.Application.Models;
using CitasMedicas.Application.UseCases.Usuarios;
using Microsoft.AspNetCore.Mvc;

namespace CitasMedicas.Api.Controllers;

/// <summary>
/// Controlador encargado de la gestión de usuarios.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class UsuariosController : ControllerBase
{
    private readonly GetUsuariosUseCase _getUsuariosUseCase;
    private readonly GetUsuarioUseCase _getUsuarioUseCase;
    private readonly CreateUsuarioUseCase _createUsuarioUseCase;
    private readonly UpdateUsuarioUseCase _updateUsuarioUseCase;
    private readonly DeleteUsuarioUseCase _deleteUsuarioUseCase;
    private readonly IMapper _mapper;

    /// <summary>
    /// Inicializa una nueva instancia del controlador de usuarios.
    /// </summary>
    /// <param name="getUsuariosUseCase">
    /// Caso de uso encargado de obtener todos los usuarios.
    /// </param>
    /// <param name="getUsuarioUseCase">
    /// Caso de uso encargado de obtener un usuario.
    /// </param>
    /// <param name="createUsuarioUseCase">
    /// Caso de uso encargado de crear un usuario.
    /// </param>
    /// <param name="updateUsuarioUseCase">
    /// Caso de uso encargado de actualizar un usuario.
    /// </param>
    /// <param name="deleteUsuarioUseCase">
    /// Caso de uso encargado de eliminar un usuario.
    /// </param>
    /// <param name="mapper">
    /// Mapper utilizado para convertir DTOs y modelos de aplicación.
    /// </param>
    public UsuariosController(
        GetUsuariosUseCase getUsuariosUseCase,
        GetUsuarioUseCase getUsuarioUseCase,
        CreateUsuarioUseCase createUsuarioUseCase,
        UpdateUsuarioUseCase updateUsuarioUseCase,
        DeleteUsuarioUseCase deleteUsuarioUseCase,
        IMapper mapper)
    {
        _getUsuariosUseCase = getUsuariosUseCase;
        _getUsuarioUseCase = getUsuarioUseCase;
        _createUsuarioUseCase = createUsuarioUseCase;
        _updateUsuarioUseCase = updateUsuarioUseCase;
        _deleteUsuarioUseCase = deleteUsuarioUseCase;
        _mapper = mapper;
    }

    /// <summary>
    /// Obtiene todos los usuarios registrados.
    /// </summary>
    /// <returns>
    /// Listado de usuarios.
    /// </returns>
    [HttpGet]
    [ProducesResponseType(
        typeof(IEnumerable<UsuarioDto>),
        StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<UsuarioDto>>> GetAll()
    {
        var usuarios =
            await _getUsuariosUseCase.ExecuteAsync();

        return Ok(
            _mapper.Map<IEnumerable<UsuarioDto>>(usuarios));
    }

    /// <summary>
    /// Obtiene un usuario por su identificador.
    /// </summary>
    /// <param name="id">
    /// Identificador del usuario.
    /// </param>
    /// <returns>
    /// Usuario solicitado.
    /// </returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(
        typeof(UsuarioDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(
        StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UsuarioDto>> GetById(
        int id)
    {
        var usuario =
            await _getUsuarioUseCase.ExecuteAsync(id);

        if (usuario is null)
            return NotFound();

        return Ok(
            _mapper.Map<UsuarioDto>(usuario));
    }

    /// <summary>
    /// Crea un nuevo usuario.
    /// </summary>
    /// <param name="dto">
    /// Datos del usuario.
    /// </param>
    /// <returns>
    /// Usuario creado.
    /// </returns>
    [HttpPost]
    [ProducesResponseType(
        typeof(UsuarioDto),
        StatusCodes.Status201Created)]
    [ProducesResponseType(
        StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UsuarioDto>> Create(
        UsuarioDto dto)
    {
        var model =
            _mapper.Map<UsuarioModel>(dto);

        var usuario =
            await _createUsuarioUseCase
                .ExecuteAsync(model);

        return CreatedAtAction(
            nameof(GetById),
            new { id = usuario.Id },
            _mapper.Map<UsuarioDto>(usuario));
    }

    /// <summary>
    /// Actualiza un usuario existente.
    /// </summary>
    /// <param name="id">
    /// Identificador del usuario.
    /// </param>
    /// <param name="dto">
    /// Nuevos datos del usuario.
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
        UsuarioDto dto)
    {
        var model =
            _mapper.Map<UsuarioModel>(dto);

        var updated =
            await _updateUsuarioUseCase
                .ExecuteAsync(
                    id,
                    model);

        if (!updated)
            return NotFound();

        return NoContent();
    }

    /// <summary>
    /// Elimina un usuario.
    /// </summary>
    /// <param name="id">
    /// Identificador del usuario.
    /// </param>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(
        StatusCodes.Status204NoContent)]
    [ProducesResponseType(
        StatusCodes.Status400BadRequest)]
    [ProducesResponseType(
        StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(
        int id)
    {
        var deleted =
            await _deleteUsuarioUseCase.ExecuteAsync(id);

        if (!deleted)
            return NotFound();

        return NoContent();
    }
}