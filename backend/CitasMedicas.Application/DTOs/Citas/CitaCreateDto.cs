namespace CitasMedicas.Application.DTOs.Citas;

/// <summary>
/// Representa los datos necesarios para crear una cita médica.
/// </summary>
public class CitaCreateDto
{
    public DateTime FechaHora { get; set; }

    public string MotivoCita { get; set; } = string.Empty;

    public int PacienteId { get; set; }

    public int MedicoId { get; set; }

    /// <summary>
    /// Diagnóstico asociado a la cita.
    /// Puede ser nulo al crearla.
    /// </summary>
    public int? DiagnosticoId { get; set; }
}