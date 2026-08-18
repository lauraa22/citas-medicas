using System.Text.Json.Serialization;

namespace CitasMedicas.Api.DTOs;

/// <summary>
/// Representa los datos de un usuario que se intercambian
/// entre la API y el cliente.
/// </summary>
public class UsuarioDto
{
    /// <summary>
    /// Identificador único del usuario.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Nombre del usuario.
    /// </summary>
    public string Nombre { get; set; } = string.Empty;

    /// <summary>
    /// Apellidos del usuario.
    /// </summary>
    public string Apellidos { get; set; } = string.Empty;

    /// <summary>
    /// Nombre de usuario utilizado para acceder al sistema.
    /// </summary>
    public string Usuario { get; set; } = string.Empty;

    /// <summary>
    /// Clave del usuario.
    /// Puede enviarse en operaciones de creación o actualización,
    /// pero no se devuelve en las respuestas de la API.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Clave { get; set; }
}