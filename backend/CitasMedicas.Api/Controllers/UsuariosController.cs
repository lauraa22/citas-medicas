using CitasMedicas.Application.DTOs.Usuarios;
using CitasMedicas.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace CitasMedicas.Api.Controllers;

/// <summary>
/// Controlador para la gestión de usuarios.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class UsuariosController : ControllerBase
{
    private readonly IUsuarioService _usuarioService;

    /// <summary>
    /// Inicializa el controlador de usuarios.
    /// </summary>
    /// <param name="usuarioService">
    /// Servicio encargado de la lógica de negocio de usuarios.
    /// </param>
    public UsuariosController(
        IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    /// <summary>
    /// Obtiene todos los usuarios.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(
        typeof(IEnumerable<UsuarioDto>),
        StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<UsuarioDto>>> GetAll()
    {
        var usuarios =
            await _usuarioService.GetAllAsync();

        return Ok(usuarios);
    }

    /// <summary>
    /// Obtiene un usuario por su identificador.
    /// </summary>
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
            await _usuarioService.GetByIdAsync(id);

        if (usuario is null)
            return NotFound();

        return Ok(usuario);
    }

    /// <summary>
    /// Crea un nuevo usuario.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(
        typeof(UsuarioDto),
        StatusCodes.Status201Created)]
    [ProducesResponseType(
        StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UsuarioDto>> Create(
        UsuarioCreateDto dto)
    {
        try
        {
            var usuario =
                await _usuarioService.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = usuario.Id },
                usuario);
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
    /// Actualiza un usuario.
    /// </summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(
        StatusCodes.Status204NoContent)]
    [ProducesResponseType(
        StatusCodes.Status400BadRequest)]
    [ProducesResponseType(
        StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        int id,
        UsuarioUpdateDto dto)
    {
        try
        {
            var updated =
                await _usuarioService.UpdateAsync(
                    id,
                    dto);

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
    /// Elimina un usuario.
    /// </summary>
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
        try
        {
            var deleted =
                await _usuarioService.DeleteAsync(id);

            if (!deleted)
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
}