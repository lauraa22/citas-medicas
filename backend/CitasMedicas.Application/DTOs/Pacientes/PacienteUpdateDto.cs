namespace CitasMedicas.Application.DTOs.Pacientes;

/// <summary>
/// Representa los datos necesarios para actualizar un paciente.
/// </summary>
public class PacienteUpdateDto
{
    public string Nombre { get; set; } = string.Empty;

    public string Apellidos { get; set; } = string.Empty;

    public string Usuario { get; set; } = string.Empty;

    public string? Clave { get; set; } = string.Empty;

    public string NSS { get; set; } = string.Empty;

    public string NumTarjeta { get; set; } = string.Empty;

    public string Telefono { get; set; } = string.Empty;

    public string Direccion { get; set; } = string.Empty;

    /// <summary>
    /// Identificadores de los médicos asociados al paciente.
    /// </summary>
    public List<int> MedicoIds { get; set; } = [];
}