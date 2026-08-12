namespace CitasMedicas.Application.DTOs.Pacientes;

/// <summary>
/// Representa los datos de un paciente enviados por la API.
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
    /// Nombre de usuario.
    /// </summary>
    public string Usuario { get; set; } = string.Empty;

    /// <summary>
    /// Número de la Seguridad Social.
    /// </summary>
    public string NSS { get; set; } = string.Empty;

    /// <summary>
    /// Número de tarjeta sanitaria.
    /// </summary>
    public string NumTarjeta { get; set; } = string.Empty;

    /// <summary>
    /// Teléfono de contacto.
    /// </summary>
    public string Telefono { get; set; } = string.Empty;

    /// <summary>
    /// Dirección del paciente.
    /// </summary>
    public string Direccion { get; set; } = string.Empty;

    /// <summary>
    /// Identificadores de los médicos relacionados con el paciente.
    /// </summary>
    public List<int> MedicoIds { get; set; } = [];
}