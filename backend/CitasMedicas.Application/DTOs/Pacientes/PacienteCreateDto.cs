namespace CitasMedicas.Application.DTOs.Pacientes;

/// <summary>
/// Representa los datos necesarios para crear un paciente.
/// </summary>
public class PacienteCreateDto
{
    public string Nombre { get; set; } = string.Empty;

    public string Apellidos { get; set; } = string.Empty;

    public string Usuario { get; set; } = string.Empty;

    public string Clave { get; set; } = string.Empty;

    public string NSS { get; set; } = string.Empty;

    public string NumTarjeta { get; set; } = string.Empty;

    public string Telefono { get; set; } = string.Empty;

    public string Direccion { get; set; } = string.Empty;

    /// <summary>
    /// Identificadores de los médicos que se desean asociar.
    /// Puede estar vacío.
    /// </summary>
    public List<int> MedicoIds { get; set; } = [];
}