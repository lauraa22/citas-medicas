namespace CitasMedicas.Application.DTOs.Usuarios;

/// <summary>
/// Representa los datos necesarios para actualizar un usuario.
/// </summary>
public class UsuarioUpdateDto
{
    public string Nombre { get; set; } = string.Empty;

    public string Apellidos { get; set; } = string.Empty;

    public string Usuario { get; set; } = string.Empty;

    public string? Clave { get; set; } = string.Empty;
}