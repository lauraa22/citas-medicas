namespace CitasMedicas.Application.DTOs.Medicos;

/// <summary>
/// Representa los datos necesarios para crear un médico.
/// </summary>
public class MedicoCreateDto
{
    public string Nombre { get; set; } = string.Empty;

    public string Apellidos { get; set; } = string.Empty;

    public string Usuario { get; set; } = string.Empty;

    public string Clave { get; set; } = string.Empty;

    public string NumColegiado { get; set; } = string.Empty;

    /// <summary>
    /// Identificadores de pacientes que se desean asociar.
    /// Puede estar vacío.
    /// </summary>
    public List<int> PacienteIds { get; set; } = [];
}