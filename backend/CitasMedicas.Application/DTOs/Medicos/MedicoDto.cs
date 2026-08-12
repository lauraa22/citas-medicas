namespace CitasMedicas.Application.DTOs.Medicos;

/// <summary>
/// Representa los datos de un médico devueltos por la aplicación.
/// </summary>
public class MedicoDto
{
    public int Id { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public string Apellidos { get; set; } = string.Empty;

    public string Usuario { get; set; } = string.Empty;

    /// <summary>
    /// Número de colegiado del médico.
    /// </summary>
    public string NumColegiado { get; set; } = string.Empty;

    /// <summary>
    /// Identificadores de los pacientes asociados al médico.
    /// </summary>
    public List<int> PacienteIds { get; set; } = [];
}