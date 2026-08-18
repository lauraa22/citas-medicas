using System.Text.Json.Serialization;

namespace CitasMedicas.Api.DTOs;

/// <summary>
/// Representa los datos de un paciente que se intercambian
/// entre la API y el cliente.
/// </summary>
public class PacienteDto
{
    /// <summary>
    /// Identificador único del paciente.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Nombre del paciente.
    /// </summary>
    public string Nombre { get; set; } = string.Empty;

    /// <summary>
    /// Apellidos del paciente.
    /// </summary>
    public string Apellidos { get; set; } = string.Empty;

    /// <summary>
    /// Nombre de usuario del paciente.
    /// </summary>
    public string Usuario { get; set; } = string.Empty;

    /// <summary>
    /// Clave del paciente.
    /// Puede enviarse en creación o actualización,
    /// pero no se devuelve en las respuestas.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Clave { get; set; }

    /// <summary>
    /// Número de la Seguridad Social del paciente.
    /// </summary>
    public string NSS { get; set; } = string.Empty;

    /// <summary>
    /// Número de tarjeta sanitaria del paciente.
    /// </summary>
    public string NumTarjeta { get; set; } = string.Empty;

    /// <summary>
    /// Número de teléfono del paciente.
    /// </summary>
    public string Telefono { get; set; } = string.Empty;

    /// <summary>
    /// Dirección del paciente.
    /// </summary>
    public string Direccion { get; set; } = string.Empty;

    /// <summary>
    /// Identificadores de los médicos asociados al paciente.
    /// </summary>
    public List<int> MedicoIds { get; set; } = [];
}