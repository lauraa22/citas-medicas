namespace CitasMedicas.Application.DTOs.Citas;

/// <summary>
/// Representa los datos de una cita médica.
/// </summary>
public class CitaDto
{
    public int Id { get; set; }

    /// <summary>
    /// Fecha y hora de la cita.
    /// </summary>
    public DateTime FechaHora { get; set; }

    /// <summary>
    /// Motivo de la cita.
    /// </summary>
    public string MotivoCita { get; set; } = string.Empty;

    /// <summary>
    /// Identificador del paciente asociado.
    /// </summary>
    public int PacienteId { get; set; }

    /// <summary>
    /// Identificador del médico asociado.
    /// </summary>
    public int MedicoId { get; set; }

    /// <summary>
    /// Identificador del diagnóstico asociado.
    /// Puede ser nulo mientras la cita no tenga diagnóstico.
    /// </summary>
    public int? DiagnosticoId { get; set; }
}