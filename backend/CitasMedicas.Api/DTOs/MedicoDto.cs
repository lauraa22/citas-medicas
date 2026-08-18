using System.Text.Json.Serialization;

namespace CitasMedicas.Api.DTOs;

/// <summary>
/// Representa los datos de un médico que se intercambian
/// entre la API y el cliente.
/// </summary>
public class MedicoDto
{
    /// <summary>
    /// Identificador único del médico.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Nombre del médico.
    /// </summary>
    public string Nombre { get; set; } = string.Empty;

    /// <summary>
    /// Apellidos del médico.
    /// </summary>
    public string Apellidos { get; set; } = string.Empty;

    /// <summary>
    /// Nombre de usuario del médico.
    /// </summary>
    public string Usuario { get; set; } = string.Empty;

    /// <summary>
    /// Clave del médico.
    /// Puede enviarse en creación o actualización,
    /// pero no se devuelve en las respuestas.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Clave { get; set; }

    /// <summary>
    /// Número de colegiado del médico.
    /// </summary>
    public string NumColegiado { get; set; } = string.Empty;

    /// <summary>
    /// Identificadores de los pacientes asociados al médico.
    /// </summary>
    public List<int> PacienteIds { get; set; } = [];
}