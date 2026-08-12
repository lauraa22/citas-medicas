using CitasMedicas.Application.DTOs.Usuarios;

namespace CitasMedicas.Application.Interfaces.Services;

/// <summary>
/// Define las operaciones disponibles para la gestión de usuarios.
/// </summary>
public interface IUsuarioService
{
    /// <summary>
    /// Obtiene todos los usuarios.
    /// </summary>
    Task<IEnumerable<UsuarioDto>> GetAllAsync();

    /// <summary>
    /// Obtiene un usuario por su identificador.
    /// </summary>
    Task<UsuarioDto?> GetByIdAsync(int id);

    /// <summary>
    /// Crea un nuevo usuario.
    /// </summary>
    Task<UsuarioDto> CreateAsync(UsuarioCreateDto dto);

    /// <summary>
    /// Actualiza un usuario existente.
    /// </summary>
    Task<bool> UpdateAsync(int id, UsuarioUpdateDto dto);

    /// <summary>
    /// Elimina un usuario.
    /// </summary>
    Task<bool> DeleteAsync(int id);
}